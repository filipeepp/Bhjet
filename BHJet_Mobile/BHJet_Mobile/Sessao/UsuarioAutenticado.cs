using BHJet_Enumeradores;

namespace BHJet_Mobile.Sessao
{
    public interface IUsuarioAutenticado
    {
        TipoProfissional Tipo { get; set; }
        TipoContrato Contrato { get; set; }
        string Nome { get; set; }
        long? IDDiariaAtendimento { get; set; }
    }

    public sealed class UsuarioAutenticado : IUsuarioAutenticado
    {
        private static UsuarioAutenticado instance;

        private UsuarioAutenticado() { }

        public static UsuarioAutenticado Instance
        {
            get
            {
                if (instance == null)
                    lock (typeof(UsuarioAutenticado))
                        if (instance == null) instance = new UsuarioAutenticado();

                return instance;
            }
        }

        public TipoProfissional Tipo { get; set; }

        public TipoContrato Contrato { get; set; }

        public string Nome { get; set; }

        public bool StatusAplicatico { get; set; }

        public long? IDCorridaAtendimento { get; set; }

        public long? IDDiariaAtendimento { get; set; }
    }
}
