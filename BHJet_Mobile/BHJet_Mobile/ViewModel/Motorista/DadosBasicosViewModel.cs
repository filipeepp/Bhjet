using BHJet_Mobile.Infra;
using BHJet_Mobile.Servico.Motorista;
using BHJet_Mobile.Sessao;
using Plugin.Geolocator;
using System;
using System.Threading.Tasks;

namespace BHJet_Mobile.ViewModel.Motorista
{
    public class DadosBasicosViewModel : PropertyChangedClass
    {
        public DadosBasicosViewModel(IUsuarioAutenticado _usuarioAutenticado, IMotoristaServico _motoristaServico)
        {
            usuarioAutenticado = _usuarioAutenticado;
            motoristaServico = _motoristaServico;
        }

        private readonly IUsuarioAutenticado usuarioAutenticado;

        private readonly IMotoristaServico motoristaServico;

        private string _Nome;
        public string Nome
        {
            get
            {
                return _Nome;
            }
            set
            {
                _Nome = value;
                OnPropertyChanged();
            }
        }

        private string _TelefoneCelular;
        public string TelefoneCelular
        {
            get
            {
                return _TelefoneCelular;
            }
            set
            {
                _TelefoneCelular = value;
                OnPropertyChanged();
            }
        }

        private string _TelefoneFixo;
        public string TelefoneFixo
        {
            get
            {
                return _TelefoneFixo;
            }
            set
            {
                _TelefoneFixo = value;
                OnPropertyChanged();
            }
        }

        private bool _Wpp;
        public bool Wpp
        {
            get
            {
                return _Wpp;
            }
            set
            {
                _Wpp = value;
                OnPropertyChanged();
            }
        }

        public async Task Carrega()
        {
            try
            {
                // Load
                Loading = true;

                // Busca dados diaria de bordo
                var model = await motoristaServico.BuscaDadosBasicos();

                // Carrega dados
                Nome = model.NomeCompleto;
                TelefoneCelular = model.TelefoneCelular;
                TelefoneFixo = model.TelefoneResidencial;
                Wpp = model.CelularWpp;
            }
            finally
            {
                // Load
                Loading = false;
            }
        }

        public async Task AtualizarDados()
        {
            try
            {
                // Load
                Loading = true;

                // Atualiza dados
                await motoristaServico.AtualizaDadosBasicos(new Servico.Motorista.Model.MotoristaDadosBasicosModel()
                {
                    TelefoneCelular = TelefoneCelular,
                    TelefoneResidencial = TelefoneFixo,
                    CelularWpp = Wpp
                });
            }
            finally
            {
                // Load
                Loading = false;
            }
        }

    }
}
