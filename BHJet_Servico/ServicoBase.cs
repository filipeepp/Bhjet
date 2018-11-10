using BHJet_Core.Variaveis;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace BHJet_Servico
{
    public abstract class ServicoBase
    {
        public ServicoBase()
        {
            ValidateResult = null;
        }

        /// <summary>
        /// Token que será utilizado em todas as requisições
        /// </summary>
        protected string AuthorizationToken
        {
            get
            {
                return "";
            }
        }

        /// <summary>
        /// Base URL
        /// </summary>
        protected readonly string BaseUrl = ServicoRotas.Base;

        protected string Get(Uri url)
        {
            using (var client = GetHttpClient())
            {
                var result = client.GetAsync(url).Result;
                return GetResult(result);
            }
        }

        protected Uri ServiceUri
        {
            get
            {
                return new Uri(CombineUri(BaseUrl, string.Empty));
            }
        }

        public static object JSchema { get; private set; }

        protected T Get<T>(Uri url)
        {
            var value = Get(url);
            return JsonConvert.DeserializeObject<T>(value);
        }

        protected Stream GetStream(Uri url)
        {
            using (var client = GetHttpClient())
            {
                // Resultado
                var result = client.GetAsync(url).Result;
                //Valida o resultado
                ValidateStatusCode(result);
                // Get Stream
                var stream = result.Content.ReadAsStreamAsync().Result;
                // Return
                return stream;
            }
        }

        protected string Delete(Uri url)
        {
            using (var client = GetHttpClient())
            {
                var result = client.DeleteAsync(url).Result;
                return GetResult(result);
            }
        }

        protected T Delete<T>(Uri url)
        {
            var value = Delete(url);
            return JsonConvert.DeserializeObject<T>(value);
        }

        protected string Post(Uri url, string data)
        {
            using (var client = GetHttpClient())
            {
                var result = client.PostAsync(url, new StringContent(data, Encoding.UTF8, "application/json")).Result;
                return GetResult(result);
            }
        }

        protected string Post<T>(Uri url, T data)
        {
            var value = JsonConvert.SerializeObject(data);
            return Post(url, value);
        }

        protected TResult Post<T, TResult>(Uri url, T data)
        {
            var value = Post<T>(url, data);
            return JsonConvert.DeserializeObject<TResult>(value);
        }

        protected string Put(Uri url, string data)
        {
            using (var client = GetHttpClient())
            {
                var result = client.PutAsync(url, new StringContent(data, Encoding.UTF8, "application/json")).Result;
                return GetResult(result);
            }
        }

        protected string Put<T>(Uri url, T data)
        {
            var value = JsonConvert.SerializeObject(data);
            return Put(url, value);
        }

        protected TResult Put<T, TResult>(Uri url, T data)
        {
            var value = Put<T>(url, data);
            return JsonConvert.DeserializeObject<TResult>(value);
        }

        protected string GetResult(HttpResponseMessage result)
        {
            //Valida o resultado
            ExecuteValidateStatusCode(result);
            // Return Content String
            return result.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Valida resultado do serviço
        /// Entrada: HttpResponseMessage
        /// Saida: Bool (true: validação base, false: não valida)
        /// </summary>
        public Func<HttpResponseMessage, bool> ValidateResult = null;

        private void ExecuteValidateStatusCode(HttpResponseMessage response)
        {
            if (ValidateResult != null)
            {
                if (!ValidateResult.Invoke(response))
                    return;
            }
            ValidateStatusCode(response);
        }

        private void ValidateStatusCode(HttpResponseMessage response)
        {
            var statusClass = ((int)response.StatusCode) / 100;
            switch (statusClass)
            {
                case 2:
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.NoContent:
                            var msgResponse = DeserializaResponse(response, Mensagem.Erro.SemResultado);
                            throw new Exception(msgResponse);
                    }
                    break;
                case 4:
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.NotFound:
                            throw new Exception(Mensagem.Erro.SemResultado);
                        case HttpStatusCode.Unauthorized:
                            throw new UnauthorizedAccessException();
                        case HttpStatusCode.BadRequest:
                        case HttpStatusCode.NotAcceptable:
                            var msgResponse = DeserializaResponse(response, Mensagem.Erro.ErroPadrao);
                            throw new Exception(msgResponse ?? Mensagem.Erro.ErroPadrao);
                        default:
                            throw new Exception(Mensagem.Erro.ErroPadrao);
                    }
                case 3:
                case 5:
                    throw new Exception(Mensagem.Erro.ErroPadrao);
            }
        }

        private string DeserializaResponse(HttpResponseMessage response, string mensagemDefault)
        {
            var retorno = response.Content.ReadAsStringAsync().Result;
            try
            {
                if (retorno.Contains("Excecao"))
                    retorno = JsonConvert.DeserializeObject<ModelExcecao>(retorno).Excecao;
                else if (retorno.Contains("Message"))
                    retorno = JsonConvert.DeserializeObject<ModelMessage>(retorno).Message;
                else if (!string.IsNullOrWhiteSpace(retorno))
                    retorno = retorno.Replace("\"", "");
                else
                    retorno = mensagemDefault;
            }
            catch
            {
                retorno = mensagemDefault;
            }
            return retorno;
        }

        private string CombineUri(string uriBase, params string[] uriParts)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(uriBase);

            var foundSlash = uriBase.EndsWith("/");
            for (int i = 0; i < uriParts.Length; i++)
            {
                if (!foundSlash)
                    sb.Append("/");

                sb.Append(uriParts[i]);
                foundSlash = uriParts[i].EndsWith("/");
            }
            return sb.ToString();
        }

        private HttpClient GetHttpClient(bool authorize = true)
        {
            // Instancia novo HttpClient
            var client = new HttpClient
            {
                BaseAddress = this.ServiceUri
            };

            if (authorize && !string.IsNullOrWhiteSpace(AuthorizationToken))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthorizationToken);

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        private class ModelMessage
        {
            public string Message { get; set; }
        }

        private class ModelExcecao
        {
            public string Excecao { get; set; }
            public string StackTrace { get; set; }
            public string Tipo { get; set; }
        }
    }
}
