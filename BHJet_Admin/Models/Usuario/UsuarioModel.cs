﻿using BHJet_Core.Enum;
using BHJet_Core.Utilitario;
using System.ComponentModel.DataAnnotations;

namespace BHJet_Admin.Models.Usuario
{
    public class UsuariosModel
    {
        public UsuarioDetalheModel[] usuarios { get; set; }
        public UsuarioDetalheModel novo { get; set; }
    }

    public class UsuarioDetalheModel
    {
        public long ID { get; set; }

        [Required(ErrorMessage ="Email obrigatório.")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Selecione o tipo de usuário.")]
        public TipoUsuario TipoUser { get; set; }

        private int? _ClienteSelecionado;
        public int? ClienteSelecionado
        {
            get
            {
                return _ClienteSelecionado;
            }
            set
            {
                _ClienteSelecionado = value;
            }
        }

        public string SituacaoDesc { get; set; }

        private string _senha;
        [Required(ErrorMessage = "Senha obrigatória.")]
        public string Senha
        {
            get
            {
                return _senha;
            }
            set
            {
                _senha = CriptografiaUtil.Criptografa(value, "ch4v3S3m2nt3BHJ0e1tA9u4t4hu1s33r") ;
            }
        }

        public bool Situacao { get; set; }
    }
}