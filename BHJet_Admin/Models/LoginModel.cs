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

        [Required(ErrorMessage = "Email obrigatório.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefone Celular obrigatório.")]
        public string Celular { get; set; }

        [Required(ErrorMessage = "Telefone comeciral obrigatório.")]
        public string Comercial { get; set; }

        [Required(ErrorMessage = "Senha obrigatório.")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "Confirmação obrigatório.")]
        public string SenhaConfirm { get; set; }

        [Required(ErrorMessage = "Nome Cartao Crédito obrigatório.")]
        public string NomeCartaoCredito { get; set; }

        [Required(ErrorMessage = "Nímero Cartao Crédito obrigatório.")]
        public string NumeroCartaoCredito { get; set; }

        [Required(ErrorMessage = "Validade Cartao Crédito obrigatório.")]
        public string ValidadeCartaoCredito { get; set; }

        [Required(ErrorMessage = "CPF obrigatório.")]
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
        public string Numero { get; set; }

        [Required(ErrorMessage = "Bairro obrigatório.")]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "Cidade obrigatório.")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "Estado obrigatório.")]
        public string Estado { get; set; }

        [Required(ErrorMessage = "País obrigatório.")]
        public string Pais { get; set; }
    }

}