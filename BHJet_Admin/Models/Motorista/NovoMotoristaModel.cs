using BHJet_CoreGlobal;
using BHJet_Enumeradores;
using System;
using System.ComponentModel.DataAnnotations;

namespace BHJet_Admin.Models.Motorista
{
    [Serializable]
    public class NovoMotoristaModel
    {
        public long ID { get; set; }

        private bool _EdicaoCadastro;
        public bool EdicaoCadastro { get => _EdicaoCadastro; set => _EdicaoCadastro = value; }

        [Required(ErrorMessage = "Nome Completo obrigatório.")]
        public string NomeCompleto { get; set; }

        [Required(ErrorMessage = "Documento RG obrigatório.")]
        public string DocumentoRG { get; set; }

        [Required(ErrorMessage = "CPF/CNPJ Completo obrigatório.")]
        [StringLength(18, ErrorMessage = "Formato de CPF/CNPJ inválido.", MinimumLength = 14)]
        [RegularExpression(@"([0-9]{2}[\.]?[0-9]{3}[\.]?[0-9]{3}[\/]?[0-9]{4}[-]?[0-9]{2})|([0-9]{3}[\.]?[0-9]{3}[\.]?[0-9]{3}[-]?[0-9]{2})", ErrorMessage = "Formato de CPF/CNPJ inválido.")]
        public string CpfCnpj { get; set; }

        [Required(ErrorMessage = "Número CNH obrigatório.")]
        public string CNH { get; set; }

        [Required(ErrorMessage = "Tipo de carteira obrigatório.")]
        public TipoCarteira TipoCarteiraMotorista { get; set; }

        #region Endereco
        [Required(ErrorMessage = "CEP obrigatório.")]
        [RegularExpression(@"[0-9]{5}-[0-9]{3}", ErrorMessage = "Formato de CEP inválido.")]
        public string Cep { get; set; }

        [Required(ErrorMessage = "Rua obrigatório.")]
        public string Rua { get; set; }

        [Required(ErrorMessage = "Número da rua obrigatório.")]
        public string RuaNumero { get; set; }

        public string Complemento { get; set; }

        [Required(ErrorMessage = "Bairro obrigatório.")]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "Cidade obrigatório.")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "Estado obrigatório.")]
        //[StringLength(2, ErrorMessage = "Preencher estado no formato: 'MG, SP, RJ'", MinimumLength = 2)]
        public string UF { get; set; }

        public bool EnderecoPrincipal { get; set; }

        public string PontoReferencia { get; set; }
        #endregion

        [RegularExpression(@"\([0-9]{2}\)[\s][0-9]{4}-[0-9]{4,5}", ErrorMessage = "Formato de Telefone inválido")]
        public string TelefoneResidencial { get; set; }

        [RegularExpression(@"\([0-9]{2}\)[\s][0-9]{4}-[0-9]{4,5}", ErrorMessage = "Formato de Celular inválido")]
        [Required(ErrorMessage = "Telefone celular obrigatório.")]
        public string TelefoneCelular { get; set; }

        public bool CelularWhatsapp { get; set; }

        [Required(ErrorMessage = "E-mail obrigatório.")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Tipo de contrato obrigatório.")]
        public RegimeContratacao TipoRegimeContratacao { get; set; }

        public string Observacao { get; set; }

        public string Senha
        {
            get; set;
        }

        public bool Situacao { get; set; }

        public NovoMotoristaComissaoModel[] Comissao { get; set; }
    }

    public class NovoMotoristaComissaoModel
    {
        public long? ID { get; set; }
        public string ValorComissao { get; set; }
        public string VigenciaInicio { get; set; }
        public string VigenciaFim { get; set; }
        public string Observacao { get; set; }
    }
}