using BHJet_DTO.Tarifa;
using System;

namespace BHJet_Servico.Tarifa
{
    public interface ITarifaServico
    {
        TarifaDTO[] BuscaTaritasCliente(long idCliente);
		TarifaDTO BuscaTarifarioPadrao();
	}

    public class TarifaServico : ServicoBase, ITarifaServico
    {
        public TarifaServico(string token) : base(token)
        {

        }

        public TarifaDTO[] BuscaTaritasCliente(long idCliente)
        {
            //return new TarifaDTO[]
            //{
            //        new TarifaDTO()
            //        {
            //         ID = 1,
            //         Descricao = "Tarifa composta",
            //          ValorDiaria = 1552
            //        }
            //};

            return this.Get<TarifaDTO[]>(new Uri($"{ServicoRotas.Base}{string.Format(ServicoRotas.Tarifa.GetTarifaCliente, idCliente)}"));
        }

		public TarifaDTO BuscaTarifarioPadrao()
		{
			return this.Get<TarifaDTO>(new Uri($"{ServicoRotas.Base}{ServicoRotas.Tarifa.GetTarifaPadrao}"));
		}

	}
}
