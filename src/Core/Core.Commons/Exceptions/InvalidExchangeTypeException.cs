using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Commons.Exceptions
{
    public class InvalidExchangeTypeException : Exception
    {
        public InvalidExchangeTypeException() : base("Invalid exchange type.") { }
    }
}
