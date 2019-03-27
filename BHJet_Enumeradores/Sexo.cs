using System.ComponentModel.DataAnnotations;

namespace BHJet_Enumeradores
{
    public enum Sexo
    {
        [Display(Name = "Masculino")]
        Masculino = 0,
        [Display(Name = "Feminino")]
        Feminino = 1
    }
}
