using BHJet_Core.Enum;
using System;

namespace BHJet_Repositorio.Admin.Entidade
{
    public class OSCorridaEntidade
    {
        public long NumeroOS { get; set; }
        public long IDCliente { get; set; }
        public long IDProfissional { get; set; }
        public string EnderecoCompleto { get; set; }
        public string ProcurarPor { get; set; }
        public string Realizar { get; set; }
        public StatusCorrida StatusCorrida { get; set; }
        public TimeSpan? TempoEspera { get; set; }
        public string Observacao { get; set; }
        public byte[] CaminhoProtocolo { get; set; }
    }
}
