using BHJet_Core.Enum;
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
                                     %clienteCondition1%

		                        --- Busca Corridas ---
		                                select P.idCorrida,
                                               P.idUsuarioChamador as idUsuarioFaturado,
	                                           P.idTarifario,
	                                           P.idUsuarioColaboradorEmpresa,
	                                           P.decValorFinalizado as decValor,
	                                           P.decValorComissaoNegociado as decValorComissao
	                                        from tblCorridas as P
                                                 join tblUsuarios as US on (P.idUsuarioChamador = us.idUsuario)
			                                     join tblColaboradoresCliente as CC on (CC.idUsuario = us.idUsuario) 
	                                        where P.dtDataHoraInicio BETWEEN  @dataInicio AND @dataFim
                                        	  and P.dtDataHoraTermino is not null 
                                              and P.idCorrida not  in (select idCorrida from tblItemFaturamento where idCorrida = P.idCorrida)
                                              %clienteCondition2%";

                        // Busca por cliente
                        if (idCliente != null && idCliente.Any())
                        {
                            query = query.Replace("%clienteCondition1%", " and  P.idCliente in @IDCliente");
                            query = query.Replace("%clienteCondition2%", " and  CC.idCliente in @IDCliente");
                        }

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
                            var servicosUnificados = new List<ItemFaturamentoEntidade>();

                            // Atualiza id periodo faturamento nas itens a faturar
                            if (idCliente != null && idCliente.Any())
                                servicosUnificados = diarias.Union(corridas).ToList();

                            // Lista final a ser faturado
                            servicosUnificados = diarias.Select(x =>
                            {
                                x.idPeriodoFaturamento = idPeriodoFaturamento;
                                x.bitFaturado = false;
                                return x;
                            }).ToList();

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
                                    PidPeriodoFaturamento = nvItemFat.idPeriodoFaturamento,
                                    PdecValor = nvItemFat.decValor,
                                    PdecValorComissao = nvItemFat.decValorComissao,
                                    PbitFaturado = nvItemFat.bitFaturado
                                }, trans);
                            }
                        }

                        // Commit
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
 	                            IT.idItemFaturamento AS ID, 
 	                            CLI.vcNomeFantasia AS NomeCliente,
 	                            concat(convert(varchar(11), PF.dtDataInicioPeriodoFaturamento, 103), ' até ', convert(varchar(11),PF.dtDataFimPeriodoFaturamento,103)) as Periodo,
 	                            'Chamados Avulsos' as TipoDescContrato,
 	                            IT.decValor as Valor
   	                            from tblItemFaturamento IT
						join tblPeriodoFaturamento PF on (IT.idPeriodoFaturamento = pf.idPeriodoFaturamento)
						left join tblRegistroDiarias RG on (RG.idRegistroDiaria = it.idRegistroDiaria)
						left join tblClientes CLI on (RG.idCliente = CLI.idCliente)
				where PF.dtDataInicioPeriodoFaturamento = @dataInicio  
						 and PF.dtDataFimPeriodoFaturamento = @dataFim  
						 and it.idCorrida is null
						 and IT.idRegistroDiaria is not null  %clienteCondition%

                        -- fatu corridas
	                            select
 	                            IT.idItemFaturamento AS ID, 
 	                            CLI.vcNomeFantasia AS NomeCliente,
 	                            concat(convert(varchar(11), PF.dtDataInicioPeriodoFaturamento, 103), ' até ', convert(varchar(11),PF.dtDataFimPeriodoFaturamento,103)) as Periodo,,
 	                            'Chamados Avulsos' as TipoDescContrato,
 	                            IT.decValor as Valor
  	                             from tblItemFaturamento IT
						join tblPeriodoFaturamento PF on (IT.idPeriodoFaturamento = pf.idPeriodoFaturamento)
						left join tblCorridas RG on (RG.idCorrida = it.idCorrida)
						 join tblColaboradoresCliente as CC on (CC.idUsuario = RG.idUsuarioChamador)             
		                 join tblClientes CLI on (CLI.idCliente = CC.idCliente)
				where PF.dtDataInicioPeriodoFaturamento = @dataInicio 
						 and PF.dtDataFimPeriodoFaturamento = @dataFim 
						 and it.idRegistroDiaria is null
						 and IT.idCorrida is not null  %clienteCondition%";

                if (idClientes != null && idClientes.Any())
                    query = query.Replace("%clienteCondition%", " and  CLI.idCliente in @IDCliente");
                else
                    query = query.Replace("%clienteCondition%", " ");

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
                    // chamadosAguardando
                    var fatDiaria = multi.Read<ItemFaturamentoResumidoEntidade>().AsList();

                    // motoristasAguardando
                    var fatContrato = multi.Read<ItemFaturamentoResumidoEntidade>().AsList();

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
                }
                // Execução
                return retorno;
            }
        }

    }
}
