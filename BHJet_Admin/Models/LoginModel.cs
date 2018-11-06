using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BHJet_Admin.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Usuário obrigatório.")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Senha obrigatória.")]
        [StringLength(200, ErrorMessage = "A senha deve conter no mínimo 8 caracteres.", MinimumLength = 8)]
        public string Senha { get; set; }
    }

    [DataContract(IsReference = true)]
    public class UsuarioModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UsuarioModel()
        {

        }

        public string USULOGIN { get; set; }

        public string USUNOME{ get; set; }

        public string USUSENHA { get; set; }

        public string USUTOKEN { get; set; }
    }

}