using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Exceptions
{
    internal class BrunTypeErrorException : Exception
    {
        public BrunTypeErrorException(string message) : base(message)
        {

        }
    }
}
