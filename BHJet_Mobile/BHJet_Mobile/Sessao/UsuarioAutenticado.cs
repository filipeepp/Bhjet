using BHJet_Enumeradores;
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
        long? IDCorridaPesquisada { get; set; }
        CancellationTokenSource CancelaPesquisa { get; set; }
        bool StatusAplicatico { get; set; }
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

        public bool StatusAplicatico { get; set; }

        public long? IDCorridaPesquisada { get; set; }

        public long? IDCorridaAtendimento { get; set; }

        public CancellationTokenSource CancelaPesquisa { get; set; }

        //public List<EsperaOcorrencia> TempoDeEspera { get; set; }

        public void SetPerfil(PerfilMotoristaModel perfil)
        {
            Instance.IDProfissional = perfil.idColaboradorEmpresaSistema;
            Instance.Nome = perfil.NomeCompleto;
            Instance.Tipo = perfil.TipoProfissional;
            Instance.Contrato = perfil.idRegistroDiaria == null ? BHJet_Enumeradores.TipoContrato.ChamadosAvulsos : BHJet_Enumeradores.TipoContrato.ContratoLocacao;
            Instance.Contrato = perfil.idRegistroDiaria == null ? BHJet_Enumeradores.TipoContrato.ChamadosAvulsos : BHJet_Enumeradores.TipoContrato.ContratoLocacao;
            Instance.IDCorridaAtendimento = perfil.idCorrida;
        }

        public void CancelaPesquisaChamado()
        {
            if (Instance.CancelaPesquisa != null)
                Instance.CancelaPesquisa.Cancel();
        }

        public void FinalizaAtendimento()
        {
            Instance.IDCorridaAtendimento = null;
            Instance.StatusAplicatico = true;
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
                Instance.StatusAplicatico = false;

                //
                await CancelarDisponibilidade();
            }
            finally
            {
                instance = null;
            }
        }

        public async Task CancelarDisponibilidade()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;
            var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(10));

            // Atualiza
            await new MotoristaServico().AtualizaDisponibilidade(new Servico.Motorista.Model.MotoristaDisponivelModel()
            {
                bitDisponivel = false,
                idTipoProfissional = Instance.Tipo,
                latitude = position.Latitude,
                longitude = position.Longitude
            });
        }
    }

    public class EsperaOcorrencia
    {
        public long IDOcorrencia { get; set; }
        public System.Timers.Timer MinutosEspera { get; set; }
    }
}
