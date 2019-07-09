using BHJet_Mobile.Servico.Corrida;
using BHJet_Mobile.Sessao;
using System.Threading.Tasks;
using Xamarin.Forms.Background;

namespace BHJet_Mobile.Servico
{
    public class SomeBackgroundWork : IBackgroundTask
    {
        public SomeBackgroundWork()
        {
        }

        public async Task StartJob()
        {
                //await new CorridaServico().LiberarOrdemServico(UsuarioAutenticado.Instance.IDCorridaPesquisada ?? 0);
        }
    }
}
