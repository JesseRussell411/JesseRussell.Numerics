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
        static void print(object obj = null) => Console.Write(obj);
        static void println(object obj = null) => Console.WriteLine(obj);
        static void Main(string[] args)
        {
            var d = Doudec.Parse("4e5");
            var d2 = Doudec.Parse(".000001000000000");
            Console.WriteLine(d);
            Console.WriteLine(d2.IsDecimal);
            println(TextUtils.TrimNumberStringStart(",-0,,00,0654.5345000"));
         }
    }
}
