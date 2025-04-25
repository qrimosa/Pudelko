using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace PudelkoLib;

public enum UnitsOfMeasure
{
    milimeter,
    centimetr,
    meter
}

public sealed class Pudelko : IFormattable, IEquatable<Pudelko>, IEnumerable<double>
{
    private readonly double a;
    private readonly double b;
    private readonly double c;

    //ZADANIE 3
    public double A => Math.Round(a,3);
    public double B => Math.Round(b,3);
    public double C => Math.Round(c,3);

    //ZADANIE 2
    public Pudelko(double? a = null, double? b = null, double? c = null, UnitsOfMeasure? unit = UnitsOfMeasure.meter)
    {
        double ConvertToMeters(double val, UnitsOfMeasure? u) => u switch
        {
            UnitsOfMeasure.milimeter => Math.Round(val / 1000, 3),
            UnitsOfMeasure.centimetr => Math.Round(val / 100, 3),
            UnitsOfMeasure.meter => Math.Round(val, 3),
            _ => throw new ArgumentOutOfRangeException(nameof(unit), "Invalid unit")
        };

        double defaultVal = unit switch
        {
            UnitsOfMeasure.milimeter => 100,
            UnitsOfMeasure.centimetr => 10,
            UnitsOfMeasure.meter => 0.1,
            _ => throw new ArgumentOutOfRangeException(nameof(unit), "Invalid unit")
        };

        // Konwertuj do metrów najpierw!
        double valA = ConvertToMeters(a ?? defaultVal, unit);
        double valB = ConvertToMeters(b ?? defaultVal, unit);
        double valC = ConvertToMeters(c ?? defaultVal, unit);

        if (valA <= 0 || valB <= 0 || valC <= 0 || valA > 10 || valB > 10 || valC > 10)
            throw new ArgumentOutOfRangeException("Dimensions must be in range [0, 10] meters");

        this.a = valA;
        this.b = valB;
        this.c = valC;
    }

    //ZADANIE 4
    public override string ToString()
    {
        return ToString("m", CultureInfo.CurrentCulture);
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        format ??= "m";
        format = format.Trim().ToLower();   
        switch (format)
        {
            case "m":
                return $"{A.ToString("F3")} m × {B.ToString("F3")} m × {C.ToString("F3")} m";
            case "cm":
                return $"{(A * 100).ToString("F1")} cm × {(B * 100).ToString("F1")} cm × {(C * 100).ToString("F1")} cm";
            case "mm":
                return $"{Math.Round(A * 1000)} mm × {Math.Round(B * 1000)} mm × {Math.Round(C * 1000)} mm";
            default:
                throw new FormatException($"The '{format}' format string is not supported.");
        }
    }

    //ZADANIE 5
    public double Objetosc()
    {
        return Math.Round(a * b * c, 9);
    }

    //ZADANIE 6
    public double Pole()
    {
        return Math.Round(2 * (a * b + a * c + b * c), 6);
    }

    //ZADANIE 7
    public bool Equals(Pudelko other)
    {
        if (other is null)
            return false;

        double[] dims1 = new[] { Math.Round(A, 3), Math.Round(B, 3), Math.Round(C, 3) }.OrderBy(x => x).ToArray();
        double[] dims2 = new[] { Math.Round(other.A, 3), Math.Round(other.B, 3), Math.Round(other.C, 3) }.OrderBy(x => x).ToArray();

        return dims1.SequenceEqual(dims2);
    }

    public override bool Equals(object obj)
    {
        if (obj is Pudelko other)
            return Equals(other);
        return false;
    }

    public override int GetHashCode()
    {
        var dims = new[] { Math.Round(A, 3), Math.Round(B, 3), Math.Round(C, 3) }.OrderBy(x => x).ToArray();

        return HashCode.Combine(dims[0], dims[1], dims[2]);
    }

    public static bool operator ==(Pudelko p1, Pudelko p2)
    {
        if (ReferenceEquals(p1, p2))
            return true;
        if (p1 is null || p2 is null)
            return false;
        return p1.Equals(p2);
    }

    public static bool operator !=(Pudelko p1, Pudelko p2)
    {
        return !(p1 == p2);
    }

    //ZADANIE 8
    public static Pudelko operator +(Pudelko p1, Pudelko p2)
    {
        if (p1 is null || p2 is null)
            throw new ArgumentNullException("Cannot combine null boxes.");

        var dims1 = new[] { p1.A, p1.B, p1.C }.OrderBy(x => x).ToArray();
        var dims2 = new[] { p2.A, p2.B, p2.C }.OrderBy(x => x).ToArray();

        double newA = Math.Max(dims1[0], dims2[0]);
        double newB = Math.Max(dims1[1], dims2[1]);
        double newC = dims1[2] + dims2[2];

        return new Pudelko(newA, newB, newC);
    }

    //ZADANIE 9
     public static explicit operator double[](Pudelko p)
    {
        return new[]{ p.A, p.B, p.C };
    }
    public static implicit operator Pudelko((int a, int b, int c) dimensions) =>
    new(dimensions.a / 1000.0, dimensions.b / 1000.0, dimensions.c / 1000.0, UnitsOfMeasure.meter);

    //ZADANIE 10
    public double this[int index]
    {
        get
        {
            return index switch
            {
                0 => A,
                1 => B,
                2 => C,
                _ => throw new IndexOutOfRangeException("Index must be 0, 1, or 2.")
            };
        }
    }

    //ZADANIE 11
    public IEnumerator<double> GetEnumerator()
    {
        yield return A;
        yield return B;
        yield return C;
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    //ZADANIE 12
    public static Pudelko Parse(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentNullException(nameof(input));

        string[] parts = input.Split('×', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 3)
            throw new FormatException("Invalid box format.");

        double[] dimensions = new double[3];
        UnitsOfMeasure? unit = null;

        for (int i = 0; i < 3; i++)
        {
            string part = parts[i].Trim();
            string[] tokens = part.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (tokens.Length != 2)
                throw new FormatException($"Invalid format for dimension {i}.");

            if (!double.TryParse(tokens[0], System.Globalization.CultureInfo.InvariantCulture, out double value))
                throw new FormatException($"Invalid number format for dimension {i}.");

            string unitStr = tokens[1];

            UnitsOfMeasure currentUnit = unitStr switch
            {
                "m" => UnitsOfMeasure.meter,
                "cm" => UnitsOfMeasure.centimetr,
                "mm" => UnitsOfMeasure.milimeter,
                _ => throw new FormatException($"Invalid unit '{unitStr}'")
            };

            unit ??= currentUnit;

            if (currentUnit != unit)
                throw new FormatException("All units must match.");

            dimensions[i] = value;
        }

        return new Pudelko(dimensions[0], dimensions[1], dimensions[2], unit.Value);
    }
}