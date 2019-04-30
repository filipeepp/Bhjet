namespace BHJet_CoreGlobal.GoogleUtil.Model
{
    public class DistanceMatrixModel
    {
        public string[] destination_addresses { get; set; }
        public string[] origin_addresses { get; set; }
        public Row[] rows { get; set; }
        public string status { get; set; }
    }

    public class Element
    {
        public Distance distance { get; set; }
        public Duration duration { get; set; }
        public string status { get; set; }
    }

    public class Row
    {
        public Element[] elements { get; set; }
    }


    public class Distance
    {
        public string text { get; set; }
        public int value { get; set; }
    }

    public class Duration
    {
        public string text { get; set; }
        public int value { get; set; }
    }



    public class GeocoderLocation
    {
        public double Longitude { get; set; }

        public double Latitude { get; set; }
    }
}
