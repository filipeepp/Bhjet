using BHJet_Core.Utilitario;
using BHJet_Core.Variaveis;
using BHJet_CoreGlobal;
using BHJet_DTO.Autenticacao;
using BHJet_DTO.Usuario;
using BHJet_Servico.Autorizacao.Filtro;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace BHJet_Servico.Autorizacao
{
    public interface IAutorizacaoServico
    {
        TokenModel Autenticar(AutenticacaoFiltro filtro);
        UsuarioDTO BuscaPerfil(string token);
    }

    public class AutorizacaoServico : ServicoBase, IAutorizacaoServico
    {
        public AutorizacaoServico() : base()
        {

        }

        public AutorizacaoServico(string token) : base(token)
        {
            
        }

        public TokenModel Autenticar(AutenticacaoFiltro filtro)
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
                    new KeyValuePair<string, string>("username", filtro.usuario),
                    new KeyValuePair<string, string>("password",  CriptografiaUtil.Criptografa(filtro.senha, "ch4v3S3m2nt3BHJ0e1tA9u4t4hu1s33r")),
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("Modulo", "Web")
                    //new KeyValuePair<string, string>("area", ((int)filtro.area).ToString()),
                };

                // Requisiçao
                var requisicao = new HttpRequestMessage(HttpMethod.Post, ServicoRotas.Base.Replace("api/", "") + ServicoRotas.Autenticacao.PostAutenticar)
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
        }

        public UsuarioDTO BuscaPerfil(string token)
        {
            var atServicoTemp = new AutorizacaoServico(token);
            return atServicoTemp.Get<UsuarioDTO>(new Uri($"{ServicoRotas.Base}{ServicoRotas.Usuario.GetPerfil}"));
        }

    }
}
