using BHJet_Core.Utilitario;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace BHJet_Admin.Models.Faturamento
{
    public class GerarFaturamantoModel
    {
        private int _MesSelecionado = 1;
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

        private long? _ClienteSelecionado;
        public long? ClienteSelecionado
        {
            get
            {
                return _ClienteSelecionado;
            }
            set
            {
                _ClienteSelecionado = value;
            }
        }

        private IEnumerable<FaturamentoModel> _listaFaturamento = null;
        public IEnumerable<FaturamentoModel> ListaFaturamento { get => _listaFaturamento; set => _listaFaturamento = value; }
    }
}