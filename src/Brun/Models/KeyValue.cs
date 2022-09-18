using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brun.Models
{
    public class KeyValue
    {
        public KeyValue(string key, string value)
        {
            Key = key;
            Value = value;
        }
        public string Key { get; set; }
        public string Value { get; set; }
    }
    public class ValueLabel
    {
        public ValueLabel(string value, string label)
        {
            Label = label;
            Value = value;
        }
        public string Label { get; set; }
        public string Value { get; set; }
    }
}
