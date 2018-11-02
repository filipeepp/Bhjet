using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BHJet_Admin.Models.Faturamento
{
    public class FaturamentoAvulsoModel
    {
        private DateTime? _PeriodoInicial;
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? PeriodoInicial
        {
            get
            {
                return _PeriodoInicial;
            }
            set
            {
                _PeriodoInicial = value;
            }
        }

        private DateTime? _PeriodoFinal;
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? PeriodoFinal
        {
            get
            {
                return _PeriodoFinal;
            }
            set
            {
                _PeriodoFinal = value;
            }
        }

        private Dictionary<int, string> _ListaClientes;
        public Dictionary<int, string> ListaClientes
        {
            get
            {
                return _ListaClientes;
            }
            set
            {
                _ListaClientes = value;
            }
        }

        public IEnumerable<int?> ClientesSelecionados { get; set; }

        private Dictionary<int, string> _ListaTipoContrato;
        public Dictionary<int, string> ListaTipoContrato
        {
            get
            {
                return _ListaTipoContrato;
            }
            set
            {
                _ListaTipoContrato = value;
            }
        }

        private long? _TipoContratoSelecionado;
        public long? TipoContratoSelecionado
        {
            get
            {
                return _TipoContratoSelecionado;
            }
            set
            {
                _TipoContratoSelecionado = value;
            }
        }

        private IEnumerable<FaturamentoModel> _listaFaturamento = null;
        public IEnumerable<FaturamentoModel> ListaFaturamento { get => _listaFaturamento; set => _listaFaturamento = value; }
    }
}