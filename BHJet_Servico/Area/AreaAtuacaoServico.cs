using BHJet_DTO.Area;
using System;
using System.Collections.Generic;

namespace BHJet_Servico.Area
{
    public interface IAreaAtuacaoServico
    {
        AreaAtuacaoDTO[] BuscaAreaAtuacao();
        void AtualizaAreaAtuacao(IEnumerable<AreasFiltroDTO> filtro);
    }

    public class AreaAtuacaoServico : ServicoBase, IAreaAtuacaoServico
    {
        public AreaAtuacaoServico(string token) : base(token)
        {

        }

        /// <summary>
        /// Busca Detalhe Corrida
        /// </summary>
        /// <returns>ResumoModel</returns>
        public AreaAtuacaoDTO[] BuscaAreaAtuacao()
        {
            return this.Get<AreaAtuacaoDTO[]>(new Uri($"{ServicoRotas.Base}{ServicoRotas.AreaAtuacao.GetAreaAtuacao}"));
        }

        /// <summary>
        /// Atualiza Detalhe Corrida
        /// </summary>
        /// <returns>ResumoModel</returns>
        public void AtualizaAreaAtuacao(IEnumerable<AreasFiltroDTO> filtro)
        {
            this.Post(new Uri($"{ServicoRotas.Base}{ServicoRotas.AreaAtuacao.GetAreaAtuacao}"), filtro);
        }
    }
}
