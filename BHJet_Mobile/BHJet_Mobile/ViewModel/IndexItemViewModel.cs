using BHJet_Mobile.Infra;

namespace BHJet_Mobile.ViewModel
{
    public class ChamadoEncontradoItemViewModel : PropertyChangedClass
    {
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

        private string _Comissao;
        public string Comissao
        {
            get
            {
                return _Comissao;
            }
            set
            {
                _Comissao = value;
                OnPropertyChanged();
            }
        }

        private string _DestinoInicial;
        public string DestinoInicial
        {
            get
            {
                return _DestinoInicial;
            }
            set
            {
                _DestinoInicial = value;
                OnPropertyChanged();
            }
        }
    }
}
