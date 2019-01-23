using BHJet_DTO.Corrida;
using System;

namespace BHJet_Servico.Corrida
{
    public interface ICorridaServico
    {
        DetalheCorridaModel BuscaDetalheCorrida(long idCorrida);
    }

    public class CorridaServico : ServicoBase, ICorridaServico
    {
        public CorridaServico(string token) : base(token)
        {

        }

        /// <summary>
        /// Busca Detalhe Corrida
        /// </summary>
        /// <returns>ResumoModel</returns>
        public DetalheCorridaModel BuscaDetalheCorrida(long idCorrida)
        {
            return this.Get<DetalheCorridaModel>(new Uri($"{ServicoRotas.Base}{string.Format(ServicoRotas.Corrida.GetDetalheCorridas, idCorrida)}"));
        }

    }
}
