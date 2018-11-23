using BHJet_Core.Enum;
using BHJet_Core.Utilitario;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BHJet_Admin.Models.Dashboard
{
    public class DiariaModel
    {
        private DateTime _PeriodoInicial;
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Período inicial obrigatório.")]
        public DateTime PeriodoInicial
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

        private DateTime _PeriodoFinal;
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Período final obrigatório.")]
        public DateTime PeriodoFinal
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

        //private IEnumerable<SelectListItem> _ListaClientes;
        //public IEnumerable<SelectListItem> ListaClientes
        //{
        //    get
        //    {
        //        return _ListaClientes;
        //    }
        //    set
        //    {
        //        _ListaClientes = value;
        //    }
        //}

        private string _ClienteSelecionado;
        [Required(ErrorMessage = "Cliente obrigatório.")]
        public string ClienteSelecionado
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

        //private IEnumerable<SelectListItem> _ListaProfissionais;
        //public IEnumerable<SelectListItem> ListaProfissionais
        //{
        //    get
        //    {
        //        return _ListaProfissionais;
        //    }
        //    set
        //    {
        //        _ListaProfissionais = value;
        //    }
        //}

        private string _ProfissionalSelecionado;
        [Required(ErrorMessage = "Profissional obrigatório.")]
        public string ProfissionalSelecionado
        {
            get
            {
                return _ProfissionalSelecionado;
            }
            set
            {
                _ProfissionalSelecionado = value;
            }
        }

        private string _ValorDiaria;
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Valor(R$) Diária obrigatório.")]
        public string ValorDiaria
        {
            get
            {
                return _ValorDiaria;
            }
            set
            {
                _ValorDiaria = value;
            }
        }

        private string _ValorComissao;
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Valor(R$) Comissão obrigatório.")]
        public string ValorComissao
        {
            get
            {
                return _ValorComissao;
            }
            set
            {
                _ValorComissao = value;
            }
        }

        private string _Observacao;
        public string Observacao
        {
            get
            {
                return _Observacao;
            }
            set
            {
                _Observacao = value;
            }
        }

        private TipoVeiculo _TipoVeiculo;
        [Required(ErrorMessage = "Tipo de veículo obrigatório.")]
        public TipoVeiculo TipoVeiculo
        {
            get
            {
                return _TipoVeiculo;
            }
            set
            {
                _TipoVeiculo = value;
            }
        }
    }
}