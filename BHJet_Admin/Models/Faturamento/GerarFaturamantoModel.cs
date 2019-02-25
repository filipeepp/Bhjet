using BHJet_Core.Utilitario;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BHJet_Admin.Models.Faturamento
{
    public class GerarFaturamantoModel
    {
        public GerarFaturamantoModel()
        {
        }

        private int _MesSelecionado = DateTime.Now.Month;
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
                return new int[] { DateTime.Now.Year };
            }
        }

        private IEnumerable<long> _ClienteSelecionado;
        public IEnumerable<long> ClienteSelecionado
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