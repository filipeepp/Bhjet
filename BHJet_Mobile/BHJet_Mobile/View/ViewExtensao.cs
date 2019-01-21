using System;
using System.Linq;
using Xamarin.Forms;

namespace BHJet_Mobile.View
{
    public static class ViewExtensao
    {
        /// <summary>
        /// Trata Exception
        /// </summary>
        /// <param name="me"></param>
        /// <param name="ex"></param>
        public async static void TrataExceptionMobile(this Page me, Exception ex)
        {
            switch ((string)ex.GetType().Name)
            {
                case "ExceptionMobile":
                    await me.DisplayAlert("Atenção!", "Serviço Indisponível no momento.", "OK");
                    break;
                case "WarningException":
                    await me.DisplayAlert("Atenção!", ex.Message, "OK");
                    // Exibe a mensagem
                    if (me.Navigation.NavigationStack.Count() > 1)
                        await me.Navigation.PopAsync(false);
                    break;
                case "UnauthorizedAccessException":
                    await me.DisplayAlert("Atenção!", "Sessão expirada, favor realizar o login novamente.", "OK");
                    TrocarMainPage(me, new LoginPage());
                    break;
                default:
                    await me.DisplayAlert("Atenção!", "Serviço Indisponível no momento.", "OK");
                    break;
            }
        }

        /// <summary>
        /// Troca MainPage
        /// </summary>
        /// <param name="me"></param>
        /// <param name="newPage"></param>
        public static void TrocarMainPage(this Page me, Page newPage)
        {
            // Redireciona para o Login
            if (App.Current.MainPage is IDisposable)
                (App.Current.MainPage as IDisposable).Dispose();

            App.Current.MainPage = newPage;
        }
    }
}
