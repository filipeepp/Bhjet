using BHJet_DTO.Profissional;
using System;
using System.Collections.Generic;

namespace BHJet_Servico.Profissional
{
    public interface IProfissionalServico
    {
        IEnumerable<ProfissionalModel> BuscaProfissionais(string trechoPesquisa);
        ProfissionalCompletoModel BuscaProfissional(long id);
        void AtualizaDadosProfissional(ProfissionalCompletoModel proModel);
        void IncluirProfissional(ProfissionalCompletoModel proModel);
        ComissaoModel BuscaComissaoProfissional(long id);
    }

    public class ProfissionalServico : ServicoBase, IProfissionalServico
    {
        public ProfissionalServico(string token) : base(token)
        {

        }

        /// <summary>
        /// Busca Lista de profissionais
        /// </summary>
        /// <returns>ResumoModel</returns>
        public IEnumerable<ProfissionalModel> BuscaProfissionais(string trechoPesquisa)
        {
            return this.Get<IEnumerable<ProfissionalModel>>(new Uri($"{ServicoRotas.Base}{ServicoRotas.Profissional.GetProfissionais}?trecho={trechoPesquisa}"));
        }

        /// <summary>
        /// Busca Detalhe de Profissional especifico
        /// </summary>
        /// <returns>ResumoModel</returns>
        public ProfissionalCompletoModel BuscaProfissional(long id)
        {
            return this.Get<ProfissionalCompletoModel>(new Uri($"{ServicoRotas.Base}{string.Format(ServicoRotas.Profissional.GetProfissional, id)}"));
        }

        /// <summary>
        /// Atualiza dados de um Profissional especifico
        /// </summary>
        /// <returns>ResumoModel</returns>
        public void AtualizaDadosProfissional(ProfissionalCompletoModel proModel)
        {
            //return;
            this.Put(new Uri($"{ServicoRotas.Base}{string.Format(ServicoRotas.Profissional.PutProfissional, proModel.ID)}"), proModel);
        }

        /// <summary>
        /// Incluir um Profissional
        /// </summary>
        /// <returns>ResumoModel</returns>
        public void IncluirProfissional(ProfissionalCompletoModel proModel)
        {
            this.Post(new Uri($"{ServicoRotas.Base}{ServicoRotas.Profissional.PostProfissional}"), proModel);
        }

        /// <summary>
        /// Busca Detalhe de Profissional especifico
        /// </summary>
        /// <returns>ResumoModel</returns>
        public ComissaoModel BuscaComissaoProfissional(long id)
        {
            return this.Get<ComissaoModel>(new Uri($"{ServicoRotas.Base}{string.Format(ServicoRotas.Profissional.GetComissaoProfissional, id)}"));
        }
    }
}
