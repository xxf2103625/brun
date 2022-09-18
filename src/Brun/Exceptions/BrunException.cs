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
    public class BrunException : Exception
    {
        public BrunException(BrunErrorCode errorCode, string message) : base(message)
        {

        }
        public BrunException(BrunErrorCode errorCode, string message, params object[] args) : base(string.Format(message, args))
        {

        }
    }
}
