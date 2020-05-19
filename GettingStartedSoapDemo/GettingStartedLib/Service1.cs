using System;
using System.ServiceModel;

namespace GettingStartedLib
{
    public class Refrigerator : IFridge
    {
        private int _fruitCount;

        public Refrigerator() { _fruitCount = 10; }
        public int AddFruit(int amountToAdd)
        {
            _fruitCount += amountToAdd;

            Console.WriteLine("{0} peices of fruit added", amountToAdd);

            return _fruitCount;
        }

        public int GetFruit(int amountToGet)
        {
            _fruitCount -= amountToGet;

            Console.WriteLine("{0} peices of fruit taken", amountToGet);

            return _fruitCount;
        }

        public int HowMuchFruit()
        {
            Console.WriteLine("Count Requested");

            return _fruitCount;
        }
    }
}