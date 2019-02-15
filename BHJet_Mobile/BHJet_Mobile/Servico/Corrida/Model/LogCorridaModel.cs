using System;

namespace BHJet_Mobile.Servico.Corrida.Model
{
    public class LogCorridaModel
    {
        public long idCorrida { get; set; }
        public long idEnderecoCorrida { get; set; }
        public int? IDOcorrencia { get; set; }
        public DateTime? dtHoraChegada { get; set; }
        public int Status { get; set; }
        public string EnderecoCompleto { get; set; }
        public string PessoaContato { get; set; }
        public string Observacao { get; set; }
        public string Atividade { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string TelefoneContato { get; set; }
        public byte[] RegistroFoto { get; set; }
    }
}
