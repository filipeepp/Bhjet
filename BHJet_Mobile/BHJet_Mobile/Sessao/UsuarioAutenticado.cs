using BHJet_Core.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace BHJet_Mobile.Sessao
{
    public sealed class UsuarioAutenticado<T> where T : class, new()
    {
        private static T instance;

        public static T Instance()
        {
            lock (typeof(T))
                if (instance == null) instance = new T();

            return instance;
        }

        public TipoProfissional Tipo { get; set; }

        public TipoContrato Contrato { get; set; }

        public string Token { get; set; }

        public string Nome { get; set; }
    }
}
