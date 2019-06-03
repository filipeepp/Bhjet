using BHJet_DTO.Usuario;
using System;

namespace BHJet_Servico.Usuario
{
    public interface IUsuarioServico
    {
        UsuarioDTO[] BuscaListaUsuarios(string trechoPesquisa);
        void CadastrarUsuario(UsuarioDTO model);
        void AtualizaSituacao(int situacao, long id);
        void AtualizaUsuario(UsuarioDTO model);
        void DeletaUsuario(long id);
        UsuarioDTO BuscaUsuario(long id);
        void RecuperaUsuario(string email);
    }

    public class UsuarioServico : ServicoBase, IUsuarioServico
    {
        public UsuarioServico(string token) : base(token)
        {

        }

        public UsuarioDTO[] BuscaListaUsuarios(string trechoPesquisa)
        {
            return this.Get<UsuarioDTO[]>(new Uri($"{ServicoRotas.Base}{ServicoRotas.Usuario.GetUsuarios}?trecho={trechoPesquisa}"));
        }

        public void CadastrarUsuario(UsuarioDTO model)
        {
            this.Post(new Uri($"{ServicoRotas.Base}{ServicoRotas.Usuario.PostUsuario}"), model);
        }

        public void AtualizaUsuario(UsuarioDTO model)
        {
            this.Put(new Uri($"{ServicoRotas.Base}{ServicoRotas.Usuario.PutUsuario}"), model);
        }

        public void AtualizaSituacao(int situacao, long id)
        {
            this.Put(new Uri($"{ServicoRotas.Base}{string.Format(ServicoRotas.Usuario.PutSituacao, situacao, id)}"), "");
        }

        public void DeletaUsuario(long id)
        {
            this.Delete(new Uri($"{ServicoRotas.Base}{string.Format(ServicoRotas.Usuario.DeleteUsuario, id)}"));
        }

        public UsuarioDTO BuscaUsuario(long id)
        {
            return this.Get<UsuarioDTO>(new Uri($"{ServicoRotas.Base}{string.Format(ServicoRotas.Usuario.GetUsuario, id)}"));
        }

        public void RecuperaUsuario(string email)
        {
            email = $"\"{email}\"";
            this.Post(new Uri($"{ServicoRotas.Base}{ServicoRotas.Usuario.PostRecuperaSenha}"), email);
        }
    }
}
