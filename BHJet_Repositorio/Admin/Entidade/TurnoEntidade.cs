using System;

namespace BHJet_Repositorio.Admin.Entidade
{
    public class TurnoEntidade
    {
        public string IDProfissional { get; set; }

        private string _DataInicio;
        public string DataInicio
        {
            get
            {
                return _DataInicio;
            }
            set
            {
                _DataInicio = value;
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
            }
        }

        private string _DataInicioIntervalo;
        public string DataInicioIntervalo
        {
            get
            {
                return _DataInicioIntervalo;
            }
            set
            {
                _DataInicioIntervalo = value;
            }
        }

        private string _KMInicioIntervalo;
        public string KMInicioIntervalo
        {
            get
            {
                return _KMInicioIntervalo;
            }
            set
            {
                _KMInicioIntervalo = value;
            }
        }

        private string _DataFimIntervalo;
        public string DataFimIntervalo
        {
            get
            {
                return _DataFimIntervalo;
            }
            set
            {
                _DataFimIntervalo = value;
            }
        }

        private string _KMFimInvervalo;
        public string KMFimInvervalo
        {
            get
            {
                return _KMFimInvervalo;
            }
            set
            {
                _KMFimInvervalo = value;
            }
        }

        private string _DataFim;
        public string DataFim
        {
            get
            {
                return _DataFim;
            }
            set
            {
                _DataFim = value;
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
            }
        }
    }
}
