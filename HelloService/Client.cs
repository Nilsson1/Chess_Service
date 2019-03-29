using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloService
{
    public class Client
    {
        public Guid ClientId { get; set; }

        public IHelloServiceCallback Callback { get; set; }
    }
}
