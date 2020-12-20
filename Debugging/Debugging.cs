using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;

using JesseRussell.Numerics;
using JesseRussell.Numerics.WorkInProgress;


namespace Debugging
{
    class Debugging
    {
        static void print(object obj) => Console.Write(obj);
        static void println(object obj) => Console.WriteLine(obj);
        static void Main(string[] args)
        {
            Vector2 a = new JesseRussell.Numerics.WorkInProgress.Vector2(2, 1);
            Vector2 xT = new Vector2(0, 1);
            Vector2 yT = new Vector2(-1, 0);
            var aT = a.Transform(xT, yT);

            println(a);
            println(xT);
            println(yT);
            println(aT);

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
