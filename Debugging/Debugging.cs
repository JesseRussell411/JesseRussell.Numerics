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
            List<Fraction> fracBuilder = new List<Fraction>();
            Random rand = new Random(12345);
            for(int i = 0; i < 1000000; i++)
                fracBuilder.Add(rand.NextFraction(-15, 15, 15, 15));

            Fraction[] fracs = fracBuilder.ToArray();

            
            Stopwatch sw = new Stopwatch();
            long totalMs = 0;
            int count = 0;
            for (int i = 0; i < 20; i++)
            {
                sw.Restart();
                Console.WriteLine(fracs.Sum());
                sw.Stop();
                Console.WriteLine(sw.ElapsedMilliseconds);

                if (i > 6)
                {
                    count++;
                    totalMs += sw.ElapsedMilliseconds;
                }
            }
            Console.WriteLine((double)totalMs / count);


            // new: 54.857142857142854
            // new: 43.57142857142857
            // new: 68.28571428571429

            // old: 78.57142857142857
            // old: 89.57142857142857
            // old: 89.85714285714286


            Console.ReadLine();
            //Vector2 a = new JesseRussell.Numerics.WorkInProgress.Vector2(2, 1);
            //Vector2 xT = new Vector2(0, 1);
            //Vector2 yT = new Vector2(-1, 0);
            //var aT = a.Transform(xT, yT);

            //println(a);
            //println(xT);
            //println(yT);
            //println(aT);

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
