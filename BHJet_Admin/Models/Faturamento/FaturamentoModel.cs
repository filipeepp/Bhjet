﻿using System;

namespace BHJet_Admin.Models.Faturamento
{

    public class FaturamentoModel
    {
        public long ID { get; set; }
        public long IDCliente { get; set; }
        public long[] ListaOS { get; set; }
        public bool Selecionado { get; set; }
        public string Cliente { get; set; }
        public string Apuracao { get; set; }
        public int TipoContrato { get; set; }
        public string DescContrato { get; set; }
        public string Valor { get; set; }
    }
}