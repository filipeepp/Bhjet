using BHJet_Mobile.Infra;

namespace BHJet_Mobile.ViewModel.Login
{
    public class LoginModel : PropertyChangedClass
    {
        public LoginModel()
        {
        }

        private string _username;
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
    }
}
