using System;
using JesseRussell.Numerics;

namespace Debugging
{
    class Debugging
    {
        static void Main(string[] args)
        {
            IntFloat bob = default;
            Console.WriteLine(bob);
            bob = 9;
            Console.WriteLine(bob);
            bob = 9.0;
            Console.WriteLine(bob);
            bob = 34e200;
            Console.WriteLine(bob);
            bob = (float)34e100;
            Console.WriteLine(bob);
        }
    }
}
