using System;

namespace BHJet_DTO.Diaria
{
    public class DiariaAvulsaDTO
    {
        public long ID { get; set; }
        public int IDCliente { get; set; }
        public int IDColaboradorEmpresa { get; set; }
        public string NomeColaboradorEmpresa { get; set; }
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
