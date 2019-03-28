﻿using BHJet_DTO.Corrida;
using System;

namespace BHJet_Servico.Corrida
{
    public interface ICorridaServico
    {
        DetalheCorridaModel BuscaDetalheCorrida(long idCorrida);
        OcorrenciaDTO[] BuscaOcorrencias();
        decimal CalculoPrecoCorrida(CalculoCorridaDTO filtro);
        long IncluirCorrida(IncluirCorridaDTO filtro);
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
        }

        public decimal CalculoPrecoCorrida(CalculoCorridaDTO filtro)
        {
            return this.Post<CalculoCorridaDTO, decimal>(new Uri($"{ServicoRotas.Base}{ServicoRotas.Corrida.GetPreco}"), filtro);
        }

        public long IncluirCorrida(IncluirCorridaDTO filtro)
        {
            return this.Post<IncluirCorridaDTO, long>(new Uri($"{ServicoRotas.Base}{ServicoRotas.Corrida.PostCorrida}"), filtro);
        }
    }
}
