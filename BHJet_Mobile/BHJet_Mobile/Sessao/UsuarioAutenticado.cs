﻿using BHJet_Enumeradores;
using BHJet_Mobile.Infra.Database;
using BHJet_Mobile.Servico.Motorista;
using BHJet_Mobile.Servico.Motorista.Model;
using Plugin.Geolocator;
using System;
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
        CancellationTokenSource CancelaPesquisa { get; set; }
        bool StatusAplicatico { get; set; }
        void SetPerfil(PerfilMotoristaModel perfil);
        void FinalizaAtendimento();
    }

    public sealed class UsuarioAutenticado : IUsuarioAutenticado
    {
        private static UsuarioAutenticado instance;

        private UsuarioAutenticado() { }

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

        public long? IDCorridaAtendimento { get; set; }

        public CancellationTokenSource CancelaPesquisa { get; set; }

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

        public async void Sair()
        {
            Instance.CancelaPesquisaChamado();
            using (var db = new Database())
            {
                await db.LimpaTabela("BHJetMotorista");
            };
            Instance.IDProfissional = null;
            Instance.IDCorridaAtendimento = null;
            Instance.StatusAplicatico = false;
            instance = null;

            await CancelarDisponibilidade();
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
}
