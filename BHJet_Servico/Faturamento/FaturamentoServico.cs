using BHJet_DTO.Faturamento;
using System;

namespace BHJet_Servico.Faturamento
{
    public interface IFaturamentoServico
    {
        ItemFaturamentoDTO[] GerarFaturamento(GerarFaturamentoDTO model);
        ItemFaturamentoDTO[] GetFaturamentoNormal(ConsultarFaturamentoDTO model);
        ItemFaturamentoDetalheDTO[] GetFaturamentoDetalhe(long idCliente, DateTime periodoIni, DateTime periodoFim);
        ItemFaturamentoDetalheDTO[] GetFaturamentoDetalhe(long idCliente);
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

        public ItemFaturamentoDetalheDTO[] GetFaturamentoDetalhe(long idCliente, DateTime periodoIni, DateTime periodoFim)
        {
            return this.Get<ItemFaturamentoDetalheDTO[]>(new Uri($"{ServicoRotas.Base}{ServicoRotas.Faturamento.GetFaturamentoDetalhe}?IDCliente={idCliente}&DataInicioFaturamentoFiltro={periodoIni.ToString("yyyy-MM-dd")}T00:00:00&DataFimFaturamentoFiltro={periodoFim.ToString("yyyy-MM-dd")}T00:00:00"));
        }

        public ItemFaturamentoDetalheDTO[] GetFaturamentoDetalhe(long idCliente)
        {
            return this.Get<ItemFaturamentoDetalheDTO[]>(new Uri($"{ServicoRotas.Base}{ServicoRotas.Faturamento.GetFaturamentoDetalhe}?IDCliente={idCliente}&DataInicioFaturamentoFiltro={null}&DataFimFaturamentoFiltro={null}"));
        }
    }
}
