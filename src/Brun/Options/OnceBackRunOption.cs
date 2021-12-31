using Brun.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun
{
    public class OnceBackRunOption : BackRunOption
    {
        public OnceBackRunOption() : this(null, null) { }
        public OnceBackRunOption(string id, string name) : base(id, name) { }
    }
}
