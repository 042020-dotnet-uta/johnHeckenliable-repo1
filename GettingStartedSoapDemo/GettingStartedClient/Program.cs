using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GettingStartedClient.ServiceReference1;

namespace GettingStartedClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //Step 1: Create an instance of the WCF proxy.
            FridgeClient client = new FridgeClient();

            int fruit = client.HowMuchFruit();
            Console.WriteLine("There are {0} pieces of fruit to start", fruit);
            Console.ReadLine();


            fruit = client.AddFruit(5);
            Console.WriteLine("There are {0} pieces of fruit left after adding {1} pieces", fruit, 5);
            Console.ReadLine();

            fruit = client.HowMuchFruit();
            Console.WriteLine("There are now {0} pieces of fruit left", fruit);
            Console.ReadLine();

            fruit = client.GetFruit(2);
            Console.WriteLine("There are now {0} pieces of fruit left after getting {1} pieces", fruit, 2);
            Console.ReadLine();


            client.Close();
        }
    }
}