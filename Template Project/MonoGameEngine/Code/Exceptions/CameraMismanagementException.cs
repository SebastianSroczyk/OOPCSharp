using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameEngine
{
    [Serializable]
    internal class CameraMismanagementException : Exception
    {
        internal CameraMismanagementException() { }

        internal CameraMismanagementException(string message) : base(message) { }

        internal CameraMismanagementException(string message, Exception innerException) : base(message, innerException) { }
    }
}
