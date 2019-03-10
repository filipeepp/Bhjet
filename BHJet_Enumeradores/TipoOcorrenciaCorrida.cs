using System.ComponentModel.DataAnnotations;

namespace BHJet_Enumeradores
{
    public enum TipoOcorrenciaCorrida
    {
        [Display(Name = "Retirar documento(s) ou objeto(s) de pequeno porte.")]
        PequenoPorte = 1,
        [Display(Name = "Retirar objeto(s) de maior porte ou pacote(s).")]
        GrandePorte = 2,

        [Display(Name = "Entregar documento(s), objeto(s) ou pacote(s)")]
        EntregarItem = 3,
        [Display(Name = "Coletar assinaturas ou autenticar documento(s).")]
        ColherDocumento = 4,
        [Display(Name = "Retirar documento(s) ou objeto(s) de pequeno porte.")]
        RetirarObjetoPequeno = 5,
        [Display(Name = "Retirar objeto(s) de maior porte ou pacote(s).")]
        RetirarObjetoGrande = 6,
        [Display(Name = "Outros.")]
        Outros = 5,
    }
}
