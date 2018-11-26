﻿using BHJet_DTO.Diaria;
using System;

namespace BHJet_Servico.Diaria
{
    public interface IDiariaServico
    {
        void IncluirDiaria(DiariaAvulsaDTO model);
    }

    public class DiariaServico : ServicoBase, IDiariaServico
    {
        public DiariaServico(string token) : base(token)
        {

        }

        /// <summary>
        /// Incluir um Diaria
        /// </summary>
        /// <returns>ResumoModel</returns>
        public void IncluirDiaria(DiariaAvulsaDTO model)
        {
            this.Post(new Uri($"{ServicoRotas.Base}{ServicoRotas.Diaria.PostDiaria}"), model);
        }
    }
}