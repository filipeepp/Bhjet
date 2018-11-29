using BHJet_DTO.Faturamento;
using System;

namespace BHJet_Servico.Faturamento
{
    public interface IFaturamentoServico
    {
        ItemFaturamentoDTO[] GerarFaturamento(GerarFaturamentoDTO model);
        ItemFaturamentoDTO[] GetFaturamentoNormal(ConsultarFaturamentoDTO model);
    }

    public class FaturamentoServico : ServicoBase, IFaturamentoServico
    {
        public FaturamentoServico(string token) : base(token)
        {

        }

        public ItemFaturamentoDTO[] GerarFaturamento(GerarFaturamentoDTO model)
        {
            return this.Post<GerarFaturamentoDTO, ItemFaturamentoDTO[]>(new Uri($"{ServicoRotas.Base}{ServicoRotas.Faturamento.PostFaturamento}"), model);
        }

        public ItemFaturamentoDTO[] GetFaturamentoNormal(ConsultarFaturamentoDTO model)
        {
            return this.Post<ConsultarFaturamentoDTO, ItemFaturamentoDTO[]>(new Uri($"{ServicoRotas.Base}{ServicoRotas.Faturamento.PostFaturamentoComum}"), model);
        }
    }
}
