using BHJet_Enumeradores;
using BHJet_Repositorio.Admin.Entidade;
using Dapper;
using System.Collections.Generic;
using System.Linq;

namespace BHJet_Repositorio.Admin
{
    public class DashboardRepositorio : RepositorioBase
    {
        /// <summary>
        /// Busca Resumo de chamados e motoristas disponiveis
        /// </summary>
        /// <param name="filtro">ValidaUsuarioFiltro</param>
        /// <returns>UsuarioEntidade</returns>
        public ResumoDetalheEntidade BuscaResumoDashboard()
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"--- Quantidade de chamados abertos esperando profissional 
		                               select CD.idTipoProfissional AS TipoProfissional, COUNT(*) as Quantidade from tblCorridas CD
							                    --join tblLogCorrida LGCD on (CD.idCorrida = LGCD.idCorrida)
								            where CD.idStatusCorrida = 3
								                group by CD.idTipoProfissional

		                        --- Quantidade de motorista e motociclista disponiveis
		                               select idTipoProfissional as TipoProfissional, count(*) as Quantidade from tblColaboradoresEmpresaDisponiveis
			                                where bitDisponivel = 1
				                      group by idTipoProfissional";

                // Query Multiple
                using (var multi = sqlConnection.QueryMultiple(query))
                {
                    // chamadosAguardando
                    var chamadosAguardando = multi.Read<ResumoEntidade>().AsList();

                    // motoristasAguardando
                    var motoristasAguardando = multi.Read<ResumoEntidade>().AsList();

                    // Return
                    return new ResumoDetalheEntidade()
                    {
                        ChamadosAguardandoMotociclista = chamadosAguardando?.AsList().Count(ch => ch.TipoProfissional == TipoProfissional.Motociclista) ?? 0,
                        ChamadosAguardandoMotorista = chamadosAguardando?.AsList().Count(ch => ch.TipoProfissional == TipoProfissional.Motorista) ?? 0,
                        MotociclistaDisponiveis = motoristasAguardando?.AsList().Count(ch => ch.TipoProfissional == TipoProfissional.Motociclista) ?? 0,
                        MotoristasDisponiveis = motoristasAguardando?.AsList().Count(ch => ch.TipoProfissional == TipoProfissional.Motorista) ?? 0
                    };
                }
            }
        }

        /// <summary>
        /// Busca Resumo de chamados concluidos e advertentes
        /// </summary>
        /// <param name="filtro">ValidaUsuarioFiltro</param>
        /// <returns>UsuarioEntidade</returns>
        public IEnumerable<ResumoChamados> BuscaResumoChamados()
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"select  idStatusCorrida as Status, 
					count(CD.idCorrida) as Quantidade,
					dtDataHoraRegistroCorrida as DataRegistro
							 from tblCorridas CD
										group by 
												 idStatusCorrida, dtDataHoraRegistroCorrida";

                return sqlConnection.Query<ResumoChamados>(query);
            }
        }

        /// <summary>
        /// Busca quantidade de chamados por cada profissional
        /// </summary>
        /// <param name="filtro">ValidaUsuarioFiltro</param>
        /// <returns>UsuarioEntidade</returns>
        public IEnumerable<ResumoAtendimentoEntidade> BuscaResumoAtendimentosProfissionais()
        {
            using (var sqlConnection = this.InstanciaConexao())
            {
                // Query
                string query = @"select idTipoProfissional as TipoProfissional,
                					  count(idUsuarioColaboradorEmpresa) as Quantidade,
					                  dtDataHoraRegistroCorrida as DataRegistro
							    from tblCorridas
									where idStatusCorrida IN (select idStatusCorrida from tblDOMStatusCorrida WHERE bitFinaliza = 1 OR bitCancela = 1)
										group by idTipoProfissional, dtDataHoraRegistroCorrida	";

                return sqlConnection.Query<ResumoAtendimentoEntidade>(query);
            }
        }
    }
}
