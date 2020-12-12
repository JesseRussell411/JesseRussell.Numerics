using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.Numerics;

using JesseRussell.Numerics;


namespace Debugging
{
    class Debugging
    {
        static void print(object obj) => Console.Write(obj);
        static void println(object obj) => Console.WriteLine(obj);
        static void Main(string[] args)
        {
            BigInteger big = BigInteger.Parse("23623465246245634563457356732572652456245763457645624624562472456246545725624563735624562463456244262626455624564");
            BigInteger _2 = 2;
            BigInteger _666 = 666;
            println(IntFloat.Pow(_2, _666));
            //IntFloatFrac steve = default;
            ////IntFloat bob = default;
            ////Console.WriteLine(bob);
            ////bob = 9;
            ////Console.WriteLine(bob);
            ////bob = 9.0;
            ////Console.WriteLine(bob);
            ////bob = 34e200;
            ////Console.WriteLine(bob);
            ////bob = (float)34e100;
            ////Console.WriteLine(bob);

            //int n = 100000000;

            //List<IntFloat> testList = new List<IntFloat>();
            //IntFloat bob = 0;
            //Stopwatch sw = new Stopwatch();
            //while (true)
            //{
            //    sw.Restart();

            //    for (int i = 0; i < n; ++i)
            //    {
            //        Doudec test = bob.Float;
            //        //testList.Add(bob);

            //    }
            //    sw.Stop();

            //    Console.WriteLine($"time: {sw.ElapsedMilliseconds} milliseconds");
            //}
            //Console.ReadLine();

            ////3gb - new
            ////9.1gb - old
        }
    }
}
