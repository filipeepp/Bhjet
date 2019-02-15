using System;

namespace BHJet_Mobile.Infra
{
    public class SucessException : Exception
    {
        public SucessException()
        {

        }

        public SucessException(string name) : base(name)
        {

        }
    }

    public class WarningException : Exception
    {
        public WarningException()
        {

        }

        public WarningException(string name) : base(name)
        {

        }
    }

    public class ErrorException : Exception
    {
        public ErrorException()
        {

        }

        public ErrorException(string name) : base(name)
        {

        }
    }

    public class NoContentException : Exception
    {
        public NoContentException()
        {

        }

        public NoContentException(string name) : base(name)
        {

        }
    }

    public class DiariaException : Exception
    {
        public DiariaException()
        {

        }

        public DiariaException(string name) : base(name)
        {

        }
    }

    public class AvulsoException : Exception
    {
        public AvulsoException()
        {

        }

        public AvulsoException(string name) : base(name)
        {

        }
    }

    public class CorridaException : Exception
    {
        public CorridaException()
        {

        }

        public CorridaException(string name) : base(name)
        {

        }
    }
}
