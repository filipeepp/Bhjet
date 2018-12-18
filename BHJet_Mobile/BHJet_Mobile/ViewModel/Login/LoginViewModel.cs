using BHJet_Mobile.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace BHJet_Mobile.ViewModel.Login
{
    public class LoginViewModel : PropertyChangedClass
    {
        /// <summary>
        /// Construtor LoginViewModel
        /// </summary>
        /// <param name="_BeneficiarioNegocio"></param>
        public LoginViewModel()
        {
            Login = new LoginModel();
        }

        /// <summary>
        /// Usuario Logado
        /// </summary>
        public LoginModel Login { get; set; }

        /// <summary>
        /// Metodo de Login
        /// </summary>
        /// <returns></returns>
        public async Task ExecutarLogin()
        {
            try
            {
                // Carregando
                Loading = true;
                OffLoading = false;

                await Task.Delay(5000);

            }
            finally
            {
                // Finaliza loading
                Loading = false;
                OffLoading = true;
            }

        }
    }
}
