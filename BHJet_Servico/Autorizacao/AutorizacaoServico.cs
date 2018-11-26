using BHJet_Core.Utilitario;
using BHJet_Core.Variaveis;
using BHJet_DTO.Autenticacao;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace BHJet_Servico.Autorizacao
{
    public interface IAutorizacaoServico
    {
        TokenModel Autenticar(string usuario, string senha);
    }

    public class AutorizacaoServico : ServicoBase, IAutorizacaoServico
    {
        public AutorizacaoServico(string token) : base(token)
        {

        }

        public TokenModel Autenticar(string usuario, string senha)
        {
            // HttpClient
            using (var httpClient = new HttpClient()
            {
                BaseAddress = new Uri(ServicoRotas.Base)
            })
            {
                // Parametros
                var parametros = new KeyValuePair<string, string>[]
                {
                    new KeyValuePair<string, string>("username", usuario),
                    new KeyValuePair<string, string>("password",  CriptografiaUtil.Criptografa(senha, "ch4v3S3m2nt3BHJ0e1tA9u4t4hu1s33r")),
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("Modulo", "Web"),
                    //new KeyValuePair<string, string>("Perfil", ((int)perfil).ToString()),
                };

                // Requisiçao
                var requisicao = new HttpRequestMessage(HttpMethod.Post, ServicoRotas.Autenticacao.PostAutenticar)
                {
                    Content = new FormUrlEncodedContent(parametros)
                };

                // Resposta
                var resposta = httpClient.SendAsync(requisicao).Result;

                // Content
                var conteudo = resposta.Content;

                // Validação
                switch (resposta.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        return JsonConvert.DeserializeObject<TokenModel>(conteudo.ReadAsStringAsync().Result);
                    case System.Net.HttpStatusCode.Unauthorized:
                        throw new UnauthorizedAccessException();
                    default:
                        var teste = DeserializaResponse(resposta, Mensagem.Erro.ErroPadrao); 
                        throw new UnauthorizedAccessException(teste);
                }
            }
            //return this.Post<TokenFiltro, TokenModel>(new Uri(ServicoRotas.Base + ServicoRotas.Autenticacao.PostAutenticar), new TokenFiltro()
            //{
            //    username = usuario,
            //    password = CriptografiaUtil.Criptografa(senha, "ch4v3S3m2nt3BHJ0e1tA9u4t4hu1s33r")
            //});
        }



    }
}
