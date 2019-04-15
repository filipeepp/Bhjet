using BHJet_Repositorio.Admin.Entidade;
using BHJet_Repositorio.Admin.Filtro;
using Dapper;
using System.Collections.Generic;

namespace BHJet_Repositorio.Admin
{
    public class TarifaRepositorio : RepositorioBase
    {
        /// <summary>
        /// Busca tarifas de um cliente especifico
        /// </summary>
        /// <param name="filtro">TipoProfissional</param>
        /// <returns>UsuarioEntidade</returns>
        public TarifaEntidade BuscaTarificaPorCliente(long? clienteID, int? tipoVeiculo)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"DECLARE @ExisteCliente int;  
                                 DECLARE @IDCliente int;  
                                    set @IDCliente = @id;
                                    set @ExisteCliente = (select CAST(COUNT(1) AS BIT) from tblClienteTarifario where idCliente = @IDCliente )
                                IF @ExisteCliente = 1  
                                    select  top(1)  idCliente as ID,
												   vcDescricaoTarifario as Descricao,
												   dtDataInicioVigencia as DataInicioVigencia,
												   decValorContrato as ValorContrato,
												   intFranquiaKM as FranquiaKM,
												   decValorKMAdicional as ValorKMAdicional,
                                                   intFranquiaHoras,
                                                   decValorHoraAdicional,
												   bitAtivo as Ativo,
                                                   decValorPontoExcedente , decValorPontoColeta, decValorMinutoParado , 
												  vcObservacao as Observacao from tblClienteTarifario where idCliente = @IDCliente  and bitAtivo = 1 &tpVei& order by dtDataInicioVigencia desc
                                ELSE   
                                    select top(1) idTarifario as ID,
												   vcDescricaoTarifario as Descricao,
												   dtDataInicioVigencia as DataInicioVigencia,
												   decValorContrato as ValorContrato,
												   intFranquiaMinutosParados as MinutosParados,
												   decValorMinutoParado as ValorMinutosParados,
												   intFranquiaKM as FranquiaKM,  
                                                   intFranquiaHoras,
                                                   decValorHoraAdicional,
												   decValorKMAdicional as ValorKMAdicional,
                                                   decValorPontoExcedente , decValorPontoColeta, decValorMinutoParado , 
												   bitAtivo as Ativo from tblTarifario where bitAtivo = 1 and idTipoServico = 2 &tpVei& order by dtDataInicioVigencia desc";

                query = tipoVeiculo != null ? query.Replace("&tpVei&", "and idTipoVeiculo = @tp") : query.Replace("&tpVei&", "");

                // Execução
                return sqlConnection.QueryFirstOrDefault<TarifaEntidade>(query, new
                {
                    id = clienteID,
                    tp = tipoVeiculo
                });
            }
        }

        public TarifaEntidade BuscaTarificaCorrida(int tipoVeiculo)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @" select top(1) idTarifario as ID,
												   vcDescricaoTarifario as Descricao,
												   dtDataInicioVigencia as DataInicioVigencia,
												   decValorContrato as ValorContrato,
												   intFranquiaMinutosParados as MinutosParados,
												   decValorMinutoParado as ValorMinutosParados,
												   intFranquiaKM as FranquiaKM,  
                                                   intFranquiaHoras,
                                                   decValorHoraAdicional,
												   decValorKMAdicional as ValorKMAdicional,
                                                   decValorPontoExcedente , decValorPontoColeta, decValorMinutoParado , 
												   bitAtivo as Ativo from tblTarifario where idTipoVeiculo = @tp  and idTipoServico = 1 order by dtDataInicioVigencia desc";

                // Execução
                return sqlConnection.QueryFirstOrDefault<TarifaEntidade>(query, new
                {
                    tp = tipoVeiculo
                });
            }
        }

        /// <summary>
        /// Busca tarifa ativa na tabela de preço
        /// </summary>
        /// <param name="filtro">TipoProfissional</param>
        /// <returns>TarifaDTO</returns>
        public TarifaEntidade BuscaTarfaPadraoAtiva(int codigoTipoVeiculo)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"SELECT
										idTarifario as ID,
										vcObservacao as Observacao,
										decValorContrato as  ValorContrato,
										decFranquiaKM as FranquiaKM,
										decValorKMAdicional as ValorKMAdicional,
                                        bitAtivo as Ativo
									FROM
										tblTarifario
									WHERE
										idTipoVeiculo = @TipoVeiculo
									AND
										bitAtivo = 1";

                // Execução
                return sqlConnection.QueryFirstOrDefault<TarifaEntidade>(query, new { TipoVeiculo = codigoTipoVeiculo });
            }
        }

        /// <summary>
        /// Busca tarifa ativa na tabela de preço
        /// </summary>
        /// <param name="filtro">TipoProfissional</param>
        /// <returns>TarifaDTO</returns>
        public IEnumerable<TarifarioEntidade> BuscaTarifaPadrao()
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"select * from tblTarifario";

                // Execução
                return sqlConnection.Query<TarifarioEntidade>(query);
            }
        }

        /// <summary>
        /// Busca tarifa ativa na tabela de preço
        /// </summary>
        /// <param name="filtro">TipoProfissional</param>
        /// <returns>TarifaDTO</returns>
        public void AtualizaTarifaPadrao(TarifarioEntidade[] filtro)
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                using (var trans = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        // Atualiza tarifas
                        foreach (var tarifa in filtro)
                        {
                            string queryAtualiza = @"update tblTarifario set  
                                                        intFranquiaMinutosParados = @minParado,
                                                        decValorMinutoParado = @valMinuto,
                                                        decValorContrato = @valorContrato,
                                                        intFranquiaKM = @franquiaKM,
                                                        decValorKMAdicional = @valorKM,
                                                        intFranquiaHoras = @franquiaHoras,
                                                        decValorHoraAdicional = @valorHora,
                                                        decValorPontoExcedente = @ValoPonto,
                                                        decValorPontoColeta = @ValorColeta, 
                                                        vcObservacao = @obs where idTarifario = @id";

                            trans.Connection.Execute(queryAtualiza, new
                            {
                                id = tarifa.idTarifario,
                                minParado = tarifa.intFranquiaMinutosParados,
                                valMinuto = tarifa.decValorMinutoParado,
                                valorContrato = tarifa.decValorContrato,
                                franquiaKM = tarifa.intFranquiaKM,
                                valorKM = tarifa.decValorKMAdicional,
                                franquiaHoras = tarifa.intFranquiaHoras,
                                valorHora = tarifa.decValorHoraAdicional,
                                ValoPonto = tarifa.decValorPontoExcedente,
                                ValorColeta = tarifa.decValorPontoColeta,
                                obs = tarifa.vcObservacao
                            }, trans);
                        }

                        // Comit
                        trans.Commit();
                    }
                    catch (System.Exception e)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

    }

}

