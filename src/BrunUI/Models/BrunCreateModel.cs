using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrunUI.Models
{
    public class BrunCreateModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string WorkerKey { get; set; }
        public string BrunType { get; set; }
    }
}
