using System;
using System.Collections.Generic;
using PudelkoLib;

namespace PudelkoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var pudelka = new List<Pudelko>
            {
                new Pudelko(2.5, 9.321, 0.1),
                new Pudelko(500, 600, 700, UnitOfMeasure.milimeter),
                new Pudelko(0.2, 0.3, 0.4),
                (200, 300, 400)
            };

            Console.WriteLine("Pudełka:");
            foreach (var p in pudelka)
                Console.WriteLine(p);

            pudelka.Sort((p1, p2) =>
            {
                int cmp = p1.Objetosc.CompareTo(p2.Objetosc);
                if (cmp != 0) return cmp;
                cmp = p1.Pole.CompareTo(p2.Pole);
                if (cmp != 0) return cmp;
                return (p1.A + p1.B + p1.C).CompareTo(p2.A + p2.B + p2.C);
            });

            Console.WriteLine("\nPosortowane:");
            foreach (var p in pudelka)
                Console.WriteLine(p);
        }
    }
}
