using BHJet_Mobile.Infra;
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
        public async Task<bool> ExecutarLogin()
        {
            try
            {
                // Carregando
                Loading = true;
                OffLoading = false;

                await Task.Delay(2000);

                if (Login.Username == "diaria")
                    return true;
                else
                    return false;
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
