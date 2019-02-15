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
							                    join tblLogCorrida LGCD on (CD.idCorrida = LGCD.idCorrida)
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
                string query = @"select LGCD.idStatusCorrida as Status, 
					count(CD.idCorrida) as Quantidade,
					CD.dtDataHoraRegistroCorrida as DataRegistro
							 from tblCorridas CD
								join tblLogCorrida LGCD on (CD.idCorrida = LGCD.idCorrida)
									where LGCD.idStatusCorrida in (11, 7, 8, 9, 10)
										group by CD.dtDataHoraRegistroCorrida,
												 LGCD.idStatusCorrida";

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
                string query = @"select ES.idTipoProfissional as TipoProfissional,
                					   count(CD.idUsuarioColaboradorEmpresa) as Quantidade,
					                   CD.dtDataHoraRegistroCorrida as DataRegistro
							    from tblCorridas CD
								join tblLogCorrida LGCD on (CD.idCorrida = LGCD.idCorrida)
								join tblColaboradoresEmpresaSistema ES on (cd.idUsuarioColaboradorEmpresa = ES.idColaboradorEmpresaSistema)
									where LGCD.idStatusCorrida = 10
										group by ES.idTipoProfissional,  CD.dtDataHoraRegistroCorrida";

                return sqlConnection.Query<ResumoAtendimentoEntidade>(query);
            }
        }
    }
}
