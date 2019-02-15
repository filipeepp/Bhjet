using BHJet_Enumeradores;
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
        public ResumoServico(string token) : base(token)
        {

        }

        /// <summary>
        /// Busca Resumo Dashboard
        /// </summary>
        /// <returns>ResumoModel</returns>
        public ResumoModel BuscaResumo()
        {
            return this.Get<ResumoModel>(new Uri($"{ServicoRotas.Base}{ServicoRotas.Dashboard.GetResumo}"));
        }


        /// <summary>
        /// Busca Resumo Chamados por situacao no ultimo ano
        /// </summary>
        /// <returns>ResumoModel</returns>
        public IEnumerable<ResumoChamadoModel> BuscaResumoChamadosSituacao()
        {
            try
            {
                return this.Get<IEnumerable<ResumoChamadoModel>>(new Uri($"{ServicoRotas.Base}{ServicoRotas.Dashboard.GetResumoChamadosSit}"));
            }
            catch
            {
                return new ResumoChamadoModel[] { };
            }
        }

        /// <summary>
        /// Busca Resumo Dashboard
        /// </summary>
        /// <returns>ResumoModel</returns>
        public IEnumerable<ResumoAtendimentoModel> BuscaResumoAtendimentosSituacao()
        {
            try
            {
                return this.Get<IEnumerable<ResumoAtendimentoModel>>(new Uri($"{ServicoRotas.Base}{ServicoRotas.Dashboard.GetResumoAtendimentosCategoria}"));
            }
            catch
            {
                return new ResumoAtendimentoModel[] { };
            }
        }

        /// <summary>
        /// Busca Localizacao Profissionais
        /// </summary>
        /// <returns>ResumoModel</returns>
        public IEnumerable<LocalizacaoProfissionalModel> BuscaLocalizacaoProfissionais(TipoProfissional tipo)
        {
            return this.Get<LocalizacaoProfissionalModel[]>(new Uri($"{ServicoRotas.Base}{string.Format(ServicoRotas.Profissional.GetLocalizacoesProfissionais, (int)tipo)}"));
        }

        /// <summary>
        /// Busca Localizacao Profissionais
        /// </summary>
        /// <returns>ResumoModel</returns>
        public LocalizacaoProfissionalModel BuscaLocalizacaoProfissional(int idProfissional)
        {
            return this.Get<LocalizacaoProfissionalModel>(new Uri($"{ServicoRotas.Base}{string.Format(ServicoRotas.Profissional.GetLocalizacaoProfissional, idProfissional)}"));
        }

        /// <summary>
        /// Busca Localizacao Corridas
        /// </summary>
        /// <returns>ResumoModel</returns>
        public IEnumerable<LocalizacaoCorridaModel> BuscaLocalizacaoCorridas(StatusCorrida status, TipoProfissional tipo)
        {
            return this.Get<LocalizacaoCorridaModel[]>(new Uri($"{ServicoRotas.Base}" +
                $"{string.Format(ServicoRotas.Corrida.GetLocalizacaoCorridas, (int)status, (int)tipo)}"));
        }

    }
}
