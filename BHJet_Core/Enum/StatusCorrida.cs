using System.ComponentModel.DataAnnotations;

namespace BHJet_Core.Enum
{
    public enum StatusCorrida
    {
        Iniciada = 1,
        [Display(Name = "Chegou no Endereço")]
        ChegouNoEndereco = 2,
        [Display(Name = "Aguardando Atendimento")]
        AguardandoAtendimento = 3,
        [Display(Name = "Saiu do Endereço")]
        SaiuDoEndereco = 4,
        [Display(Name = "Em Rota")]
        EmRota = 5,
        [Display(Name = "Problemas no veículo")]
        ProblemasNoVeiculo = 6,
        [Display(Name = "Cliente Cancelou")]
        ClienteCancelou = 7,
        [Display(Name = "Pessoa Ausente")]
        PessoaAusente = 8,
        [Display(Name = "Objeto não disponível")]
        ObjetoNaoDisponível = 9,
        [Display(Name = "Concluída")]
        Concluida = 10,
    }
}
