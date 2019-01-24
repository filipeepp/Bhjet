using BHJet_Mobile.Infra.Variaveis;
using BHJet_Mobile.Servico.Motorista.Model;
using BHJet_Servico;
using System;
using System.Threading.Tasks;

namespace BHJet_Mobile.Servico.Motorista
{
    public interface IMotoristaServico
    {
        Task<PerfilMotoristaModel> BuscaPerfilMotorista();
    }

    public class MotoristaServico : ServicoBase, IMotoristaServico
    {
        /// <summary>
        /// Busca Detalhe Corrida
        /// </summary>
        /// <returns>ResumoModel</returns>
        public async Task<PerfilMotoristaModel> BuscaPerfilMotorista()
        {
            return await this.Get<PerfilMotoristaModel>(new Uri($"{ServicoRotas.Base}{ServicoRotas.Motorista.GetPerfil}"));
        }
    }
}
