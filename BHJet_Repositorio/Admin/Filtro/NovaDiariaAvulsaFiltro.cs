using System;

namespace BHJet_Repositorio.Admin.Filtro
{
    public class NovaDiariaAvulsaFiltro
    {
        public long IDCliente { get; set; }
        public long IDColaboradorEmpresa { get; set; }
        public long IDUsuarioSolicitacao { get; set; }
        public DateTime DataHoraInicioExpediente { get; set; }
        public DateTime DataHoraFimExpediente { get; set; }
        public decimal ValorDiariaNegociado { get; set; }
        public decimal ValorDiariaComissaoNegociado { get; set; }
        public decimal ValorKMAdicionalNegociado { get; set; }
        public decimal FranquiaKMDiaria { get; set; }
    }
}
