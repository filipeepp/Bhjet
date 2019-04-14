using BHJet_Enumeradores;
using System;
using System.ComponentModel.DataAnnotations;

namespace BHJet_Admin.Models.Tarifario
{
    public class TarifarioModel
    {
        public TarifaModel[] Tarifas { get; set; }
    }

    public class TarifaModel
    {
        public int idTarifario { get; set; }
        public TipoServico idTipoServico { get; set; }
        public TipoVeiculo idTipoVeiculo { get; set; }
        public string DescricaoTarifario { get; set; }
        public DateTime? DataInicioVigencia { get; set; }
        public DateTime? DataFimVigencia { get; set; }

        [DataAnnotationsExtensions.Integer(ErrorMessage = "Informar uma valor númerico.")]
        public int? FranquiaMinutosParados { get; set; }

        public string ValorMinutoParado { get; set; }

        [Required(ErrorMessage = "Valor Contrato obrigatório.")]
        public string ValorContrato { get; set; }

        [DataAnnotationsExtensions.Integer(ErrorMessage = "Informar uma valor númerico.")]
        public int? FranquiaKM { get; set; }

        public string ValorKMAdicional { get; set; }

        [DataAnnotationsExtensions.Integer(ErrorMessage = "Informar uma valor númerico.")]
        public int? FranquiaHoras { get; set; }

        public string ValorHoraAdicional { get; set; }

        public string ValorPontoExcedente { get; set; }

        public string ValorPontoColeta { get; set; }

        public bool Ativo { get; set; }
        public string Observacao { get; set; }
    }
}