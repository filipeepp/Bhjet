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
        Task<CorridaAbertaModel> BuscaCorridaAberta(long idProfissional, TipoProfissional tipo);
        Task<LogCorridaModel[]> BuscaLogCorrida(long idCorrida);
        Task CadastraProtocolo(byte[] protocolo, long idEnderecoCorrida);
        Task RegistraChegaLogCorrida(long idEnderecoCorrida);
        Task<OcorrenciaModel[]> BuscaOcorrencias();
        Task EncerrarOrdemServico(long? statusCorrida, long idCorrida, EncerrarCorridaFiltro filtro);
        Task AtualizaOcorrenciaCorrida(long statusCorrida, long idCorrida);
        Task RecusarOrdemServico(long idCorrida);
        Task LiberarOrdemServico(long idCorrida);
    }

    public class CorridaServico : ServicoBase, ICorridaServico
    {
        /// <summary>
        /// Busca Corrida aberta
        /// </summary>
        /// <returns>ResumoModel</returns>
        public async Task<CorridaAbertaModel> BuscaCorridaAberta(long idProfissional, TipoProfissional tipo)
        {
            return await this.Get<CorridaAbertaModel>(new Uri($"{ServicoRotas.Base}{string.Format(ServicoRotas.Corrida.GetAberta, idProfissional, (int)tipo)}"));
        }

        /// <summary>
        /// Busca Corrida aberta
        /// </summary>
        /// <returns>ResumoModel</returns>
        public async Task<LogCorridaModel[]> BuscaLogCorrida(long idCorrida)
        {
            return await this.Get<LogCorridaModel[]>(new Uri($"{ServicoRotas.Base}{string.Format(ServicoRotas.Corrida.GetLog, idCorrida)}"));
        }

        /// <summary>
        /// Registra Chega Log Corrida
        /// </summary>
        /// <returns>ResumoModel</returns>
        public async Task RegistraChegaLogCorrida(long idEnderecoCorrida)
        {
            // status/{idStatus:long}/{idCorrida:long}
            await this.Put(new Uri($"{ServicoRotas.Base}{string.Format(ServicoRotas.Corrida.PutChegada, idEnderecoCorrida)}"), "");
        }

        /// <summary>
        /// Cadastra Protocolo Corrida
        /// </summary>
        /// <returns>ResumoModel</returns>
        public async Task CadastraProtocolo(byte[] protocolo, long idEnderecoCorrida)
        {
            await this.Upload(new Uri($"{ServicoRotas.Base}{string.Format(ServicoRotas.Corrida.PostProtocolo, idEnderecoCorrida)}"), protocolo);
        }

        /// <summary>
        /// Busca Ocorrencioas
        /// </summary>
        /// <returns>ResumoModel</returns>
        public async Task<OcorrenciaModel[]> BuscaOcorrencias()
        {
            return await this.Get<OcorrenciaModel[]>(new Uri($"{ServicoRotas.Base}{ServicoRotas.Corrida.GetOcorrencias}"));
        }

        /// <summary>
        /// Encerrar OS
        /// </summary>
        /// <returns>ResumoModel</returns>
        public async Task AtualizaOcorrenciaCorrida(long statusCorrida, long idCorrida)
        {
            // status/{idStatus:long}/{idCorrida:long}
            await this.Put(new Uri($"{ServicoRotas.Base}{string.Format(ServicoRotas.Corrida.PutOcorrenciaCorrida, statusCorrida, idCorrida)}"), "");
        }

        /// <summary>
        /// Encerrar OS 
        /// </summary>
        /// <returns>ResumoModel</returns>
        public async Task EncerrarOrdemServico(long? statusCorrida, long idCorrida, EncerrarCorridaFiltro filtro)
        {
            // status/{idStatus:long}/{idCorrida:long}
            await this.Put(new Uri($"{ServicoRotas.Base}{string.Format(ServicoRotas.Corrida.PutEncerrarCorrida, idCorrida, statusCorrida)}"), filtro);
        }

        /// <summary>
        /// RECUSAR OS 
        /// </summary>
        /// <returns>ResumoModel</returns>
        public async Task RecusarOrdemServico(long idCorrida)
        {
            // status/{idStatus:long}/{idCorrida:long}
            await this.Post(new Uri($"{ServicoRotas.Base}{string.Format(ServicoRotas.Corrida.PostRecusarCorrida, idCorrida)}"), "");
        }

        /// <summary>
        /// LIBERAR OS 
        /// </summary>
        /// <returns>ResumoModel</returns>
        public async Task LiberarOrdemServico(long idCorrida)
        {
            // status/{idStatus:long}/{idCorrida:long}
            await this.Post(new Uri($"{ServicoRotas.Base}{string.Format(ServicoRotas.Corrida.PostLiberarCorrida, idCorrida)}"), "");
        }
    }
}
