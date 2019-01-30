using BHJet_Mobile.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace BHJet_Mobile.ViewModel.Corrida
{
    public class DetalheItemViewModel : PropertyChangedClass
    {
        private long _idCorrida;
        public long idCorrida
        {
            get
            {
                return _idCorrida;
            }
            set
            {
                _idCorrida = value;
                OnPropertyChanged();
            }
        }

        private long _idEnderecoCorrida;
        public long idEnderecoCorrida
        {
            get
            {
                return _idEnderecoCorrida;
            }
            set
            {
                _idEnderecoCorrida = value;
                OnPropertyChanged();
            }
        }

        private int? _IDOcorrencia;
        public int? IDOcorrencia
        {
            get
            {
                return _IDOcorrencia;
            }
            set
            {
                _IDOcorrencia = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _HoraChegada;
        public DateTime? HoraChegada
        {
            get
            {
                return _HoraChegada;
            }
            set
            {
                _HoraChegada = value;
                OnPropertyChanged();
            }
        }

        private int _Status;
        public int Status
        {
            get
            {
                return _Status;
            }
            set
            {
                _Status = value;
                OnPropertyChanged();
            }
        }

        private string _EnderecoCompleto;
        public string EnderecoCompleto
        {
            get
            {
                return _EnderecoCompleto;
            }
            set
            {
                _EnderecoCompleto = value;
                OnPropertyChanged();
            }
        }

        private string _PessoaContato;
        public string PessoaContato
        {
            get
            {
                return _PessoaContato;
            }
            set
            {
                _PessoaContato = value;
                OnPropertyChanged();
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
                OnPropertyChanged();
            }
        }

        private string _Atividade;
        public string Atividade
        {
            get
            {
                return _Atividade;
            }
            set
            {
                _Atividade = value;
                OnPropertyChanged();
            }
        }

        private decimal _Latitude;
        public decimal Latitude
        {
            get
            {
                return _Latitude;
            }
            set
            {
                _Latitude = value;
                OnPropertyChanged();
            }
        }

        private decimal _Longitude;
        public decimal Longitude
        {
            get
            {
                return _Longitude;
            }
            set
            {
                _Longitude = value;
                OnPropertyChanged();
            }
        }

        private string _TelefoneContato;
        public string TelefoneContato
        {
            get
            {
                return _TelefoneContato;
            }
            set
            {
                _TelefoneContato = value;
                OnPropertyChanged();
            }
        }

        private byte[] _RegistroFoto;
        public byte[] RegistroFoto
        {
            get
            {
                return _RegistroFoto;
            }
            set
            {
                _RegistroFoto = value;
                OnPropertyChanged();
            }
        }
    }

}
