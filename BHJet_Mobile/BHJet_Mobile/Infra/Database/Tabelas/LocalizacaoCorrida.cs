using SQLite;

namespace BHJet_Mobile.Infra.Database.Tabelas
{
    [Table("BHJetLocalizacaoCorrida")]
    public class LocalizacaoCorrida
    {
        [PrimaryKey]
        [AutoIncrement]
        public long IDRegistros { get; set; }

        [NotNull]
        public long IDCorrida { get; set; }

        [NotNull]
        public double Latitude { get; set; }

        [NotNull]
        public double Longitude { get; set; }
    }
}
