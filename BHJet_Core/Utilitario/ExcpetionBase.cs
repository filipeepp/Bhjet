using System;

namespace BHJet_Core.Utilitario
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

}
