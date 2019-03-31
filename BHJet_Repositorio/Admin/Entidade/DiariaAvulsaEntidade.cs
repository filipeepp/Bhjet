﻿using System;

namespace BHJet_Repositorio.Admin.Entidade
{
    public class DiariaAvulsaEntidade
    {
        public long idRegistroDiaria { get; set; }
        public int IDCliente { get; set; }
        public int IDTarifario { get; set; }
        public int IDColaboradorEmpresa { get; set; }
        public long IDUsuarioSolicitacao { get; set; }
        public string vcNomeCompleto { get; set; }
        public DateTime DataHoraInicioExpediente { get; set; }
        public int? OdometroInicioExpediente { get; set; }
        public DateTime? DataHoraInicioIntervalo { get; set; }
        public int? OdometroInicioIntervalo { get; set; }
        public DateTime? DataHoraFimIntervalo { get; set; }
        public DateTime? DataHoraFimExpediente { get; set; }
        public int? OdometroFimExpediente { get; set; }
        public decimal? ValorDiariaNegociado { get; set; }
        public decimal? ValorDiariaComissaoNegociado { get; set; }
        public decimal? ValorKMAdicionalNegociado { get; set; }
        public decimal? FranquiaKMDiaria { get; set; }
        public DateTime DataHoraSolicitacao { get; set; }
        public bool FaturarComoDiaria { get; set; }
    }
}

