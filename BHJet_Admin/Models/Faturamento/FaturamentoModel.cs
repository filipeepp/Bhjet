using System;

namespace BHJet_Admin.Models.Faturamento
{

    public class FaturamentoModel
    {
        public long ID { get; set; }
        public string Cliente { get; set; }
        public string Apuração { get; set; }
        public int TipoContrato { get; set; }
        public string DescContrato { get; set; }
        public string Valor { get; set; }
    }
}