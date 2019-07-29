using BHJet_Enumeradores;
using BHJet_Mobile.Infra;
using BHJet_Mobile.Infra.Database;
using BHJet_Mobile.Servico.Motorista;
using BHJet_Mobile.Servico.Motorista.Model;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BHJet_Mobile.Sessao
{
    public interface IUsuarioAutenticado
    {
        long? IDProfissional { get; set; }
        TipoProfissional Tipo { get; set; }
        TipoContrato Contrato { get; set; }
        string Nome { get; set; }
        long? IDCorridaAtendimento { get; set; }
        long? IDDiaria { get; set; }
        CancellationTokenSource CancelaPesquisa { get; set; }
        CancellationTokenSource CancelaPorInatividade { get; set; }
        StatusAplicativoEnum StatusAplicatico { get; set; }
        void SetPerfil(PerfilMotoristaModel perfil);
        void FinalizaAtendimento();
        //List<EsperaOcorrencia> TempoDeEspera { get; set; }
    }

    public sealed class UsuarioAutenticado : IUsuarioAutenticado
    {
        private static UsuarioAutenticado instance;

        private UsuarioAutenticado()
        {
            // TempoDeEspera = new List<EsperaOcorrencia>();
        }

        public static UsuarioAutenticado Instance
        {
            get
            {
                if (instance == null)
                    lock (typeof(UsuarioAutenticado))
                        if (instance == null) instance = new UsuarioAutenticado();

                return instance;
            }
        }

        public long? IDProfissional { get; set; }

        public TipoProfissional Tipo { get; set; }

        public TipoContrato Contrato { get; set; }

        public string Nome { get; set; }

        public StatusAplicativoEnum StatusAplicatico { get; set; }

        public long? IDCorridaAtendimento { get; set; }

        public long? IDDiaria { get; set; }

        public CancellationTokenSource CancelaPesquisa { get; set; }

        public CancellationTokenSource CancelaPorInatividade { get; set; }

        //public List<EsperaOcorrencia> TempoDeEspera { get; set; }

        public void SetPerfil(PerfilMotoristaModel perfil)
        {
            Instance.IDProfissional = perfil.idColaboradorEmpresaSistema;
            Instance.Nome = perfil.NomeCompleto;
            Instance.Tipo = perfil.TipoProfissional;
            Instance.Contrato = perfil.idRegistroDiaria == null ? BHJet_Enumeradores.TipoContrato.ChamadosAvulsos : BHJet_Enumeradores.TipoContrato.ContratoLocacao;
            Instance.IDDiaria = perfil.idRegistroDiaria;
            Instance.IDCorridaAtendimento = perfil.idCorrida;

            if (Instance.Contrato == TipoContrato.ChamadosAvulsos && Instance.IDCorridaAtendimento != null)
                Instance.StatusAplicatico = StatusAplicativoEnum.Atendimento;
            else if (Instance.Contrato == TipoContrato.ContratoLocacao)
                Instance.StatusAplicatico = StatusAplicativoEnum.Diarista;
        }

        public void CancelaPesquisaChamado()
        {
            if (Instance.CancelaPesquisa != null)
                Instance.CancelaPesquisa.Cancel();
            UsuarioAutenticado.Instance.AlteraDisponibilidade(false, false);
            GlobalVariablesManager.SetApplicationCurrentProperty(GlobalVariablesManager.VariaveisGlobais.DadosCorridaPesquisada, null);
        }

        public void FinalizaAtendimento()
        {
            Instance.IDCorridaAtendimento = null;
            Instance.StatusAplicatico = StatusAplicativoEnum.Pesquisando;
            GlobalVariablesManager.SetApplicationCurrentProperty(GlobalVariablesManager.VariaveisGlobais.DadosCorridaPesquisada, null);
        }

        public async Task Sair()
        {
            try
            {
                //
                Instance.CancelaPesquisaChamado();

                //
                using (var db = new Database())
                {
                    await db.LimpaTabela("BHJetMotorista");
                };

                // Variaveis
                Instance.IDProfissional = null;
                Instance.IDCorridaAtendimento = null;
                Instance.StatusAplicatico = StatusAplicativoEnum.Pausado;

                //
                await AlteraDisponibilidade();

                //
                GlobalVariablesManager.SetApplicationCurrentProperty(GlobalVariablesManager.VariaveisGlobais.DadosCorridaPesquisada, null);
            }
            finally
            {
                instance = null;
            }
        }

        public async Task AlteraDisponibilidade(bool disponibilidade = false, bool lancarErro = true)
        {
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;
                var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));

                // Atualiza
                await new MotoristaServico().AtualizaDisponibilidade(new Servico.Motorista.Model.MotoristaDisponivelModel()
                {
                    bitDisponivel = disponibilidade,
                    idTipoProfissional = Instance.Tipo,
                    latitude = position.Latitude,
                    longitude = position.Longitude
                });
            }
            catch
            {
                if (lancarErro)
                    throw;
            }
        }
    }

    public class EsperaOcorrencia
    {
        public long IDOcorrencia { get; set; }
        public System.Timers.Timer MinutosEspera { get; set; }
    }
}
