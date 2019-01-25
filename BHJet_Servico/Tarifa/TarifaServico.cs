﻿using BHJet_DTO.Tarifa;
using System;

namespace BHJet_Servico.Tarifa
{
    public interface ITarifaServico
    {
        TarifaDTO BuscaTaritaCliente(long? idCliente);
		TarifaDTO BuscaTarifaAtiva(int codigoTipoVeiculo);
    }

    public class TarifaServico : ServicoBase, ITarifaServico
    {
        public TarifaServico(string token) : base(token)
        {

        }

        public TarifaDTO BuscaTaritaCliente(long? idCliente)
        {
            return this.Get<TarifaDTO>(new Uri($"{ServicoRotas.Base}{ServicoRotas.Tarifa.GetTarifaCliente}?idCliente={idCliente}"));
        }

		public TarifaDTO BuscaTarifaAtiva(int codigoTipoVeiculo)
		{
			return this.Get<TarifaDTO>(new Uri($"{ServicoRotas.Base}{string.Format(ServicoRotas.Tarifa.GetTarifaPadrao, codigoTipoVeiculo)}"));
		}
	}
}
