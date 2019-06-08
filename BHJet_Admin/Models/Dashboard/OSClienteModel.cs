namespace BHJet_Admin.Models.Dashboard
{
    public class OSClienteModel
    {
        public long NumeroOS { get; set; }
        public long Cliente { get; set; }
        public long Motorista { get; set; }
        public OSClienteEnderecoModel Origem { get; set; }
        public OSClienteEnderecoModel[] Desinos { get; set; }
    }

    public class OSClienteEnderecoModel
    {
        public string EnderecoOrigem { get; set; }
        public string ProcurarPessoa { get; set; }
        public string Realizar { get; set; }
        public string Observacao { get; set; }
        public string Status { get; set; }
        public byte[] Foto { get; set; }
        public string Espera { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
}