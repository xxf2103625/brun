using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BrunWebTest
{
    public class TestScopeService : ITestScopeService
    {
        private Guid Guid;
        public TestScopeService()
        {
            Guid = Guid.NewGuid();
        }
        public string Todo()
        {
            Console.WriteLine(Guid.ToString());
            return Guid.ToString();
        }
    }
}
