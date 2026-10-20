using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabAws.Infrastructure.Exceptions
{
    public class InfrastructureException : Exception
    {
        public InfrastructureException(string message, Exception? innerException = null) : base(message, innerException) { }
    }
}
