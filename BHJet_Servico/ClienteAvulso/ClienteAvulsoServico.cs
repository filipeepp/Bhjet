using BHJet_DTO.ClienteAvulso;
using System;
using System.Collections.Generic;
using System.Text;

namespace BHJet_Servico.ClienteAvulso
{
	public interface IClienteAvulsoServico
	{
		ClienteAvulsoDTO[] BuscaClientesAvulsosValorAtivo();
	}

	public class ClienteAvulsoServico : ServicoBase, IClienteAvulsoServico
	{
		public ClienteAvulsoServico(string token) : base(token)
		{

		}

		public ClienteAvulsoDTO[] BuscaClientesAvulsosValorAtivo()
		{
			return this.Get<ClienteAvulsoDTO[]>(new Uri($"{ServicoRotas.Base}{ServicoRotas.ClienteAvulso.GetClientesAvulsosValorAtivo}"));
		}
	}
}
