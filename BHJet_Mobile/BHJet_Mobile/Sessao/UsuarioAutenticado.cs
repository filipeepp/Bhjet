﻿using BHJet_Enumeradores;
using BHJet_Mobile.Servico.Motorista.Model;
using System.Threading;

namespace BHJet_Mobile.Sessao
{
    public interface IUsuarioAutenticado
    {
        long IDProfissional { get; set; }
        TipoProfissional Tipo { get; set; }
        TipoContrato Contrato { get; set; }
        string Nome { get; set; }
        long? IDCorridaAtendimento { get; set; }
        long? IDDiariaAtendimento { get; set; }
        CancellationTokenSource CancelaPesquisa { get; set; }
        bool StatusAplicatico { get; set; }
        void SetPerfil(PerfilMotoristaModel perfil);
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

        public long IDProfissional { get; set; }

        public TipoProfissional Tipo { get; set; }

        public TipoContrato Contrato { get; set; }

        public string Nome { get; set; }

        public bool StatusAplicatico { get; set; }

        public long? IDCorridaAtendimento { get; set; }

        public long? IDDiariaAtendimento { get; set; }

        public CancellationTokenSource CancelaPesquisa { get; set; }

        public void SetPerfil(PerfilMotoristaModel perfil)
        {
            Instance.IDProfissional = perfil.idColaboradorEmpresaSistema;
            Instance.Nome = perfil.NomeCompleto;
            Instance.Tipo = perfil.TipoProfissional;
            Instance.Contrato = perfil.idRegistroDiaria == null ? BHJet_Enumeradores.TipoContrato.ChamadosAvulsos : BHJet_Enumeradores.TipoContrato.ContratoLocacao;
            Instance.Contrato = perfil.idRegistroDiaria == null ? BHJet_Enumeradores.TipoContrato.ChamadosAvulsos : BHJet_Enumeradores.TipoContrato.ContratoLocacao;
            Instance.IDDiariaAtendimento = perfil.idRegistroDiaria;
        }

        public void CancelaPesquisaChamado()
        {
            if (UsuarioAutenticado.Instance.CancelaPesquisa != null)
                UsuarioAutenticado.Instance.CancelaPesquisa.Cancel();
        }
    }
}
