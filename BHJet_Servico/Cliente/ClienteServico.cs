using BHJet_DTO.Cliente;
using BHJet_DTO.Corrida;
using System;

namespace BHJet_Servico.Cliente
{
    public interface IClienteServico
    {
        //Cliente Normal
        ClienteDTO[] BuscaListaClientes(string trechoPesquisa);
        ClienteDTO[] BuscaClientesValorAtivo();
        ClienteDTO[] BuscaClienteContrato(string trechoPesquisa);
        ClienteCompletoModel BuscaClientePorID(long clienteID);
        void IncluirCliente(ClienteCompletoModel clienteModel);
        void IncluirClienteAvulso(ClienteAvulsoDTO clienteModel);
        void IncluirContato(ClienteContatoModel contatoModel, long clienteID);
        void IncluirValor(ClienteValorModel valorModel, long clienteID);
        void EditarCliente(ClienteCompletoModel clienteModel);
        void ExcluirContato(int idContato);
        void ExcluirValor(int idValor);
        DadosBancariosDTO BuscaDadosBancariosCliente(long idCliente);

        //Cliente Avulso
        ClienteDTO[] BuscaClientesAvulsosValorAtivo();
        DetalheCorridaModel[] BuscaOsCliente(long clienteID);
    }

    public class ClienteServico : ServicoBase, IClienteServico
    {
        public ClienteServico(string token) : base(token)
        {

        }

        #region Cliente Normal
        public ClienteDTO[] BuscaListaClientes(string trechoPesquisa)
        {
            return this.Get<ClienteDTO[]>(new Uri($"{ServicoRotas.Base}{ServicoRotas.Cliente.GetClientes}?trecho={trechoPesquisa}"));
        }
        public ClienteDTO[] BuscaClienteContrato(string trechoPesquisa)
        {
            return this.Get<ClienteDTO[]>(new Uri($"{ServicoRotas.Base}{ServicoRotas.Cliente.GetClienteContrato}?trecho={trechoPesquisa}"));
        }

        public ClienteDTO[] BuscaClientesValorAtivo()
        {
            return this.Get<ClienteDTO[]>(new Uri($"{ServicoRotas.Base}{ServicoRotas.Cliente.GetClientesValorAtivo}"));
        }

        public ClienteCompletoModel BuscaClientePorID(long clienteID)
        {
            return this.Get<ClienteCompletoModel>(new Uri($"{ServicoRotas.Base}{string.Format(ServicoRotas.Cliente.GetClienteCompleto, clienteID)}"));
        }

        public void IncluirCliente(ClienteCompletoModel clienteModel)
        {
            this.Post(new Uri($"{ServicoRotas.Base}{ServicoRotas.Cliente.PostCliente}"), clienteModel);
        }

        public void IncluirClienteAvulso(ClienteAvulsoDTO clienteModel)
        {
            this.Post(new Uri($"{ServicoRotas.Base}{ServicoRotas.Cliente.PostClienteAvulso}"), clienteModel);
        }

        public void IncluirContato(ClienteContatoModel contatoModel, long clienteID)
        {
            this.Post(new Uri($"{ServicoRotas.Base}{string.Format(ServicoRotas.Cliente.PostClienteContato, clienteID)}"), contatoModel);
        }

        public void IncluirValor(ClienteValorModel valorModel, long clienteID)
        {
            this.Post(new Uri($"{ServicoRotas.Base}{string.Format(ServicoRotas.Cliente.PostClienteValor, clienteID)}"), valorModel);
        }

        public void EditarCliente(ClienteCompletoModel clienteModel)
        {
            this.Put(new Uri($"{ServicoRotas.Base}{string.Format(ServicoRotas.Cliente.PutCliente, clienteModel.ID)}"), clienteModel);
        }

        public void ExcluirContato(int idContato)
        {
            this.Delete(new Uri($"{ServicoRotas.Base}{string.Format(ServicoRotas.Cliente.DeleteContato, idContato)}"));
        }

        public void ExcluirValor(int idValor)
        {
            this.Delete(new Uri($"{ServicoRotas.Base}{string.Format(ServicoRotas.Cliente.DeleteValor, idValor)}"));
        }
        #endregion

        #region Cliente Avulso

        public ClienteDTO[] BuscaClientesAvulsosValorAtivo()
        {
            return this.Get<ClienteDTO[]>(new Uri($"{ServicoRotas.Base}{ServicoRotas.Cliente.GetClientesAvulsosValorAtivo}"));
        }

        public DadosBancariosDTO BuscaDadosBancariosCliente(long idCliente)
        {
            return this.Get<DadosBancariosDTO>(new Uri($"{ServicoRotas.Base}{string.Format(ServicoRotas.Cliente.GetDadosBancarios, idCliente)}"));
        }

        public DetalheCorridaModel[] BuscaOsCliente(long clienteID)
        {
            try
            {
                return this.Get<DetalheCorridaModel[]>(new Uri($"{ServicoRotas.Base}{string.Format(ServicoRotas.Corrida.GetCorridaCliente, clienteID)}"));
            }
            catch
            {
                return new DetalheCorridaModel[] { };
            }

        }

        #endregion
    }
}
