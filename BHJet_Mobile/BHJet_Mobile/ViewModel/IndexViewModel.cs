using BHJet_Mobile.Infra;
using BHJet_Mobile.Sessao;
using System;
using System.Collections.Generic;
using System.Text;

namespace BHJet_Mobile.ViewModel
{
    public class IndexViewModel : PropertyChangedClass
    {
        public IndexViewModel(IUsuarioAutenticado _usuarioAutenticado)
        {
            usuarioAutenticado = _usuarioAutenticado;
        }

        private readonly IUsuarioAutenticado usuarioAutenticado;

        public bool DiarioBordo
        {
            get
            {
                return usuarioAutenticado.Contrato == BHJet_Enumeradores.TipoContrato.ContratoLocacao ? true : false;
            }
        }

        private ChamadoEncontradoItemViewModel _chamadoItem;
        public ChamadoEncontradoItemViewModel chamadoItem
        {
            get
            {
                return _chamadoItem;
            }
            set
            {
                _chamadoItem = value;
                OnPropertyChanged();
            }
        }

        private bool _PermitePesquisaCorrida;
        public bool PermitePesquisaCorrida
        {
            get
            {
                return _PermitePesquisaCorrida;
            }
            set
            {
                _PermitePesquisaCorrida = value;
                OnPropertyChanged();
            }
        }

        public string NomeCompleto
        {
            get
            {
                return usuarioAutenticado.Nome;
            }
        }

        public void Carrega(bool teste)
        {
            // Carrega dados de corrida
            if (usuarioAutenticado.Contrato == BHJet_Enumeradores.TipoContrato.ContratoLocacao)
            {
                // Verifica se a diaria foi aberta
                if (teste) // aberta
                    PermitePesquisaCorrida = false;
                else
                    PermitePesquisaCorrida = true;

                // Alerta Pesquisa
                if (!PermitePesquisaCorrida)
                    throw new DiariaException("Você está alocado para o cliente 'TALS', inicie o registro do turno para iniciar as corridas.");
            }
            else
            {
                // Avulso sempre permite pesquisa
                PermitePesquisaCorrida = true;
            }
        }


        public void BuscaCorrida()
        {
            // Buscando Corrida
            try
            {
                chamadoItem = new ChamadoEncontradoItemViewModel()
                {
                    NomeCliente = "Cliente fulano",
                    Comissao = "R$ 50,00",
                    DestinoInicial = "Rua teste, nº 10 - Bairro Centro"
                };
            }
            catch
            {

            }
        }


    }
}
