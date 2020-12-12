# JesseRussell.Numerics
A collection of useful number types. Uploaded as a NuGet package: https://www.nuget.org/packages/JesseRussell.Numerics/
All of these types are readonly structs with out-of-place operations. They have been designed with precision and safety taking priority over speed.

Fraction
  A fraction using BigInteger for the numerator and denominator
  
  FractionOperation
    Middle-man type to allow automatic simplification in operations like addition without simlplifying after every operation in a string of operations like 1/2 + 1/3 + 1/4. When performing operations using the overloaded operators (+ - * / etc) a FractionOperation is returned instead of a Fraction. This type can be implicitly cast back to a Fraction and is simplified when this happens. If the user prefers to handle simplification manually, they can use the instance methods instead (.Add, .Subtract, .Multiply, etc.), calling .Simplify() to simplify.

Doudec
  Combination double and decimal. Always tries to store as a decimal if possible.

IntFloat
  Combination BigInteger and Doudec. Always tries to store as a BigInteger if possible.

IntFloatFrac
  Combination IntFloat and Fraction. Always tries to store as a Fraction if possible.
  
