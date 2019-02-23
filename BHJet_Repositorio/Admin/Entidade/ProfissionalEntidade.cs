using BHJet_Enumeradores;
using System;

namespace BHJet_Repositorio.Admin.Entidade
{
    public class ProfissionalEntidade
    {
        public long ID { get; set; }
        public string NomeCompleto { get; set; }
        public TipoProfissional TipoProfissional { get; set; }
        public RegimeContratacao TipoRegime { get; set; }
    }
    
    public class ProfissionalCompletoEntidade
    {
        public long ID { get; set; }
        public long IDGestor { get; set; }
        public string NomeCompleto { get; set; }
        public string CPF { get; set; }
        public string CNH { get; set; }
        public TipoCarteira TipoCNH { get; set; }
        public string Cep { get; set; }
        public string Rua { get; set; }
        public string RuaNumero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        public string PontoReferencia { get; set; }
        public bool EnderecoPrincipal { get; set; }
        public string TelefoneResidencial { get; set; }
        public string TelefoneCelular { get; set; }
        public bool CelularWpp { get; set; }
        public string Email { get; set; }
        public bool ContratoCLT { get; set; }
        public string Observacao { get; set; }
        public string DocumentoRG { get; set; }
        public string Usuario { get; set; }
        public string Senha { get; set; }
        public bool StatusUsuario { get; set; }
        public TipoProfissional TipoProfissional { get; set; }
        public RegimeContratacao TipoRegime { get; set; }
        public ProfissionalComissaoEntidade[] Comissoes { get; set; }
    }

    public class ProfissionalComissaoEntidade
    {
        public long? idComissaoColaboradorEmpresaSistema { get; set; }
        public decimal decPercentualComissao { get; set; }
        public DateTime dtDataInicioVigencia { get; set; }
        public DateTime dtDataFimVigencia { get; set; }
        public string vcObservacoes { get; set; }
    }
}
