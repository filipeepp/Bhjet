﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BHJet_Admin.Models.Dashboard
{
    public class DiariaModel
    {
        private DateTime _PeriodoInicial;
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
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
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
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

        private Dictionary<long, string> _ListaClientes;
        public Dictionary<long, string> ListaClientes
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

        private long _ClienteSelecionado;
        [Required(ErrorMessage = "Cliente obrigatório.")]
        public long ClienteSelecionado
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

        private Dictionary<long, string> _ListaProfissionais;
        public Dictionary<long, string> ListaProfissionais
        {
            get
            {
                return _ListaProfissionais;
            }
            set
            {
                _ListaProfissionais = value;
            }
        }

        private long _ProfissionalSelecionado;
        [Required(ErrorMessage = "Profissional obrigatório.")]
        public long ProfissionalSelecionado
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

        private decimal? _ValorDiaria;
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Valor(R$) Diária obrigatório.")]
        public decimal? ValorDiaria
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

        private decimal? _ValorComissao;
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Valor(R$) Comissão obrigatório.")]
        public decimal? ValorComissao
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

        private string _TipoVeiculo;
        [Required(ErrorMessage = "Tipo de veículo obrigatório.")]
        public string TipoVeiculo
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