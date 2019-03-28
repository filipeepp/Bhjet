using BHJet_Mobile.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace BHJet_Mobile.ViewModel.DiariaDeBordo
{
    public class TurnoItemViewModel : PropertyChangedClass
    {
        private string _InicioJornada;
        public string InicioJornada
        {
            get
            {
                return _InicioJornada;
            }
            set
            {
                _InicioJornada = value;
                OnPropertyChanged();
            }
        }

        private string _KMInicio;
        public string KMInicio
        {
            get
            {
                return _KMInicio;
            }
            set
            {
                _KMInicio = value;
                OnPropertyChanged();
            }
        }

        private string _InicioAlmoco;
        public string InicioAlmoco
        {
            get
            {
                return _InicioAlmoco;
            }
            set
            {
                _InicioAlmoco = value;
                OnPropertyChanged();
            }
        }

        private string _KMAlmoco;
        public string KMAlmoco
        {
            get
            {
                return _KMAlmoco;
            }
            set
            {
                _KMAlmoco = value;
                OnPropertyChanged();
            }
        }

        private string _FimAlmoco;
        public string FimAlmoco
        {
            get
            {
                return _FimAlmoco;
            }
            set
            {
                _FimAlmoco = value;
                OnPropertyChanged();
            }
        }

        private string _KMFimAlmoco;
        public string KMFimAlmoco
        {
            get
            {
                return _KMFimAlmoco;
            }
            set
            {
                _KMFimAlmoco = value;
                OnPropertyChanged();
            }
        }

        private string _FimJornada;
        public string FimJornada
        {
            get
            {
                return _FimJornada;
            }
            set
            {
                _FimJornada = value;
                OnPropertyChanged();
            }
        }

        private string _KMFim;
        public string KMFim
        {
            get
            {
                return _KMFim;
            }
            set
            {
                _KMFim = value;
                OnPropertyChanged();
            }
        }

        private string _NomeCliente;
        public string NomeCliente
        {
            get
            {
                return _NomeCliente;
            }
            set
            {
                _NomeCliente = value;
                OnPropertyChanged();
            }
        }

        private string _EnderecoCliente;
        public string EnderecoCliente
        {
            get
            {
                return _EnderecoCliente;
            }
            set
            {
                _EnderecoCliente = value;
                OnPropertyChanged();
            }
        }
    }
}
