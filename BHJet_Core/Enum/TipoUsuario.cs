using System.ComponentModel.DataAnnotations;

namespace BHJet_Core.Enum
{
    public enum TipoUsuario
    {
        [Display(Name = "Administrador")]
        Administrador = 1,
        [Display(Name = "Profissional Motorista")]
        Profissional = 2,
        [Display(Name = "Cliente Avulso Site")]
        ClienteAvulsoSite = 3,
        [Display(Name = "Funcionário Cliente")]
        FuncionarioCliente = 4
    }
}
