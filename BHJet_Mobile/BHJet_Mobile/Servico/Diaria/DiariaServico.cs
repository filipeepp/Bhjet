using BHJet_Mobile.Infra.Variaveis;
using BHJet_Mobile.Servico.Diaria.Model;
using BHJet_Mobile.Sessao;
using BHJet_Servico;
using System;
using System.Threading.Tasks;

namespace BHJet_Mobile.Servico.Diaria
{
    public interface IDiariaServico
    {
        Task<bool> VerificaDiariaAberta();
        Task<TurnoModel> BuscaTurno();
        Task AtualizaTurno(TurnoModel filtro);
    }

    public class DiariaServico : ServicoBase, IDiariaServico
    {
        /// <summary>
        /// Verifica se diaria foi aberta
        /// </summary>
        /// <returns>ResumoModel</returns>
        public async Task<bool> VerificaDiariaAberta()
        {
            return await this.Get<bool>(new Uri($"{ServicoRotas.Base}{ServicoRotas.Diaria.GetVerificacaoAberta}"));
        }

        /// <summary>
        /// Busca Turno
        /// </summary>
        /// <returns>ResumoModel</returns>
        public async Task<TurnoModel> BuscaTurno()
        {
            return await this.Get<TurnoModel>(new Uri($"{ServicoRotas.Base}{string.Format(ServicoRotas.Diaria.GetTurno, UsuarioAutenticado.Instance.IDDiaria)}"));
        }

        /// <summary>
        /// Atualiza Turno
        /// </summary>
        /// <returns>ResumoModel</returns>
        public async Task AtualizaTurno(TurnoModel filtro)
        {
            await this.Post(new Uri($"{ServicoRotas.Base}{string.Format(ServicoRotas.Diaria.PostTurno, UsuarioAutenticado.Instance.IDDiaria)}"), filtro);
        }
    }
}
