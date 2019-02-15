using SQLite;

namespace BHJet_Mobile.Infra.Database.Tabelas
{

    [Table("BHJetMotorista")]
    public class UsuarioLogado
    {
        [PrimaryKey]
        [AutoIncrement]
        public long? IDMotorista { get; set; }

        [NotNull]
        public string usuario { get; set; }

        [NotNull]
        public string senha { get; set; }
    }
}
