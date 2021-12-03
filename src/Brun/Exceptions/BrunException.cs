using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Exceptions
{
    /// <summary>
    /// Brun异常信息
    /// </summary>
    internal class BrunException : Exception
    {
        public BrunException(BrunErrorCode errorCode, string message) : base(message)
        {

        }
    }
}
