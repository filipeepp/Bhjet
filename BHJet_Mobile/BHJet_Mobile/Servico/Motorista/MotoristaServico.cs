using BHJet_Mobile.Infra.Variaveis;
using BHJet_Mobile.Servico.Motorista.Model;
using BHJet_Mobile.Sessao;
using BHJet_Servico;
using System;
using System.Threading.Tasks;

namespace BHJet_Mobile.Servico.Motorista
{
    public interface IMotoristaServico
    {
        Task<PerfilMotoristaModel> BuscaPerfilMotorista();
        Task<MotoristaDadosBasicosModel> BuscaDadosBasicos();
        Task AtualizaDadosBasicos(MotoristaDadosBasicosModel proModel);
        Task AtualizaDisponibilidade(MotoristaDisponivelModel disponibilidadeFiltro);
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

        /// <summary>
        /// Busca Detalhe Corrida
        /// </summary>
        /// <returns>ResumoModel</returns>
        public async Task<MotoristaDadosBasicosModel> BuscaDadosBasicos()
        {
            return await this.Get<MotoristaDadosBasicosModel>(new Uri($"{ServicoRotas.Base}{string.Format(ServicoRotas.Motorista.GetDadosBasicos, UsuarioAutenticado.Instance.IDProfissional)}"));
        }

        /// <summary>
        /// Busca Detalhe Corrida
        /// </summary>
        /// <returns>ResumoModel</returns>
        public async Task AtualizaDadosBasicos(MotoristaDadosBasicosModel proModel)
        {
            await this.Put(new Uri($"{ServicoRotas.Base}{string.Format(ServicoRotas.Motorista.PutDadosBasicos, UsuarioAutenticado.Instance.IDProfissional)}"), proModel);
        }

        /// <summary>
        /// Busca Detalhe Corrida
        /// </summary>
        /// <returns>ResumoModel</returns>
        public async Task AtualizaDisponibilidade(MotoristaDisponivelModel disponibilidadeFiltro)
        {
            await this.Put(new Uri($"{ServicoRotas.Base}{ServicoRotas.Motorista.PutDisponibilidade}"), disponibilidadeFiltro);
        }
    }
}
