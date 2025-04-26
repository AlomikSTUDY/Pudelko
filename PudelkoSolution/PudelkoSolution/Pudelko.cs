using System;
using System.Collections;
using System.Globalization;

namespace PudelkoLib
{

    public sealed class Pudelko : IFormattable, IEquatable<Pudelko>, IEnumerable
    {
        private readonly double a;
        private readonly double b;
        private readonly double c;

        public double A => Math.Round(a, 3);
        public double B => Math.Round(b, 3);
        public double C => Math.Round(c, 3);

        public double Objetosc => Math.Round(A * B * C, 9);

        public double Pole => Math.Round(2 * (A * B + A * C + B * C), 6);

        public Pudelko(double a = 0.1, double b = 0.1, double c = 0.1, UnitOfMeasure unit = UnitOfMeasure.meter)
        {
            (this.a, this.b, this.c) = ConvertToMeters(a, b, c, unit);

            if (this.a <= 0 || this.b <= 0 || this.c <= 0 || this.a > 10 || this.b > 10 || this.c > 10)
                throw new ArgumentOutOfRangeException();
        }

        private (double, double, double) ConvertToMeters(double a, double b, double c, UnitOfMeasure unit)
        {
            double factor = unit switch
            {
                UnitOfMeasure.milimeter => 0.001,
                UnitOfMeasure.centimeter => 0.01,
                UnitOfMeasure.meter => 1,
                _ => throw new ArgumentException()
            };
            return (Math.Floor(a * 1000) / 1000 * factor, Math.Floor(b * 1000) / 1000 * factor, Math.Floor(c * 1000) / 1000 * factor);
        }

        public override string ToString()
        {
            return ToString("m", CultureInfo.CurrentCulture);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            format = format?.ToLower() ?? "m";
            string result = format switch
            {
                "m" => $"{A:F3} m × {B:F3} m × {C:F3} m",
                "cm" => $"{(A * 100):F1} cm × {(B * 100):F1} cm × {(C * 100):F1} cm",
                "mm" => $"{(int)(A * 1000)} mm × {(int)(B * 1000)} mm × {(int)(C * 1000)} mm",
                _ => throw new FormatException()
            };
            return result;
        }

        public static Pudelko Parse(string text)
        {
            var parts = text.Split('×', StringSplitOptions.TrimEntries);
            if (parts.Length != 3) throw new FormatException();

            double[] values = new double[3];
            UnitOfMeasure[] units = new UnitOfMeasure[3];

            for (int i = 0; i < 3; i++)
            {
                var tokens = parts[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                values[i] = double.Parse(tokens[0], CultureInfo.InvariantCulture);
                units[i] = tokens[1] switch
                {
                    "m" => UnitOfMeasure.meter,
                    "cm" => UnitOfMeasure.centimeter,
                    "mm" => UnitOfMeasure.milimeter,
                    _ => throw new FormatException()
                };
            }

            var (a, _, _) = new Pudelko().ConvertToMeters(values[0], values[1], values[2], units[0]);
            return new Pudelko(values[0], values[1], values[2], units[0]);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Pudelko);
        }

        public bool Equals(Pudelko other)
        {
            if (other == null) return false;

            double[] dims1 = { A, B, C };
            double[] dims2 = { other.A, other.B, other.C };
            Array.Sort(dims1);
            Array.Sort(dims2);

            for (int i = 0; i < 3; i++)
                if (Math.Abs(dims1[i] - dims2[i]) > 0.001) return false;

            return true;
        }

        public override int GetHashCode()
        {
            double[] dims = { A, B, C };
            Array.Sort(dims);
            return HashCode.Combine(dims[0], dims[1], dims[2]);
        }

        public static bool operator ==(Pudelko p1, Pudelko p2)
        {
            if (ReferenceEquals(p1, p2)) return true;
            if (p1 is null || p2 is null) return false;
            return p1.Equals(p2);
        }

        public static bool operator !=(Pudelko p1, Pudelko p2) => !(p1 == p2);

        public static Pudelko operator +(Pudelko p1, Pudelko p2)
        {
            double[] dims1 = { p1.A, p1.B, p1.C };
            double[] dims2 = { p2.A, p2.B, p2.C };

            Array.Sort(dims1);
            Array.Sort(dims2);

            double a = Math.Max(dims1[0], dims2[0]);
            double b = Math.Max(dims1[1], dims2[1]);
            double c = dims1[2] + dims2[2];

            return new Pudelko(a, b, c);
        }

        public static explicit operator double[](Pudelko p) => new double[] { p.A, p.B, p.C };

        public static implicit operator Pudelko((int a, int b, int c) dims) =>
            new Pudelko(dims.a, dims.b, dims.c, UnitOfMeasure.milimeter);

        public double this[int index]
        {
            get
            {
                return index switch
                {
                    0 => A,
                    1 => B,
                    2 => C,
                    _ => throw new IndexOutOfRangeException()
                };
            }
        }

        public IEnumerator GetEnumerator()
        {
            yield return A;
            yield return B;
            yield return C;
        }
    }
}
