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
            var bob = IntFloatFrac.Parse(".5");

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
