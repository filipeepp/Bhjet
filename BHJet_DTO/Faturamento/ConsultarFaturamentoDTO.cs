﻿using BHJet_Enumeradores;
using System;
using System.Collections.Generic;

namespace BHJet_DTO.Faturamento
{
    public class ConsultarFaturamentoDTO
    {
        public IEnumerable<long> IdClienteFiltro { get; set; }
        public DateTime DataInicioFaturamentoFiltro { get; set; }
        public DateTime DataFimFaturamentoFiltro { get; set; }
        public TipoContrato? TipoContratoFiltro { get; set; }
    }

    public class ConsultarFaturamentoDetalheDTO
    {
        public long IDCliente { get; set; }
        public long[] IDOS { get; set; }
        public DateTime? DataInicioFaturamentoFiltro { get; set; }
        public DateTime? DataFimFaturamentoFiltro { get; set; }
    }
}
