namespace BHJet_Admin.Models.Clientes
{
    public class ValorModel
	{
        public int? idTarifario { get; set; }

        //[Required(ErrorMessage = "Valor Contrato obrigatório.")]
        public string ValorContrato { get; set; }

        //[Required(ErrorMessage = "Franquia KM obrigatório.")]
        [DataAnnotationsExtensions.Integer(ErrorMessage = "Informar uma valor númerico.")]
        public int? FranquiaKM { get; set; }

        //[Required(ErrorMessage = "Valor KM Adicional obrigatório.")]
        public string ValorKMAdicional { get; set; }

        //[Required(ErrorMessage = "Franquia Horas obrigatório.")]
        [DataAnnotationsExtensions.Integer(ErrorMessage = "Informar uma valor númerico.")]
        public int? FranquiaHoras { get; set; }

        //[Required(ErrorMessage = "Valor Hora Adicional obrigatório.")]
        public string ValorHoraAdicional { get; set; }

        //[Required(ErrorMessage = "Franquia Minutos Parados obrigatório.")]
        public int? FranquiaMinutosParados { get; set; }

        //[Required(ErrorMessage = "Valor Minuto Parado obrigatório.")]
        public string ValorMinutoParado { get; set; }

        public string Observacao { get; set; }
    }
}