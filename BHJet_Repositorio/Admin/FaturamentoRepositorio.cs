﻿using BHJet_Core.Enum;
using BHJet_Repositorio.Admin.Entidade;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BHJet_Repositorio.Admin
{
    public class FaturamentoRepositorio : RepositorioBase
    {
        /// <summary>
        /// Busca Resumo de chamados e motoristas disponiveis
        /// </summary>
        /// <param name="filtro">ValidaUsuarioFiltro</param>
        /// <returns>UsuarioEntidade</returns>
        public void GeraFaturamento(long[] idCliente, DateTime periodoFatInico, DateTime periodoFatFim)
        {
            // Cadastra Periodo Faturamento
            var idPeriodo = CadastraPeriodoFaturamento(periodoFatInico, periodoFatFim);

            // Valida
            if (idPeriodo == null)
                throw new InvalidOperationException("Não foi possível cadastrar um novo período de fatuamento.");

            // Busca Faturamentos
            using (var sqlConnection = this.InstanciaConexao())
            {
                using (var trans = sqlConnection.BeginTransaction())
                {
                    try
                    {

                        // Query
                        string query = @"--- Busca Diarias ---
		                               select  P.idRegistroDiaria,
		                               P.idTarifario, 
		                               P.idColaboradorEmpresaSistema,
		                               P.idUsuarioSolicitacao as idUsuarioFaturado,
                               		   P.decValorDiariaNegociado as decValor,
		                               P.decValorDiariaComissaoNegociado as decValorComissao
                                 from tblRegistroDiarias as P
	                               where P.dtDataHoraInicioExpediente BETWEEN @dataInicio AND @dataFim
		                             and P.intOdometroFimExpediente is not null
		                             and P.idCliente not in (select RD.idCliente from tblItemFaturamento as IT left join tblRegistroDiarias as RD ON (IT.idRegistroDiaria = RD.idRegistroDiaria))
                                     and P.idRegistroDiaria not in (select idRegistroDiaria from tblItemFaturamento where idRegistroDiaria = P.idRegistroDiaria)
                                     %clienteCondition%

		                        --- Busca Corridas ---
		                                select P.idCorrida,
                                               P.idUsuarioChamador as idUsuarioFaturado,
	                                           P.idTarifario,
	                                           P.idUsuarioColaboradorEmpresa as idColaboradorEmpresaSistema,
	                                           P.decValorFinalizado as decValor,
	                                           P.decValorComissaoNegociado as decValorComissao
	                                        from tblCorridas as P
	                                        where P.dtDataHoraInicio BETWEEN  @dataInicio AND @dataFim
                                        	  and P.dtDataHoraTermino is not null 
                                              and P.idCorrida not in (select idCorrida from tblItemFaturamento where idCorrida = P.idCorrida)
                                              %clienteCondition%";

                        // Busca por cliente
                        if (idCliente != null && idCliente.Any())
                            query = query.Replace("%clienteCondition%", " and  P.idCliente in @IDCliente");
                        else
                            query = query.Replace("%clienteCondition%", "");

                        // Query Multiple
                        using (var multi = sqlConnection.QueryMultiple(query, new
                        {
                            dataInicio = periodoFatInico,
                            dataFim = periodoFatFim,
                            IDCliente = idCliente
                        }, trans))
                        {
                            // Diarias a faturar no periodo
                            var diarias = multi.Read<ItemFaturamentoEntidade>().AsList();

                            // Corridas a faturar no periodo
                            var corridas = multi.Read<ItemFaturamentoEntidade>().AsList();

                            // Cria Periodo Faturamento
                            //var servicosUnificados = new List<ItemFaturamentoEntidade>();

                            // Faturamento unificado
                            var servicosUnificados = diarias.Union(corridas).ToList();

                            // Lista final a ser faturado
                            //var servicosRealizados = servicosUnificados.GroupBy(fat => fat.idCorrida);

                            if (servicosUnificados == null || !servicosUnificados.Any())
                                throw new InvalidOperationException("Não existe item a faturar no período informado.");

                            // Cria Item Faturamento
                            string queryItem = @"INSERT INTO [dbo].[tblItemFaturamento]
                                                         ([idCorrida]
                                                         ,[idRegistroDiaria]
                                                         ,[idTarifario]
                                                         ,[idColaboradorEmpresaSistema]
                                                         ,[idUsuarioFaturado]
                                                         ,[idPeriodoFaturamento]
                                                         ,[decValor]
                                                         ,[decValorComissao]
                                                         ,[bitFaturado])
                                                       VALUES
                                                             (@PidCorrida
                                                             ,@PidRegistroDiaria
                                                             ,@PidTarifario
                                                             ,@PidColaboradorEmpresaSistema
                                                             ,@PidUsuarioFaturado
                                                             ,@PidPeriodoFaturamento
                                                             ,@PdecValor
                                                             ,@PdecValorComissao
                                                             ,@PbitFaturado) select @@IDENTITY";
                            foreach (var nvItemFat in servicosUnificados)
                            {
                                // Execução 
                                var id = trans.Connection.ExecuteScalar<int?>(queryItem, new
                                {
                                    PidCorrida = nvItemFat.idCorrida,
                                    PidRegistroDiaria = nvItemFat.idRegistroDiaria,
                                    PidTarifario = nvItemFat.idTarifario,
                                    PidColaboradorEmpresaSistema = nvItemFat.idColaboradorEmpresaSistema,
                                    PidUsuarioFaturado = nvItemFat.idUsuarioFaturado,
                                    PidPeriodoFaturamento = idPeriodo,
                                    PdecValor = nvItemFat.decValor,
                                    PdecValorComissao = nvItemFat.decValorComissao,
                                    PbitFaturado = false
                                }, trans);
                            }
                        }

                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        throw e;
                    }
                }
            }
        }

        private int? CadastraPeriodoFaturamento(DateTime periodoFatInico, DateTime periodoFatFim)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                using (var trans = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        // Periodo Faturamento
                        string fatQuery = @"select idPeriodoFaturamento from tblPeriodoFaturamento
                                              where dtDataInicioPeriodoFaturamento = @dataInicio 
                                                and dtDataFimPeriodoFaturamento = @dataFim";
                        var idPeriodoFaturamento = trans.Connection.QueryFirstOrDefault<int?>(fatQuery, new
                        {
                            dataInicio = periodoFatInico,
                            dataFim = periodoFatFim
                        }, trans);

                        // Gera Periodo de faturamento
                        if (idPeriodoFaturamento == null)
                        {
                            string fatPerQuery = @"INSERT INTO [dbo].[tblPeriodoFaturamento]
                                                               ([dtDataInicioPeriodoFaturamento]
                                                               ,[dtDataFimPeriodoFaturamento])
                                                         VALUES
                                                               (@dataInicio
                                                               ,@dataFim) select @@IDENTITY";
                            idPeriodoFaturamento = trans.Connection.ExecuteScalar<int?>(fatPerQuery, new
                            {
                                dataInicio = periodoFatInico,
                                dataFim = periodoFatFim
                            }, trans);
                        }

                        // Commit
                        trans.Commit();

                        // Return 
                        return idPeriodoFaturamento;
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        throw e;
                    }
                }
            }
        }


        /// <summary>
        /// Busca Profissionais Disponiveis
        /// </summary>
        /// <param name="filtro">TipoProfissional</param>
        /// <returns>UsuarioEntidade</returns>
        public IEnumerable<ItemFaturamentoResumidoEntidade> BuscaItemFaturamento(long[] idClientes, TipoContrato? tipo, DateTime periodoFatInico, DateTime periodoFatFim)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query

                string query = @"-- fatu registro diaria
	                       select
                                IT.idItemFaturamento as ID, 
                                CLI.idCliente,
 	                            CLI.vcNomeFantasia AS NomeCliente,
 	                            --concat(convert(varchar(11), PF.dtDataInicioPeriodoFaturamento, 103), ' até ', convert(varchar(11),PF.dtDataFimPeriodoFaturamento,103)) as Periodo,
 	                            --'Chamados Avulsos' as TipoDescContrato,
 	                            sum(IT.decValor) as Valor
   	                            from tblItemFaturamento IT
						join tblPeriodoFaturamento PF on (IT.idPeriodoFaturamento = pf.idPeriodoFaturamento)
						join tblRegistroDiarias RG on (RG.idRegistroDiaria = it.idRegistroDiaria)
						join tblClientes CLI on (RG.idCliente = CLI.idCliente)
				where PF.dtDataInicioPeriodoFaturamento = @dataInicio  
						 and PF.dtDataFimPeriodoFaturamento = @dataFim  
						 and it.idCorrida is null
						 and IT.idRegistroDiaria is not null  %clienteCondition% 
                    group by CLI.idCliente, CLI.vcNomeFantasia, IT.decValor, IT.idItemFaturamento

                        -- fatu corridas                            
					     select
                                IT.idItemFaturamento as ID,
								CLI.idCliente as IDCliente,
 	                            CLI.vcNomeFantasia AS NomeCliente,
 	                           -- concat(convert(varchar(11), PF.dtDataInicioPeriodoFaturamento, 103), ' até ', convert(varchar(11),PF.dtDataFimPeriodoFaturamento,103)) as Periodo,
 	                            --'Chamados Avulsos' as TipoDescContrato,
 	                            IT.decValor as Valor
  	                             from tblItemFaturamento IT
						 join tblPeriodoFaturamento PF on (IT.idPeriodoFaturamento = pf.idPeriodoFaturamento)
						 join tblCorridas RG on (RG.idCorrida = it.idCorrida)
						 join tblClientes as CLI on (CLI.idCliente = rg.idCliente) 
				where PF.dtDataInicioPeriodoFaturamento = @dataInicio 
						 and PF.dtDataFimPeriodoFaturamento = @dataFim 
						 and it.idRegistroDiaria is null
						 and IT.idCorrida is not null  %clienteCondition2% 
                    group by CLI.idCliente, CLI.vcNomeFantasia, IT.decValor, IT.idItemFaturamento";

                if (idClientes != null && idClientes.Any())
                {
                    query = query.Replace("%clienteCondition%", "and CLI.idCliente in @IDCliente");
                    query = query.Replace("%clienteCondition2%", "and CLI.idCliente in @IDCliente");
                }
                else
                {
                    query = query.Replace("%clienteCondition%", " ");
                    query = query.Replace("%clienteCondition2%", " ");
                }

                // Retorno
                var retorno = new ItemFaturamentoResumidoEntidade[] { };

                // Query Multiple
                using (var multi = sqlConnection.QueryMultiple(query, new
                {
                    dataInicio = periodoFatInico,
                    dataFim = periodoFatFim,
                    IDCliente = idClientes
                }))
                {
                    // registra diaria
                    var fatDiaria = multi.Read<ItemFaturamentoResumidoEntidade>().AsList();
                    fatDiaria = fatDiaria.Select(x =>
                    {
                        x.Periodo = $"{periodoFatInico.ToShortDateString()} a {periodoFatFim.ToShortDateString()}";
                        x.TipoDescContrato = "Diárias Avulsas";
                        return x;
                    }).ToList();

                    // corridas
                    var fatContrato = multi.Read<ItemFaturamentoResumidoEntidade>().AsList();
                    fatContrato = fatContrato.Select(x =>
                    {
                        x.Periodo = $"{periodoFatInico.ToShortDateString()} a {periodoFatFim.ToShortDateString()}";
                        x.TipoDescContrato = "Chamados Avulsos";
                        return x;
                    }).ToList();

                    // Serpara por tipo
                    if (tipo != null)
                    {
                        if (tipo == TipoContrato.ChamadosAvulsos)
                            retorno = fatDiaria.ToArray();
                        else
                            retorno = fatContrato.ToArray();
                    }
                    else
                        retorno = fatDiaria.Union(fatContrato).ToArray();

                    // Agrupa por cliente
                    retorno = retorno.GroupBy(fat => fat.IDCliente).Select(g => new ItemFaturamentoResumidoEntidade()
                    {
                        //ID = g.Key,
                         
                        Valor = g.Sum(s => s.Valor),
                        ID = g.First().ID,
                        IDCliente = g.First().IDCliente,
                        NomeCliente = g.First().NomeCliente,
                        Periodo = g.First().Periodo,
                        TipoDescContrato = tipo == null ? "Todos" : g.First().TipoDescContrato
                    }).ToArray();

                }
                // Execução
                return retorno;
            }
        }


        public IEnumerable<ItemFaturamentoDetalheEntidade> BuscaDetalheItemFaturadoDiaria(long idCliente, DateTime periodoFatInico, DateTime periodoFatFim)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query

                string queryDiarias = @"-- busca todas diarias		
						  select
                                it.idRegistroDiaria
   	                            from tblItemFaturamento IT
						join tblPeriodoFaturamento PF on (IT.idPeriodoFaturamento = pf.idPeriodoFaturamento)
						join tblRegistroDiarias RG on (RG.idRegistroDiaria = it.idRegistroDiaria)
						join tblClientes CLI on (RG.idCliente = CLI.idCliente)
				where PF.dtDataInicioPeriodoFaturamento = @dtIni 
						 and PF.dtDataFimPeriodoFaturamento = @dtFim  
						 and it.idCorrida is null
						 and IT.idRegistroDiaria is not null 
						 and RG.idCliente = @idcli";


                // Retorno
                var diarias = sqlConnection.Query<long>(queryDiarias, new
                {
                    idcli = idCliente,
                    dtIni = periodoFatInico,
                    dtFim = periodoFatFim
                });

                if (diarias == null || !diarias.Any())
                    return new ItemFaturamentoDetalheEntidade[] { };

                string queryDetalheDiarias = @"	select
                                                        CLI.vcNomeFantasia as NomeCliente,
                                                        	'Diária Avulsa' as Tipo,
	                                                        RD.dtDataHoraInicioExpediente as Data,
	                                                        RD.idRegistroDiaria as OS,
	                                                        CES.vcNomeCompleto as Profissional,
	                                                        (intOdometroFimExpediente - intOdometroInicioExpediente) as KM,
	                                                        decValorDiariaNegociado as Valor
                                                         from tblRegistroDiarias  as RD
                                                            join tblColaboradoresEmpresaSistema CES on(CES.idColaboradorEmpresaSistema = rd.idColaboradorEmpresaSistema)
                                                            join tblClientes CLI on(CLI.idCliente = rd.idCliente)
                                                         where idRegistroDiaria in (@ids)";

                var diariasDetalhe = sqlConnection.Query<ItemFaturamentoDetalheEntidade>(queryDetalheDiarias, new
                {
                    ids = string.Join(",", diarias)
                });

                // Execução
                return diariasDetalhe;
            }
        }

        public IEnumerable<ItemFaturamentoDetalheEntidade> BuscaDetalheItemFaturadoCorrida(long idCliente, DateTime periodoFatInico, DateTime periodoFatFim)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query

                string queryCorridas = @"-- busca todas diarias		
						    select
                                it.idCorrida
   	                            from tblItemFaturamento IT
						join tblPeriodoFaturamento PF on (IT.idPeriodoFaturamento = pf.idPeriodoFaturamento)
						join tblCorridas CR on (CR.idCorrida = it.idCorrida)
						join tblClientes CLI on (CR.idCliente = CLI.idCliente)
				where PF.dtDataInicioPeriodoFaturamento = @dtIni 
						 and PF.dtDataFimPeriodoFaturamento = @dtFim  
						 and it.idRegistroDiaria is null
						 and IT.idCorrida is not null
						 and CR.idCliente = @idCliente";


                // Retorno
                var corridas = sqlConnection.Query<long>(queryCorridas, new
                {
                    idCliente = idCliente,
                    dtIni = periodoFatInico,
                    dtFim = periodoFatFim
                });

                if (corridas == null || !corridas.Any())
                    return new ItemFaturamentoDetalheEntidade[] { };

                string queryDetalheDiarias = @"select
                                                CLI.vcNomeFantasia as NomeCliente,
                                                	    	'Chamado Avulso' as Tipo,
                                                            dtDataHoraInicio as Data,
                                                            idCorrida as OS,
                                                            CES.vcNomeCompleto as Profissional,
                                                            decValorFinalizado as Valor
                                                      from tblCorridas as CR
                                                            join tblColaboradoresEmpresaSistema CES on (CES.idColaboradorEmpresaSistema = CR.idUsuarioColaboradorEmpresa)
                                                            join tblClientes as CLI on (CLI.idCliente = CR.idCliente) 
                                                         where idCorrida in @ids";

                var diariasDetalhe = sqlConnection.Query<ItemFaturamentoDetalheEntidade>(queryDetalheDiarias, new
                {
                    ids = corridas
                });

                // Execução
                return diariasDetalhe;
            }
        }
    }
}

//-- Detalhe Registro Diaria
//select
//    CLI.vcNomeFantasia as NomeCliente,
//	'Diária Avulsa' as Tipo,
//	RD.dtDataHoraInicioExpediente as Data,
//	RD.idRegistroDiaria as OS,
//	CES.vcNomeCompleto as Profissional,
//	(intOdometroFimExpediente - intOdometroInicioExpediente) as KM,
//	decValorDiariaNegociado as Valor
// from tblRegistroDiarias  as RD
//    join tblColaboradoresEmpresaSistema CES on(CES.idColaboradorEmpresaSistema = rd.idColaboradorEmpresaSistema)

//    join tblClientes CLI on(CLI.idCliente = rd.idCliente)
//        where idRegistroDiaria = 4

//-- Detalhe Corrida

//        select

//            CLI.vcNomeFantasia as NomeCliente,
//	    	'Chamado Avulso' as Tipo,
//            dtDataHoraInicio as Data,
//            idCorrida as OS,
//            CES.vcNomeCompleto as Profissional,
//            decValorFinalizado as Valor

//            from tblCorridas as CR

//            join tblColaboradoresEmpresaSistema CES on (CES.idColaboradorEmpresaSistema = CR.idUsuarioColaboradorEmpresa)

//             LEFT join tblClientes as CLI on (CLI.idUsuario = CR.idUsuarioChamador) 

//             LEFT join tblColaboradoresCliente as CC on (CC.idUsuario = CR.idUsuarioChamador) 
//				where idCorrida = 3

//-- Busca KMS percorridos corrida

//                SELECT

//                 geoPosicao.STY  as vcLatitude,
//                 geoPosicao.STX  as vcLongitude

//                 FROM tblLogCorrida
//                    where idCorrida = 3

//                        and idStatusCorrida in (2, 3, 8, 9, 10)