using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Commons.Exceptions
{
    public class InvalidWildcardTypeException : Exception
    {
        public InvalidWildcardTypeException() : base("Invalid wildcard type. Plz use '*' or '#'.") { }
    }
}
