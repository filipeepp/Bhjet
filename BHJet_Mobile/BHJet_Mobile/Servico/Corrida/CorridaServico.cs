using BHJet_Enumeradores;
using BHJet_Mobile.Infra.Variaveis;
using BHJet_Mobile.Servico.Corrida.Model;
using BHJet_Servico;
using System;
using System.Threading.Tasks;

namespace BHJet_Mobile.Servico.Corrida
{
    public interface ICorridaServico
    {
        Task<CorridaAbertaModel> BuscaCorridaAberta(TipoProfissional tipo);
    }

    public class CorridaServico : ServicoBase, ICorridaServico
    {
        /// <summary>
        /// Busca Corrida aberta
        /// </summary>
        /// <returns>ResumoModel</returns>
        public async Task<CorridaAbertaModel> BuscaCorridaAberta(TipoProfissional tipo)
        {
            return await this.Get<CorridaAbertaModel>(new Uri($"{ServicoRotas.Base}{string.Format(ServicoRotas.Corrida.GetAberta, (int)tipo)}"));
        }
    }
}
