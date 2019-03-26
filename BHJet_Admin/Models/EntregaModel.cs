using BHJet_Enumeradores;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BHJet_Admin.Models
{
    public class EntregaModel
    {
        public long? IDCliente { get; set; }
        public long? OSNumero { get; set; }
        public decimal? ValorCorrida { get; set; }
        [Required(ErrorMessage = "Tipo Profissional obrigatório.")]
        public TipoProfissional? TipoProfissional { get; set; }
        public List<EnderecoModel> Enderecos { get; set; }
    }

    public class EnderecoModel
    {
        public string Longitude { get; set; }
        public string Latitude { get; set; }

        [Required(ErrorMessage = "Endereço obrigatório.")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "Favor informar a pessoa a ser procurada.")]
        public string ProcurarPessoa { get; set; }

        [Required(ErrorMessage = "Informar o tipo de chamado a ser realizado.")]
        public int? TipoOcorrencia { get; set; }

        public string Observacao { get; set; }
    }
}