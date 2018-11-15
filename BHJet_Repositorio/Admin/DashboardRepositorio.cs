﻿using BHJet_Core.Enum;
using BHJet_Repositorio.Admin.Entidade;
using Dapper;
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
		                               select CLB.idTipoProfissional, COUNT(*) as Quantidade from tblCorridas CD
							                    join tblLogCorrida LGCD on (CD.idCorrida = LGCD.idCorrida)
							                    join tblColaboradoresEmpresaSistema as CLB on (CD.idUsuarioColaboradorEmpresa = CLB.idColaboradorEmpresaSistema)
								            where LGCD.idStatusCorrida = 4
								                group by CLB.idTipoProfissional

		                        --- Quantidade de motorista e motociclista disponiveis
		                               select idTipoProfissional as TipoProfissional, count(*) as Quantidade from tblColaboradoresEmpresaDisponiveis
			                                where bitDisponivel = 0
				                      group by idTipoProfissional";

                // Query Multiple
                using (var multi = sqlConnection.QueryMultiple(query, new { InvoiceID = 1 }))
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
    }
}