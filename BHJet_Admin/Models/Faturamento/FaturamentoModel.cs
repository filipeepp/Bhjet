using System;

namespace BHJet_Admin.Models.Faturamento
{

    public class FaturamentoModel
    {
        public string Cliente { get; set; }
        public DateTime Apuração { get; set; }
        public int TipoContrato { get; set; }
        public string DescContrato { get; set; }
        public Decimal? Valor { get; set; }
    }
}