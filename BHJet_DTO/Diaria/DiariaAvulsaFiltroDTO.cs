using System;

namespace BHJet_DTO.Diaria
{
    public class DiariaAvulsaFiltroDTO
    {
        public long IDCliente { get; set; }
        public long IDColaboradorEmpresa { get; set; }
        public DateTime DataInicioExpediente { get; set; }
        public DateTime DataFimExpediente { get; set; }
        public TimeSpan HoraInicioExpediente { get; set; }
        public TimeSpan HoraFimExpediente { get; set; }
        public decimal ValorDiariaNegociado { get; set; }
        public decimal ValorDiariaComissaoNegociado { get; set; }
        public decimal ValorKMAdicionalNegociado { get; set; }
        public decimal FranquiaKMDiaria { get; set; }
    }
}
