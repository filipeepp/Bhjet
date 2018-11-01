using BHJet_Core.Utilitario;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BHJet_Admin.Models.Faturamento
{
    public class FaturamentoNormal
    {

        private int _MesSelecionado = 0;
        public int MesSelecionado
        {
            get
            {
                return _MesSelecionado;
            }
            set
            {
                _MesSelecionado = value;
            }
        }

        public IEnumerable<SelectListItem> Meses
        {
            get
            {
                return DataUtil.ListaMesesAno();
            }
        }

        private int _AnoSelecionado = DateTime.Now.Year;
        public int AnoSelecionado
        {
            get
            {
                return _AnoSelecionado;
            }
            set
            {
                _AnoSelecionado = value;
            }
        }

        public IEnumerable<int> ListaAno
        {
            get
            {
                return DataUtil.BuscaProximosAnos(10);
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

        public IEnumerable<int> ClientesSelecionados { get; set; }

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
    }
}