﻿using System;
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
        static string readln() => Console.ReadLine();
        static void Main(string[] args)
        {
            Fraction f = Fraction.FromDecimal(1.00M);
            println(f * (Fraction)0.5);
            readln();
        }
    }
}
