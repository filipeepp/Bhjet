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
        public bool bitColetarAssinatura { get; set; }
        public bool bitEntregarDocumento { get; set; }
        public bool bitEntregarObjeto { get; set; }
        public bool bitRetirarDocumento { get; set; }
        public bool bitRetirarObjeto { get; set; }
        public StatusCorrida StatusCorrida { get; set; }
        public DateTime? TempoEspera { get; set; }
        public string Observacao { get; set; }
        public byte[] CaminhoProtocolo { get; set; }
    }
}
