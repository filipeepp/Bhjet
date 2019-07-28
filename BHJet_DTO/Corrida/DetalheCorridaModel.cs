using BHJet_Enumeradores;
using System;

namespace BHJet_DTO.Corrida
{
    public class DetalheCorridaModel
    {
        public long NumeroOS { get; set; }
        public long IDCliente { get; set; }
        public long IDProfissional { get; set; }
		public string NomeProfissional { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataInicio { get; set; }
        public decimal? ValorEstimado { get; set; }
        public decimal? ValorFinalizado { get; set; }
        public DetalheOSEnderecoModel Origem { get; set; }
        public DetalheOSEnderecoModel[] Destinos { get; set; }
    }

    public class DetalheOSEnderecoModel
    {
        public string EnderecoCompleto { get; set; }
        public string ProcurarPor { get; set; }
        public string Realizar { get; set; }
        public string Observacao { get; set; }
        public StatusCorrida StatusCorrida { get; set; }
        public TimeSpan? TempoEspera { get; set; }
        public byte[] CaminhoProtocolo { get; set; }
        public string vcLatitude { get; set; }
        public string vcLongitude { get; set; }
    }
}
