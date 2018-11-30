using BHJet_Core.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BHJet_Admin.Models.Clientes
{
	public class ValorModel
	{
        //[Required(ErrorMessage = "O valor unitário é obrigatório.")]
        public string ValorUnitario { get; set; }

        //[Required(ErrorMessage = "O tipo da tarifa é obrigatório.")]
        public TipoTarifa TipoTarifa { get; set; }

        //[Required(ErrorMessage = "A data de início de vigência é obrigatório.")]
        public DateTime VigenciaInicio { get; set; }

        //[Required(ErrorMessage = "A data de fim de vigência é obrigatório.")]
        public DateTime VigenciaFim { get; set; }

        //[Required(ErrorMessage = "A franquia é obrigatório.")]
        //[RegularExpression(@"^[0-9]+(\.[0-9]{1,2})$", ErrorMessage = "Forneça um número válido")]
        public string Franquia { get; set; }

        //[RegularExpression(@"^[0-9]+(\.[0-9]{1,2})$", ErrorMessage = "Forneça um número válido")]
        public string FranquiaAdicional { get; set; }

		public string Observacao { get; set; }
	}
}