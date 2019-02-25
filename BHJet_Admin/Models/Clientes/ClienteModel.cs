using BHJet_Enumeradores;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BHJet_Admin.Models.Clientes
{
    public class ClienteModel
    {
        public ClienteModel()
        {
            DadosCadastrais = new DadosCadastraisModel() { };
        }

        public long ID { get; set; }

        [Required(ErrorMessage = "Tipo de contrato obrigatório.")]
        public TipoContrato Contrato { get; set; }

        public DadosCadastraisModel DadosCadastrais { get; set; }

		public List<ContatoModel> Contato { get; set; }

		public ValorModel ValorCarro { get; set; }

        public ValorModel ValorMoto { get; set; }
    }
}