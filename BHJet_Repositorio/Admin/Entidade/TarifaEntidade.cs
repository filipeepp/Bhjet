using System;

namespace BHJet_Repositorio.Admin.Entidade
{
    public class TarifaEntidade
    {
        public int idTarifario { get; set; }
        public string vcDescricaoTarifario { get; set; }
        public DateTime? dtDataInicioVigencia { get; set; }
        public DateTime? dtDataFimVigencia { get; set; }
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
        public bool bitAtivo { get; set; }
        public bool bitPagamentoAVista { get; set; }
        public string vcObservacao { get; set; }
    }
}
