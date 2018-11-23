using BHJet_DTO.Profissional;
using System;
using System.Collections.Generic;

namespace BHJet_Servico.Profissional
{
    public interface IProfissionalServico
    {
        IEnumerable<ProfissionalModel> BuscaProfissionais();
        ProfissionalCompletoModel BuscaProfissional(long id);
        void AtualizaDadosProfissional(ProfissionalCompletoModel proModel);
        void IncluirProfissional(ProfissionalCompletoModel proModel);
    }

    public class ProfissionalServico : ServicoBase, IProfissionalServico
    {
        /// <summary>
        /// Busca Lista de profissionais
        /// </summary>
        /// <returns>ResumoModel</returns>
        public IEnumerable<ProfissionalModel> BuscaProfissionais()
       {
            return new List<ProfissionalModel>()
            {
                new ProfissionalModel()
                {
                     ID = 1,
                      NomeCompleto = "Fulano",
                       TipoProfissional = BHJet_Core.Enum.TipoProfissional.Motociclista,
                        TipoRegime = BHJet_Core.Enum.RegimeContratacao.CLT
                },
                new ProfissionalModel()
                {
                     ID = 1,
                      NomeCompleto = "Jose da Silva",
                       TipoProfissional = BHJet_Core.Enum.TipoProfissional.Motociclista,
                        TipoRegime = BHJet_Core.Enum.RegimeContratacao.CLT
                },
                new ProfissionalModel()
                {
                     ID = 1,
                      NomeCompleto = "Pedro",
                       TipoProfissional = BHJet_Core.Enum.TipoProfissional.Motociclista,
                        TipoRegime = BHJet_Core.Enum.RegimeContratacao.CLT
                }
            };

            return this.Get<IEnumerable<ProfissionalModel>>(new Uri($"{ServicoRotas.Base}{ServicoRotas.Profissional.GetProfissionais}"));
        }

        /// <summary>
        /// Busca Detalhe de Profissional especifico
        /// </summary>
        /// <returns>ResumoModel</returns>
        public ProfissionalCompletoModel BuscaProfissional(long id)
        {
            return new ProfissionalCompletoModel()
            {
                NomeCompleto = "Fulano",
                TipoProfissional = BHJet_Core.Enum.TipoProfissional.Motociclista,
                TipoRegime = BHJet_Core.Enum.RegimeContratacao.CLT,
                Cep = "30510080",
                Email = "teste@teste.com.br",
                TipoCNH = BHJet_Core.Enum.TipoCarteira.A,
                CPF = "09733322225",
                TelefoneCelular = "31971656958",
                 UF = "MG"
            };

            return this.Get<ProfissionalCompletoModel>(new Uri($"{ServicoRotas.Base}{string.Format(ServicoRotas.Profissional.GetProfissional, id)}"));
        }

        /// <summary>
        /// Atualiza dados de um Profissional especifico
        /// </summary>
        /// <returns>ResumoModel</returns>
        public void AtualizaDadosProfissional(ProfissionalCompletoModel proModel)
        {
            return;
            this.Put(new Uri($"{ServicoRotas.Base}{string.Format(ServicoRotas.Profissional.PutProfissional, proModel.ID)}"), proModel);
        }

        /// <summary>
        /// Incluir um Profissional
        /// </summary>
        /// <returns>ResumoModel</returns>
        public void IncluirProfissional(ProfissionalCompletoModel proModel)
        {
            return;
            long idUsuarioLogado = 1;

            this.Post(new Uri($"{ServicoRotas.Base}{ServicoRotas.Profissional.PutProfissional}{"?idGestorInclusao=" + idUsuarioLogado}"), proModel);
        }
    }
}
