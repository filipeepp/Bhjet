using BHJet_Enumeradores;
using BHJet_Core.Utilitario;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BHJet_Admin.Models.Dashboard
{
    public class DiariaModel
    {
        private string _PeriodoInicial;
        [Required(ErrorMessage = "Período inicial obrigatório.")]
        public string PeriodoInicial
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

        private string _PeriodoFinal;
        [Required(ErrorMessage = "Período final obrigatório.")]
        public string PeriodoFinal
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

        private long? _ClienteSelecionado;
        [Required(ErrorMessage = "Cliente obrigatório.")]
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

        private int? _TipoVeiculoSelecionado;
        [Required(ErrorMessage = "Veiculo obrigatório.")]
        public int? TipoVeiculoSelecionado
        {
            get
            {
                return _TipoVeiculoSelecionado;
            }
            set
            {
                _TipoVeiculoSelecionado = value;
            }
        }


        private TimeSpan? _HorarioInicial;
        [Required(ErrorMessage = "Horário inicial de trabalho obrigatória.")]
        public TimeSpan? HorarioInicial
        {
            get
            {
                return _HorarioInicial;
            }
            set
            {
                _HorarioInicial = value;
            }
        }

        private TimeSpan? _HorarioFim;
        [Required(ErrorMessage = "Horário final de trabalho obrigatória.")]
        public TimeSpan? HorarioFim
        {
            get
            {
                return _HorarioFim;
            }
            set
            {
                _HorarioFim = value;
            }
        }

        private int? _ProfissionalSelecionado;
        [Required(ErrorMessage = "Profissional obrigatório.")]
        public int? ProfissionalSelecionado
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

        private string _TipoProfissional;
        public string TipoProfissional
        {
            get
            {
                return _TipoProfissional;
            }
            set
            {
                _TipoProfissional = value;
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
        [Required(ErrorMessage = "Percentual(%) de Comissão obrigatório.")]
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

        private string _ValorKMAdicional;
        [DataType(DataType.Currency)]
        public string ValorKMAdicional
        {
            get
            {
                return _ValorKMAdicional;
            }
            set
            {
                _ValorKMAdicional = value;
            }
        }

        private long _FranquiaKMDiaria;
        public long FranquiaKMDiaria
        {
            get
            {
                return _FranquiaKMDiaria;
            }
            set
            {
                _FranquiaKMDiaria = value;
            }
        }
    }
}