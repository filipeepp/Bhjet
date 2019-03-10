using BHJet_Enumeradores;
using System.Collections.Generic;

namespace BHJet_Usuario.Models.Entregas
{
    public class EntregaModel
    {
        public List<EnderecoModel> Enderecos { get; set; }
    }

    public class EnderecoModel
    {
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Descricao { get; set; }
        public string ProcurarPessoa { get; set; }
        public TipoOcorrenciaCorrida TipoOcorrencia { get; set; }
        public string Observacao { get; set; }
    }
}