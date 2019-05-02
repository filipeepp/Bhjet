namespace BHJet_CoreGlobal.GoogleUtil.Model
{
    public class GeoLocalizacaoModel
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }

    public class GeoLocalizacaoMatrixModel
    {
        public GeoLocalizacaoModel Origem { get; set; }
        public GeoLocalizacaoModel Destino { get; set; }
    }
}
