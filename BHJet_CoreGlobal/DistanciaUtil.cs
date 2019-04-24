using System;

namespace BHJet_CoreGlobal
{
    public static class DistanciaUtil
    {
        private static double GrausEmRadio(double graus)
        {
            return graus * Math.PI / 180.0;
        }

        public static double CalculaDistancia(Localidade localizacao1, Localidade localizacao2)
        {
            

            double circuferencia = 40000.0; // Circuferencia da terra
            double distancia = 0.0;

            // Calcula radio
            double latitude1Rad = GrausEmRadio(localizacao1.Latitude);
            double longitude1Rad = GrausEmRadio(localizacao1.Longitude);
            double latititude2Rad = GrausEmRadio(localizacao2.Latitude);
            double longitude2Rad = GrausEmRadio(localizacao2.Longitude);

            double logitudeDiff = Math.Abs(longitude1Rad - longitude2Rad);

            if (logitudeDiff > Math.PI)
            {
                logitudeDiff = 2.0 * Math.PI - logitudeDiff;
            }

            double anguloCalculado =
                Math.Acos(
                    Math.Sin(latititude2Rad) * Math.Sin(latitude1Rad) +
                    Math.Cos(latititude2Rad) * Math.Cos(latitude1Rad) * Math.Cos(logitudeDiff));

            distancia = circuferencia * anguloCalculado / (2.0 * Math.PI);

            return distancia;
        }

        public static double CalculaDistancia(params Localidade[] localidades)
        {
            double distanciaTotal = 0.0;

            for (int i = 0; i < localidades.Length - 1; i++)
            {
                Localidade atual = localidades[i];
                Localidade proxima = localidades[i + 1];

                distanciaTotal += CalculaDistancia(atual, proxima);
            }

            return distanciaTotal;
        }
    }

    public struct Localidade
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
