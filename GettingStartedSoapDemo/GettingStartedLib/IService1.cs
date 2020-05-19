using System;
using System.ServiceModel;

namespace GettingStartedLib
{
    [ServiceContract(Namespace = "http://Microsoft.ServiceModel.Samples")]
    public interface IFridge
    {
        [OperationContract]
        int HowMuchFruit();
        [OperationContract]
        int AddFruit(int amountToAdd);
        [OperationContract]
        int GetFruit(int amountToGet);
    }
}