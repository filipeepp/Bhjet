using BHJet_Core.Enum;
using BHJet_DTO.Corrida;
using BHJet_DTO.Dashboard;
using BHJet_DTO.Profissional;
using System;
using System.Collections.Generic;

namespace BHJet_Servico.Dashboard
{
    public interface IResumoServico
    {
        ResumoModel BuscaResumo();
        IEnumerable<LocalizacaoProfissionalModel> BuscaLocalizacaoProfissionais(TipoProfissional tipo);
        IEnumerable<LocalizacaoCorridaModel> BuscaLocalizacaoCorridas(StatusCorrida status, TipoProfissional tipo);
        LocalizacaoProfissionalModel BuscaLocalizacaoProfissional(int idProfissional);
        IEnumerable<ResumoChamadoModel> BuscaResumoChamadosSituacao();
        IEnumerable<ResumoAtendimentoModel> BuscaResumoAtendimentosSituacao();
    }

    public class ResumoServico : ServicoBase, IResumoServico
    {
        /// <summary>
        /// Busca Resumo Dashboard
        /// </summary>
        /// <returns>ResumoModel</returns>
        public ResumoModel BuscaResumo()
        {
            return new ResumoModel()
            {
                MotociclistaDisponiveis = 10,
                ChamadosAguardandoMotorista = 23,
                ChamadosAguardandoMotociclista = 5,
                MotoristasDisponiveis = 3
            };

            return this.Get<ResumoModel>(new Uri($"{ServicoRotas.Base}{ServicoRotas.Dashboard.GetResumo}"));
        }


        /// <summary>
        /// Busca Resumo Chamados por situacao no ultimo ano
        /// </summary>
        /// <returns>ResumoModel</returns>
        public IEnumerable<ResumoChamadoModel> BuscaResumoChamadosSituacao()
        {
            // Separa chamados 
            List<ResumoChamadoModel> ResumoMensal = new List<ResumoChamadoModel>();

            // Percorre os meses
            for (int i = 1; i <= 12; i++)
            {
                ResumoMensal.Add(new ResumoChamadoModel()
                {
                    Mes = i,
                    ChamadosAdvertentes = i + 1,
                    ChamadosConcluidos = i + 2
                });
            }

            return ResumoMensal;

            return this.Get<IEnumerable<ResumoChamadoModel>>(new Uri($"{ServicoRotas.Base}{ServicoRotas.Dashboard.GetResumoChamadosSit}"));
        }

        /// <summary>
        /// Busca Resumo Dashboard
        /// </summary>
        /// <returns>ResumoModel</returns>
        public IEnumerable<ResumoAtendimentoModel> BuscaResumoAtendimentosSituacao()
        {

            // Separa chamados 
            List<ResumoAtendimentoModel> ResumoMensal = new List<ResumoAtendimentoModel>();

            // Percorre os meses
            for (int i = 1; i <= 12; i++)
            {
                ResumoMensal.Add(new ResumoAtendimentoModel()
                {
                    Mes = i,
                    QtdAtendimentoMotociclista = i + 2,
                    QtdAtendimentoMotorista = i + 1,
                });
            }

            return ResumoMensal;

            return this.Get<IEnumerable<ResumoAtendimentoModel>>(new Uri($"{ServicoRotas.Base}{ServicoRotas.Dashboard.GetResumoAtendimentosCategoria}"));
        }

        /// <summary>
        /// Busca Localizacao Profissionais
        /// </summary>
        /// <returns>ResumoModel</returns>
        public IEnumerable<LocalizacaoProfissionalModel> BuscaLocalizacaoProfissionais(TipoProfissional tipo)
        {
            return new LocalizacaoProfissionalModel[]
            {
                new LocalizacaoProfissionalModel()
                {
                     idColaboradorEmpresaSistema = 1,
                      geoPosicao = "-19.8157;-43.9542",
                }
            };

            return this.Get<LocalizacaoProfissionalModel[]>(new Uri($"{ServicoRotas.Base}{string.Format(ServicoRotas.Profissional.GetLocalizacoesProfissionais, (int)tipo)}"));
        }

        /// <summary>
        /// Busca Localizacao Profissionais
        /// </summary>
        /// <returns>ResumoModel</returns>
        public LocalizacaoProfissionalModel BuscaLocalizacaoProfissional(int idProfissional)
        {
            return new LocalizacaoProfissionalModel()
            {
                idColaboradorEmpresaSistema = 1,
                geoPosicao = "-19.8157;-43.9542",
            };

            return this.Get<LocalizacaoProfissionalModel>(new Uri($"{ServicoRotas.Base}{string.Format(ServicoRotas.Profissional.GetLocalizacaoProfissional, idProfissional)}"));
        }

        /// <summary>
        /// Busca Localizacao Corridas
        /// </summary>
        /// <returns>ResumoModel</returns>
        public IEnumerable<LocalizacaoCorridaModel> BuscaLocalizacaoCorridas(StatusCorrida status, TipoProfissional tipo)
        {
            return new LocalizacaoCorridaModel[]
            {
                new LocalizacaoCorridaModel()
                {
                   idCorrida = 1,
                   geoPosicao = "-19.914801;-43.984832"
                },
                 new LocalizacaoCorridaModel()
                {
                   idCorrida = 1,
                   geoPosicao = "-19.931725;-43.986317"
                }
            };

            return this.Get<LocalizacaoCorridaModel[]>(new Uri($"{ServicoRotas.Base}" +
                $"{string.Format(ServicoRotas.Corrida.GetLocalizacaoCorridas, (int)status, (int)tipo)}"));
        }

    }
}
