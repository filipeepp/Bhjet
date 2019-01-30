using BHJet_Mobile.Infra;
using BHJet_Mobile.Infra.Extensao;
using BHJet_Mobile.Servico.Corrida;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BHJet_Mobile.ViewModel.Corrida
{
    public class OcorrenciaViewModel : PropertyChangedClass
    {
        public OcorrenciaViewModel(ICorridaServico _corridaServico)
        {
            corridaServico = _corridaServico;
        }

        private readonly ICorridaServico corridaServico;

        private ObservableCollection<OcorrenciaItemViewModel> _ListaOcorrencias;
        public ObservableCollection<OcorrenciaItemViewModel> ListaOcorrencias
        {
            get
            {
                return _ListaOcorrencias;
            }
            set
            {
                _ListaOcorrencias = value;
                OnPropertyChanged();
            }
        }


        public async Task CarregaTela()
        {
            // Carrega Dados
            var entidade = await corridaServico.BuscaOcorrencias();

            // Binding
            ListaOcorrencias = entidade.Select(oc => new OcorrenciaItemViewModel()
            {
                Descricao = oc.vcDescricaoStatus,
                IDOcorrencia = oc.idStatusCorrida
            }).ToObservable();
        }
    }

    public class OcorrenciaItemViewModel : PropertyChangedClass
    {
        private long _IDOcorrencia;
        public long IDOcorrencia
        {
            get
            {
                return _IDOcorrencia;
            }
            set
            {
                _IDOcorrencia = value;
                OnPropertyChanged();
            }
        }

        private string _Descricao;
        public string Descricao
        {
            get
            {
                return _Descricao;
            }
            set
            {
                _Descricao = value;
                OnPropertyChanged();
            }
        }
    }
}
