using System;

namespace BHJet_DTO.Cliente
{
    public class ClienteAvulsoDTO
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Celular { get; set; }
        public string Comercial { get; set; }
        public string Senha { get; set; }
        public string NomeCartaoCredito { get; set; }
        public string NumeroCartaoCredito { get; set; }
        public string ValidadeCartaoCredito { get; set; }
        public string CPF { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Sexo { get; set; }
        public string CEP { get; set; }
        public string Rua { get; set; }
        public long Numero { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Pais { get; set; }
    }
}
