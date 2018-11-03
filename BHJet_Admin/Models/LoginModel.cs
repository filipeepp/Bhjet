using System.ComponentModel.DataAnnotations;

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
}