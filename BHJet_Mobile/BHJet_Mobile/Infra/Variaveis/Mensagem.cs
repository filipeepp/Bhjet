namespace BHJet_Mobile.Infra.Variaveis
{
    public static class Mensagem
    {
        public static class Validacao
        {
            public static readonly string UsuarioNaoEncontrato = "Usuário e/ou senha inválidos. Tente novamente.";
            public static readonly string UsuarioSemPermissao = "Usuário sem permissão de acesso a está aplicação.";
            public static readonly string TipoProfissionalInvalido = "Favor informar um tipo de profissional válido.";
            public static readonly string StatusCorridaInvalido = "Não foi possível identificar o status da corrida desejada.";
        }

        public static class Erro
        {
            public static readonly string ErroPadrao = "Sistema indiponível.";
            public static readonly string SemResultado = "Não foram encontrados resultados.";
        }
    }
}
