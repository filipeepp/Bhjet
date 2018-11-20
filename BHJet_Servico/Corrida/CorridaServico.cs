using BHJet_DTO.Corrida;
using System;

namespace BHJet_Servico.Corrida
{
    public interface ICorridaServico
    {
        DetalheCorridaModel BuscaDetalheCorrida(long idCorrida);
    }

    public class CorridaServico : ServicoBase, ICorridaServico
    {
        /// <summary>
        /// Busca Detalhe Corrida
        /// </summary>
        /// <returns>ResumoModel</returns>
        public DetalheCorridaModel BuscaDetalheCorrida(long idCorrida)
        {
            return new DetalheCorridaModel()
            {
                IDCliente = 1,
                IDProfissional = 2,
                NumeroOS = 1,
                Origem = new DetalheOSEnderecoModel()
                {
                    EnderecoCompleto = "Rua Padre Eustáquio, 12 - Belo Horizonte / MG",
                    ProcurarPor = "Barbara",
                    Realizar = "Pegar documento de transferência",
                    StatusCorrida = BHJet_Core.Enum.StatusCorrida.Concluida,
                    TempoEspera = new TimeSpan(0, 30, 0),
                    Observacao = "Recebido por outra pessoa"
                },
                Destinos = new DetalheOSEnderecoModel[]
                 {
                        new DetalheOSEnderecoModel()
                        {
                            EnderecoCompleto = "Av. Pedro Segundo, 52 - Belo Horizonte / MG",
                            ProcurarPor = "Pedro",
                            Realizar = "Entregar documento e pegar comprovante",
                            Observacao = "Perto de posto de gasolina",
                            StatusCorrida = BHJet_Core.Enum.StatusCorrida.ChegouNoEndereco,
                            TempoEspera = new TimeSpan(0, 10, 0)
                        },
                       new DetalheOSEnderecoModel()
                        {
                            EnderecoCompleto = "Rua Castelo branco, 105 - Belo Horizonte / MG",
                            ProcurarPor = "Diogo",
                            Realizar = "Entregar comprovante",
                            Observacao = "Entregar somente para o Diogo.",
                            StatusCorrida = BHJet_Core.Enum.StatusCorrida.EmRota,
                            TempoEspera = null
                        }
                 }
            };

            return this.Get<DetalheCorridaModel>(new Uri($"{ServicoRotas.Base}{string.Format(ServicoRotas.Corrida.GetDetalheCorridas, idCorrida)}"));
        }

    }
}
