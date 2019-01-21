using BHJet_Mobile.Infra.Variaveis;
using BHJet_Mobile.Servico.Motorista.Model;
using BHJet_Servico;
using System;

namespace BHJet_Mobile.Servico.Motorista
{
    public interface IMotoristaServico
    {
        PerfilMotoristaModel BuscaPerfilMotorista();
    }

    public class MotoristaServico : ServicoBase, IMotoristaServico
    {
        /// <summary>
        /// Busca Detalhe Corrida
        /// </summary>
        /// <returns>ResumoModel</returns>
        public PerfilMotoristaModel BuscaPerfilMotorista()
        {
            return this.Get<PerfilMotoristaModel>(new Uri($"{ServicoRotas.Base}{ServicoRotas.Motorista.GetPerfil}"));
        }
    }
}
