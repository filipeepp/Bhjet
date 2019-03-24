using BHJet_Enumeradores;
using System.Collections.Generic;

namespace BHJet_Repositorio.Admin.Filtro
{
    public class CorridaFiltro
    {
        public long? IDCliente { get; set; }
        public double? ValorEstimado { get; set; }
        public decimal? Comissao { get; set; }
        public int? TipoProfissional { get; set; }
        public List<EnderecoModel> Enderecos { get; set; }
    }

    public class EnderecoModel
    {
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Descricao { get; set; }
        public string ProcurarPessoa { get; set; }
        public int? TipoOcorrencia { get; set; }
        public string Observacao { get; set; }
    }
}
