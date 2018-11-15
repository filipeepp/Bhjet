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
