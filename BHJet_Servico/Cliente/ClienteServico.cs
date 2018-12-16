using BHJet_DTO.Cliente;
using System;

namespace BHJet_Servico.Cliente
{
    public interface IClienteServico
    {
        ClienteDTO[] BuscaListaClientes(string trechoPesquisa);
		ClienteDTO[] BuscaClientesValorAtivo();
		ClienteDTO[] BuscaClienteContrato(string trechoPesquisa);
		ClienteCompletoModel BuscaClientePorID(long clienteID);
		void IncluirCliente(ClienteCompletoModel clienteModel);
		void ExcluirContato(int idContato);
		void ExcluirValor(int idValor);
	}

    public class ClienteServico : ServicoBase, IClienteServico
    {
        public ClienteServico(string token) : base(token)
        {

        }

        public ClienteDTO[] BuscaListaClientes(string trechoPesquisa)
        {
            //return new ClienteDTO[]
            //{
            //    new ClienteDTO()
            //    {
            //         ID = 1,
            //         vcNomeFantasia = "Açai da eskina"
            //    },
            //     new ClienteDTO()
            //    {
            //         ID = 2,
            //         vcNomeFantasia = "AutoPeça Pedro II"
            //    }
            //};

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

		public void ExcluirContato(int idContato)
		{
			this.Delete(new Uri($"{ServicoRotas.Base}{string.Format(ServicoRotas.Cliente.DeleteContato, idContato)}"));
		}

		public void ExcluirValor(int idValor)
		{
			this.Delete(new Uri($"{ServicoRotas.Base}{string.Format(ServicoRotas.Cliente.DeleteValor, idValor)}"));
		}
	}
}
