using BHJet_CoreGlobal.GoogleUtil.Model;
using System;
using System.IO;
using System.Linq;
using System.Net;

namespace BHJet_CoreGlobal.GoogleUtil
{
    public class GoogleApiUtil
    {
        private static string ApiKey { get; set; }

        public GoogleApiUtil(string GoogleApiKey)
        {
            ApiKey = GoogleApiKey;
        }

        private T GoogleRequest<T>(string requestURL)
        {
            WebRequest request = WebRequest.Create($@"https://maps.googleapis.com/maps/api/{requestURL}&key={ApiKey}");

            using (WebResponse response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    var txto = new StreamReader(stream);
                    var conteudo = txto.ReadToEnd();

                    var google = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(conteudo);

                    return google;
                }
            }
        }

        public double? BuscaDistanciaMatrix(GeoLocalizacaoMatrixModel filtro)
        {
            double? distanciaKM = 0;
            //string origemChave = $"{filtro.Origem.Latitude.ToString().Replace(",", ".")},{filtro.Origem.Longitude.ToString().Replace(",", ".")}";
            //string destinosChave = string.Join("|", filtro.Destinos.Select(c => c.Latitude.ToString().Replace(",", ".") + "," + c.Longitude.ToString().Replace(",", ".")));
            //string request = $"distancematrix/json?units=imperial&origins={origemChave}&destinations={destinosChave}";
            string origemChave = $"{filtro.Origem.Latitude.ToString().Replace(",", ".")},{filtro.Origem.Longitude.ToString().Replace(",", ".")}";
            string destinosChave = $"{filtro.Destino.Latitude.ToString().Replace(",", ".")},{filtro.Destino.Longitude.ToString().Replace(",", ".")}";
            string request = $"distancematrix/json?units=imperial&origins={origemChave}&destinations={destinosChave}";

            var googleDistance = GoogleRequest<DistanceMatrixModel>(request);

            if (googleDistance.status == "OK")
            {
                foreach (Element coord in googleDistance.rows.FirstOrDefault().elements)
                {
                    double distanciaM = coord.distance.value / 100;
                    distanciaKM += Math.Round(distanciaM, 2) / 10;
                }
            }

            return distanciaKM;
        }
    }
}
