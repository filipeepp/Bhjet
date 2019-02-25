using BHJet_Enumeradores;
using BHJet_Core.Utilitario;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BHJet_Admin.Models.Faturamento
{
    public class FaturamentoNormal
    {

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
                return DataUtil.BuscaProximosAnos(10);
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

        private Dictionary<int?, string> _ListaTipoContrato;
        public Dictionary<int?, string> ListaTipoContrato
        {
            get
            {
                return retornaListaTipoContrato();
            }
        }

        private TipoContrato? _TipoContratoSelecionado;
        public TipoContrato? TipoContratoSelecionado
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



        private Dictionary<int?, string> retornaListaTipoContrato()
        {
            var dic = new Dictionary<int?, string>();
            dic.Add(1, "Chamado Avulso");
            dic.Add(2, "Contrato Locacao");

            return dic;
        }

    }
}