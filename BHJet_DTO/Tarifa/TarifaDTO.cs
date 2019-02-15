using System;

namespace BHJet_DTO.Tarifa
{
    public class TarifaDTO
    {
        public int idTarifario { get; set; }
        public decimal? decValorBandeirada { get; set; }
        public decimal? decFranquiaKMBandeirada { get; set; }
        public decimal? decValorKMAdicionalCorrida { get; set; }
        public int? intFranquiaMinutosParados { get; set; }
        public decimal? decValorMinutoParado { get; set; }
        public TimeSpan? timFaixaHorarioInicial { get; set; }
        public TimeSpan? timFaixaHorarioFinal { get; set; }
        public decimal? decValorDiaria { get; set; }
        public decimal? decFranquiaKMDiaria { get; set; }
        public decimal? decValorKMAdicionalDiaria { get; set; }
        public decimal decValorMensalidade { get; set; }
        public decimal decFranquiaKMMensalidade { get; set; }
        public decimal decValorKMAdicionalMensalidade { get; set; }
        public bool bitPagamentoAVista { get; set; }
		//ADICIONADO POR DIOGO - 24/01/2019 - CAMPO DE OBSERVAÇÃO PARA TARIFA PADRÃO
		public string vcObservacao { get; set; }
	}
}
