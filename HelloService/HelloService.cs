using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;

namespace HelloService
{

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, AddressFilterMode = AddressFilterMode.Any)]
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "HelloService" in both code and config file together.
    public class HelloService : IHelloService
    {
        private List<Client> _clients = new List<Client>();

        public List<Client> GetClients()
        {
            int clientCount = 0;
            foreach(Client c in _clients)
            {
                clientCount++;
            }
            System.Console.WriteLine(clientCount);
            return _clients;
        }

        int number;
        public int IncrementNumber()
        {
            System.Console.WriteLine("Session ID: " + OperationContext.Current.SessionId);
            number++;
            return number;
        }

        public void BroadCast(Guid clientId, string message)
        {
            ThreadPool.QueueUserWorkItem
            (
                delegate
                {
                    lock (_clients)
                    {
                        var clientGuids = new List<Guid>();

                        foreach (var client in _clients)
                        {
                            try
                            {
                                // if the client isn't the one whiich raised the 
                                // message and the message type matches, broadcast to that client
                                if (client.ClientId != clientId)
                                {
                                    client.Callback.HandleMessage(message);
                                }
                            }
                            catch (Exception)
                            {
                                clientGuids.Add(client.ClientId);
                            }
                        }

                        foreach (var clientGuid in clientGuids)
                        {
                            _clients.Remove(_clients.First(i => i.ClientId == clientGuid));
                        }
                    }
                }
            );
        }


        public Guid Subscribe()
        {
            var callback = OperationContext.Current.GetCallbackChannel<IHelloServiceCallback>();
            var clientId = Guid.NewGuid();

            if (callback == null) return Guid.Empty;

            lock (_clients)
            {
                _clients.Add(new Client
                {
                    ClientId = clientId,
                    Callback = callback,
                });
            }

            return clientId;
        }

        public void GetMessage()
        {
            for (int i = 1; i <= 100; i++)
            {
                // some logic to process the report
                Thread.Sleep(100);
                // Get the callback channel to send messages to the client
                OperationContext.Current.
                    GetCallbackChannel<IHelloServiceCallback>().Progress(i);
            }
        }
    }
}
