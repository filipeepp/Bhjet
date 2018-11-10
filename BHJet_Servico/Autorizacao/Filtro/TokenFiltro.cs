namespace BHJet_Servico.Autorizacao.Filtro
{
    public class TokenFiltro
    {
        public readonly string grant_type = "password";
        public string username { get; set; }
        public string password { get; set; }
    }
}
