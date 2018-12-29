using BHJet_DTO.Tarifa;
using System;

namespace BHJet_Servico.Tarifa
{
    public interface ITarifaServico
    {
        TarifaDTO BuscaTaritaCliente(long? idCliente);
    }

    public class TarifaServico : ServicoBase, ITarifaServico
    {
        public TarifaServico(string token) : base(token)
        {

        }

        public TarifaDTO BuscaTaritaCliente(long? idCliente)
        {
            return this.Get<TarifaDTO>(new Uri($"{ServicoRotas.Base}{string.Format(ServicoRotas.Tarifa.GetTarifaCliente, idCliente)}"));
        }
    }
}
