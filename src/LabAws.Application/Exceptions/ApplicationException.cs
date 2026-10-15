using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabAws.Application.Exceptions
{
    public class ApplicationException : Exception
    {
        public ApplicationException(string Message) : base(Message) { }
    }


    public class ValidationBucketNameException : ApplicationException
    {
        public ValidationBucketNameException(string Message) : base(Message) { }
    }

}
