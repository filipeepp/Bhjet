using BHJet_Core.Enum;
using System;
using System.Collections.Generic;

namespace BHJet_Admin.Models.Faturamento
{
    public class DetalheFaturamentoModel
    {
        public string Cliente { get; set; }
        public string Contrato { get; set; }
        public string PeriodoIntervalo { get; set; }
        public DateTime DataRelatorio { get; set; }
        public IEnumerable<DetalheCorridaFaturamentoModel> Detalhes { get; set; }
        public DetalheTotalFaturamentoModel Total { get; set; }
    }

    public class DetalheCorridaFaturamentoModel
    {
        public string Mensageiro { get; set; }
        public IEnumerable<DetalheLogCorridaFaturamentoModel> LogCorrida { get; set; }
        public DetalheTotalFaturamentoModel DetalheTotal { get; set; }
    }

    public class DetalheLogCorridaFaturamentoModel
    {
        public DateTime Data { get; set; }
        public TimeSpan InicioDiaria { get; set; }
        public TimeSpan InicioAlmoco { get; set; }
        public TimeSpan FinalAlmoco { get; set; }
        public TimeSpan FinalDiaria { get; set; }
        public double KMRodado { get; set; }
        public double ValorTransporte { get; set; }
        public TipoProfissional Tipo { get; set; }
    }

    public class DetalheTotalFaturamentoModel
    {
        public double TotalKm { get; set; }
        public double FranquiaContratada { get; set; }
        public double KMExcedente { get; set; }
        public int TotalDiarias { get; set; }
        public int ValorDiaria { get; set; }
        public double ValorDiarias { get; set; }
        public double ValorExcedente { get; set; }
        public double TotalFatura { get; set; }
    }

    public class DetalheFaturamentoAvulso
    {
        public string Cliente { get; set; }
        //public string Contrato { get; set; }
        public string PeriodoIntervalo { get; set; }
        public string DataRelatorio { get; set; }
       public DetalheFaturamentoAvulsoRegistros[] Registros { get; set; }
    }

    public class DetalheFaturamentoAvulsoRegistros
    {
        public DateTime DataCorrida { get; set; }
        public string TipoContrato { get; set; }
        public long NumeroOS { get; set; }
        public string Profissional { get; set; }
        public int QuantidadeKM { get; set; }
        public decimal Valor { get; set; }
    }

}