using BHJet_Enumeradores;
using BHJet_Mobile.Infra;
using BHJet_Mobile.Servico.Motorista;
using BHJet_Mobile.Sessao;
using Plugin.Geolocator;
using System;
using System.Threading.Tasks;

namespace BHJet_Mobile.ViewModel.Util
{
    public class TipoVeiculoViewModel : PropertyChangedClass
    {
        public TipoVeiculoViewModel(IUsuarioAutenticado _usuarioAutenticado, IMotoristaServico _motoristaServico)
        {
            usuarioAutenticado = _usuarioAutenticado;
            motoristaServico = _motoristaServico;
        }

        private readonly IUsuarioAutenticado usuarioAutenticado;

        private readonly IMotoristaServico motoristaServico;

        public async Task SelecionaVeiculo(TipoProfissional tipo)
        {
            try
            {
                // Carregando
                Loading = true;

                // Start Motorista
                usuarioAutenticado.Tipo = tipo;
                var locator = CrossGeolocator.Current;

                // Busca Localizacao
                locator.DesiredAccuracy = 50;
                var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));

                // Altera situacao de pesquisa do aplicativo
                await motoristaServico.AtualizaDisponibilidade(new Servico.Motorista.Model.MotoristaDisponivelModel()
                {
                    bitDisponivel = UsuarioAutenticado.Instance.StatusAplicatico == BHJet_Enumeradores.StatusAplicativoEnum.Diarista ? false : true,
                    idTipoProfissional = usuarioAutenticado.Tipo,
                    latitude = position.Latitude,
                    longitude = position.Longitude
                });

                // Status
                UsuarioAutenticado.Instance.StatusAplicatico = StatusAplicativoEnum.Pesquisando;
            }
            finally
            {
                // Finaliza loading
                Loading = false;
            }
        }
    }
}
