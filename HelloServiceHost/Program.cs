using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloServiceHost
{
    public class Program
    {
        static void Main(string[] args)
        {
            
            using (System.ServiceModel.ServiceHost host = new
                System.ServiceModel.ServiceHost(typeof(HelloService.HelloService)))
            {
                host.Open();
                Console.WriteLine("Host started @ " + DateTime.Now.ToString());
                Console.ReadLine();
            }
        }
    }
}
