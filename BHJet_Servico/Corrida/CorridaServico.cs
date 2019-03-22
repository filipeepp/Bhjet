using BHJet_DTO.Corrida;
using System;

namespace BHJet_Servico.Corrida
{
    public interface ICorridaServico
    {
        DetalheCorridaModel BuscaDetalheCorrida(long idCorrida);
        OcorrenciaDTO[] BuscaOcorrencias();
        decimal CalculoPrecoCorrida();
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

        /// <summary>
        /// Busca Detalhe Corrida
        /// </summary>
        /// <returns>ResumoModel</returns>
        public OcorrenciaDTO[] BuscaOcorrencias()
        {
            return this.Get<OcorrenciaDTO[]>(new Uri($"{ServicoRotas.Base}{ServicoRotas.Corrida.GetOcorrencia}"));
            //return new OcorrenciaSolicitacaoModel[]
            //{
            //    new OcorrenciaSolicitacaoModel()
            //    {
            //         IDSolicitacao = 1,
            //         DescricaoSolicitacao = "Objeto de pequeno porte"
            //    }
            //};
        }

        public decimal CalculoPrecoCorrida()
        {
            return 10;
        }
    }
}
