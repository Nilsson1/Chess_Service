using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace HelloService
{
    [ServiceContract(CallbackContract = typeof(IHelloServiceCallback))]
    public interface IHelloService
    {

        [OperationContract(IsOneWay =true)]
        void GetMessage();
        

        [OperationContract]
        Guid Subscribe();

        [OperationContract(IsOneWay = true)]
        void BroadCast(Guid clientId, string message);
    }

    // This is the callback contract
    public interface IHelloServiceCallback
    {
        // Since we have not set IsOnway=true, the operation is Request/Reply operation
        [OperationContract(IsOneWay =true)]
        void Progress(int percentageComplete);

        [OperationContract(IsOneWay = true)]
        void HandleMessage(string message);

    }
}
