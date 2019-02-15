using BHJet_Mobile.Infra;
using BHJet_Mobile.Infra.Variaveis;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BHJet_Servico
{
    public abstract class ServicoBase
    {
        public ServicoBase()
        {

        }

        /// <summary>
        /// Token que será utilizado em todas as requisições
        /// </summary>
        public static string AuthorizationToken { get { return GlobalVariablesManager.GetApplicationCurrentProperty(GlobalVariablesManager.VariaveisGlobais.Token).ToString().Replace("bearer ", ""); } }

        /// <summary>
        /// Base URL
        /// </summary>
        protected readonly string BaseUrl = ServicoRotas.Base;

        protected string serviceBaseUrl = "";

        protected async Task<string> Get(Uri url)
        {
            using (var client = await GetHttpClient())
            {
                var result = await client.GetAsync(url);
                return await GetResult(result);
            }
        }

        protected Uri ServiceUri
        {
            get
            {
                return new Uri(CombineUri(BaseUrl, serviceBaseUrl));
            }
        }

        protected async Task<T> Get<T>(Uri url)
        {
            var value = await Get(url);
            return JsonConvert.DeserializeObject<T>(value);
        }

        protected async Task<string> Delete(Uri url)
        {
            using (var client = await GetHttpClient())
            {
                var result = await client.DeleteAsync(url);
                return await GetResult(result);
            }
        }

        protected async Task<T> Delete<T>(Uri url)
        {
            var value = await Delete(url);
            return JsonConvert.DeserializeObject<T>(value);
        }

        protected async Task<string> Post(Uri url, string data)
        {
            using (var client = await GetHttpClient())
            {
                var result = await client.PostAsync(url, new StringContent(data, Encoding.UTF8, "application/json"));
                return await GetResult(result);
            }
        }

        protected async Task<string> Post<T>(Uri url, T data)
        {
            var value = JsonConvert.SerializeObject(data);
            return await Post(url, value);
        }

        protected async Task<TResult> Post<T, TResult>(Uri url, T data)
        {
            var value = await Post<T>(url, data);
            return JsonConvert.DeserializeObject<TResult>(value);
        }

        protected async Task<string> Put(Uri url, string data)
        {
            using (var client = await GetHttpClient())
            {
                var result = await client.PutAsync(url, new StringContent(data, Encoding.UTF8, "application/json"));
                return await GetResult(result);
            }
        }

        protected async Task<string> Put<T>(Uri url, T data)
        {
            var value = JsonConvert.SerializeObject(data);
            return await Put(url, value);
        }

        protected async Task<TResult> Put<T, TResult>(Uri url, T data)
        {
            var value = await Put<T>(url, data);
            return JsonConvert.DeserializeObject<TResult>(value);
        }

        public async Task<string> Upload(Uri url, byte[] paramFileBytes)
        {
            // Bytes arquivo
            HttpContent bytesContent = new ByteArrayContent(paramFileBytes);

            // Requisicao
            using (var client = await GetHttpClient())
            using (var formData = new MultipartFormDataContent())
            {
                // Adicionar arquivo na requisição
                formData.Add(bytesContent, "arquivo", "arquivo");

                // Post Imagem
                var response = await client.PostAsync(url, formData);

                // Strem
                return await GetResult(response);
            }
        }

        protected async Task<string> GetResult(HttpResponseMessage result)
        {
            //Valida o resultado
            await ValidateStatusCode(result);

            if (result.StatusCode == HttpStatusCode.NoContent)
                return "[]";

            return await result.Content.ReadAsStringAsync();
        }

        private async Task ValidateStatusCode(HttpResponseMessage response)
        {
            var statusClass = ((int)response.StatusCode) / 100;
            switch (statusClass)
            {
                case 2:
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.NoContent:
                            var msgResponseNC = DeserializaResponse(response, Mensagem.Erro.SemResultado);
                            throw new NoContentException(msgResponseNC);
                    }
                    break;
                case 4:
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.NotFound:
                            throw new NoContentException(Mensagem.Erro.SemResultado);
                        case HttpStatusCode.Unauthorized:
                            throw new UnauthorizedAccessException();
                        case HttpStatusCode.BadRequest:
                        case HttpStatusCode.NotAcceptable:
                            var msgResponse = DeserializaResponse(response, Mensagem.Erro.ErroPadrao);
                            throw new WarningException(msgResponse ?? Mensagem.Erro.ErroPadrao);
                        default:
                            throw new ErrorException(Mensagem.Erro.ErroPadrao);
                    }
                case 3:
                case 5:
                    throw new ErrorException(Mensagem.Erro.ErroPadrao);
            }
        }

        protected string DeserializaResponse(HttpResponseMessage response, string mensagemDefault)
        {
            var retorno = response.Content.ReadAsStringAsync().Result;
            try
            {
                if (retorno.Contains("Excecao"))
                    retorno = JsonConvert.DeserializeObject<ModelExcecao>(retorno).Excecao;
                else if (retorno.Contains("Message"))
                    retorno = JsonConvert.DeserializeObject<ModelMessage>(retorno).Message;
                else if (retorno.Contains("error_description"))
                    retorno = JsonConvert.DeserializeObject<ModelExcecaoError>(retorno).error_description;
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

        private async Task<HttpClient> GetHttpClient(bool authorize = true)
        {
            // Instancia novo HttpClient
            HttpClient client = new HttpClient();
            client.BaseAddress = this.ServiceUri;

            if (authorize && AuthorizationToken != null)
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

        private class ModelExcecaoError
        {
            public string error { get; set; }
            public string error_description { get; set; }
        }

    }
}
