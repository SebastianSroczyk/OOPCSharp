using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameEngine
{
    [Serializable]
    internal sealed class TransitionOverloadException : Exception
    {
        internal TransitionOverloadException() { }

        internal TransitionOverloadException(string message) : base(message) { }

        internal TransitionOverloadException(string message, Exception innerException) : base(message, innerException) { }
    }
}
