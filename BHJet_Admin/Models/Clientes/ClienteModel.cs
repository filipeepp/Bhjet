using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace BHJet_Admin.Models.Clientes
{
    public class ClienteModel
    {
        public long? ID { get; set; }
        public DadosCadastraisModel DadosCadastrais { get; set; }
		public List<ContatoModel> Contato { get; set; }
		public List<ValorModel> Valor { get; set; }
	

        public ClienteModel()
        {
			DadosCadastrais = new DadosCadastraisModel() { };
		}
    }
}