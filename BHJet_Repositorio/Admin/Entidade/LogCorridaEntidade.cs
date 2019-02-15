using System;

namespace BHJet_Repositorio.Admin.Entidade
{
    public class LogCorridaEntidade
    {
        public long idCorrida { get; set; }
        public long Status { get; set; }
        public long idEnderecoCorrida { get; set; }
        public DateTime? dtHoraChegada { get; set; }
        public string EnderecoCompleto { get; set; }
        public string vcPessoaContato { get; set; }
        public string vcObservacao { get; set; }
        public bool bitEntregarDocumento { get; set; }
        public bool bitColetarAssinatura { get; set; }
        public bool bitRetirarDocumento { get; set; }
        public bool bitRetirarObjeto { get; set; }
        public bool bitEntregarObjeto { get; set; }
        public bool bitOutros { get; set; }
        public decimal vcLatitude { get; set; }
        public decimal vcLongitude { get; set; }
        public string vcTelefoneContato { get; set; }
        public byte[] Foto { get; set; }
        public int? IDOcorrencia { get; set; }
    }
}
