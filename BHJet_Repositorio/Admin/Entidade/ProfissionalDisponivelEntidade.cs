namespace BHJet_Repositorio.Admin.Entidade
{
    public class ProfissionalDisponivelEntidade
    {
        public int idRegistro { get; set; }
        public int idColaboradorEmpresaSistema { get; set; }
        public int idTipoProfissional { get; set; }
        public string vcNomeCompleto { get; set; }
        public string vcLongitude { get; set; }
        public string vcLatitude { get; set; }
        public bool bitDisponivel { get; set; }
    }
}
