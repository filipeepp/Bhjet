using BHJet_DTO.Profissional;
using BHJet_Enumeradores;
using System;
using System.Collections.Generic;

namespace BHJet_Servico.Profissional
{
    public interface IProfissionalServico
    {
        IEnumerable<ProfissionalModel> BuscaProfissionais(string trechoPesquisa, int? tipoVeiculo);
        IEnumerable<ProfissionalModel> BuscaProfissionaisDisponiveis(string trechoPesquisa, TipoProfissional? tipoProfissional);
        ProfissionalCompletoModel BuscaProfissional(long id);
        void AtualizaDadosProfissional(ProfissionalCompletoModel proModel);
        void IncluirProfissional(ProfissionalCompletoModel proModel);
        ComissaoModel BuscaComissaoProfissional(long id);
        TipoVeiculoDTO[] BuscaTipoVeiculos();
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
        public IEnumerable<ProfissionalModel> BuscaProfissionais(string trechoPesquisa, int? tipoVeiculo)
        {
            string parms1 = string.IsNullOrWhiteSpace(trechoPesquisa) ? "" : "trecho=" + trechoPesquisa;
            string parms2 = tipoVeiculo == null ? "" : "tipoVeiculo=" + tipoVeiculo;
            string parametroCompleto = "";

            if (parms1 != "")
            {
                parametroCompleto = parms1;
                if (parms2 != "")
                    parametroCompleto += parms2;
            }
            else if (parms2 != "")
                parametroCompleto = parms2;

            return this.Get<IEnumerable<ProfissionalModel>>(new Uri($"{ServicoRotas.Base}" +
                $"{ServicoRotas.Profissional.GetProfissionais}?{parametroCompleto}"));
        }

        /// <summary>
        /// Busca Lista de profissionais Disponiveis
        /// </summary>
        /// <returns>ResumoModel</returns>
        public IEnumerable<ProfissionalModel> BuscaProfissionaisDisponiveis(string trechoPesquisa, TipoProfissional? tipoProfissional)
        {
            var tipoParam = tipoProfissional != null ? "tipoProfissional=" + (int)tipoProfissional : "";

            return this.Get<IEnumerable<ProfissionalModel>>(new Uri($"{ServicoRotas.Base}{ServicoRotas.Profissional.GetProfissionaisDisponiveis}" +
                                            $"?trecho={trechoPesquisa}&{tipoParam}"));
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

        public TipoVeiculoDTO[] BuscaTipoVeiculos()
        {
            return this.Get<TipoVeiculoDTO[]>(new Uri($"{ServicoRotas.Base}{ServicoRotas.Profissional.GetTipoVeiculos}"));
            //return new TipoVeiculoDTO[]
            //{
            //    new TipoVeiculoDTO()
            //    {
            //         ID = 1,
            //         Descricao = "Carro"
            //    }
            //};
        }
    }
}
