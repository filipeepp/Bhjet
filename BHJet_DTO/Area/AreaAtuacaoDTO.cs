using System;

namespace BHJet_DTO.Area
{
    [Serializable]
    public class AreaAtuacaoDTO
    {
        public long ID { get; set; }
        public AreaAtuacaoGeoPosicao[] GeoVertices { get; set; }
        public bool Ativo { get; set; }
    } 

    [Serializable]
    public class AreaAtuacaoGeoPosicao
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
