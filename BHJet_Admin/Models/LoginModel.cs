using BHJet_Enumeradores;
using System;
using System.ComponentModel.DataAnnotations;

namespace BHJet_Admin.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Usuário obrigatório.")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Senha obrigatória.")]
        public string Senha { get; set; }

    }

    public class RegistrarUsuarioModel
    {
        [Required(ErrorMessage = "Nome obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "E-mail obrigatório.")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefone Celular obrigatório.")]
        [RegularExpression(@"\([0-9]{2}\)[\s][0-9]{4}-[0-9]{4,5}", ErrorMessage = "Formato de Celular inválido")]
        public string Celular { get; set; }

        [Required(ErrorMessage = "Telefone comeciral obrigatório.")]
        [RegularExpression(@"\([0-9]{2}\)[\s][0-9]{4}-[0-9]{4,5}", ErrorMessage = "Formato de Telefone inválido")]
        public string Comercial { get; set; }

        [Required(ErrorMessage = "Senha obrigatório.")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "Confirmação obrigatório.")]
        public string SenhaConfirm { get; set; }

        [Required(ErrorMessage = "Nome Cartao Crédito obrigatório.")]
        [StringLength(50, ErrorMessage = "Formato de Nome do cartão inválido.", MinimumLength = 5)]
        public string NomeCartaoCredito { get; set; }

        [Required(ErrorMessage = "Nímero Cartao Crédito obrigatório.")]
        [StringLength(19, ErrorMessage = "Formato de Número do cartão inválido.", MinimumLength = 19)]
        public string NumeroCartaoCredito { get; set; }

        [Required(ErrorMessage = "Validade Cartao Crédito obrigatório.")]
        [StringLength(7, ErrorMessage = "Formato da válidade do cartão inválido.", MinimumLength = 7)]
        public string ValidadeCartaoCredito { get; set; }

        [Required(ErrorMessage = "CPF obrigatório.")]
        [StringLength(14, ErrorMessage = "Formato de CPF inválido.", MinimumLength = 14)]
        [RegularExpression(@"([0-9]{2}[\.]?[0-9]{3}[\.]?[0-9]{3}[\/]?[0-9]{4}[-]?[0-9]{2})|([0-9]{3}[\.]?[0-9]{3}[\.]?[0-9]{3}[-]?[0-9]{2})", ErrorMessage = "Formato de CPF/CNPJ inválido.")]
        public string CPF { get; set; }

        [Required(ErrorMessage = "Data Nascimento obrigatório.")]
        public DateTime? DataNascimento { get; set; }

        [Required(ErrorMessage = "Sexo obrigatório.")]
        public Sexo Sexo { get; set; }

        [Required(ErrorMessage = "CEP obrigatório.")]
        public string CEP { get; set; }

        [Required(ErrorMessage = "Rua obrigatória.")]
        public string Rua { get; set; }

        [Required(ErrorMessage = "Número obrigatório.")]
        public long? Numero { get; set; }

        [Required(ErrorMessage = "Bairro obrigatório.")]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "Cidade obrigatório.")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "Estado obrigatório.")]
        public UF Estado { get; set; }

        [Required(ErrorMessage = "País obrigatório.")]
        public string Pais { get; set; }
    }

    public class EsqueciMinhaSenhaModel
    {
        [Required(ErrorMessage = "E-mail obrigatório.")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        public string Email { get; set; }
    }
}