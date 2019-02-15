using System;

namespace BHJet_DTO.Corrida
{
    public class LogCorridaModel
    {
        public long idCorrida { get; set; }
        public long idEnderecoCorrida { get; set; }
        public DateTime? dtHoraChegada { get; set; }
        public long Status { get; set; }
        public string EnderecoCompleto { get; set; }
        public string PessoaContato { get; set; }
        public string Observacao { get; set; }
        public string Atividade { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string TelefoneContato { get; set; }
        public byte[] RegistroFoto { get; set; }
        public int? IDOcorrencia { get; set; }
    }
}
