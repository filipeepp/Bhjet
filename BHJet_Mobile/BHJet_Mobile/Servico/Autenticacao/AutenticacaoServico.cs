using BHJet_CoreGlobal;
using BHJet_Mobile.Infra;
using BHJet_Mobile.Infra.Variaveis;
using BHJet_Mobile.Servico.Autenticacao.Model;
using BHJet_Servico;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace BHJet_Mobile.Servico.Autenticacao
{
    public interface IAutenticacaoServico
    {
        TokenModel Autenticar(string usuario, string senha);
    }

    public class AutenticacaoServico : ServicoBase, IAutenticacaoServico
    {

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
                    new KeyValuePair<string, string>("Modulo", "Mobile"),
                    new KeyValuePair<string, string>("area", "2"),
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
                        // Deserialize usuario autentidado
                        var autenticado = JsonConvert.DeserializeObject<TokenModel>(conteudo.ReadAsStringAsync().Result);
                        // Sessao
                        GlobalVariablesManager.SetApplicationCurrentProperty(GlobalVariablesManager.VariaveisGlobais.Token, autenticado.access_token);
                        // Return model
                        return autenticado;
                    case System.Net.HttpStatusCode.Unauthorized:
                        throw new UnauthorizedAccessException();
                    default:
                        var teste = DeserializaResponse(resposta, Mensagem.Erro.ErroPadrao);
                        throw new UnauthorizedAccessException(teste);
                }
            }
        }
    }
}
