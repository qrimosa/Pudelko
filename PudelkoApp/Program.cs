using System;
using System.Collections.Generic;
using PudelkoLib;


namespace PudelkoApp
{
    public static class PudelkoExtensions
    {
        public static Pudelko Kompresuj(this Pudelko pudelko)
        {
            double volume = pudelko.Objetosc();
            double side = Math.Round(Math.Pow(volume, 1.0 / 3.0), 3);
            return new Pudelko(side, side, side);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            List<Pudelko> pudelka = new List<Pudelko>
            {
                new Pudelko(2.5, 1.0, 0.5),
                new Pudelko(100, 300, 250, UnitsOfMeasure.milimeter),
                new Pudelko(10, 10, 10, UnitsOfMeasure.centimetr),
                new Pudelko(1.0, 1.0, 1.0),
                new Pudelko(0.1, 0.1, 0.1)
            };

            Console.WriteLine("Oryginalna lista pudełek:");
            foreach (var p in pudelka)
            {
                Console.WriteLine(p);
            }

            Console.WriteLine("\nLista po kompresji każdego pudełka:");
            foreach (var p in pudelka)
            {
                var compressed = p.Kompresuj();
                Console.WriteLine($"{p} => {compressed}");
            }

            pudelka.Sort(ComparePudelka);

            Console.WriteLine("\nPosortowana lista pudełek:");
            foreach (var p in pudelka)
            {
                Console.WriteLine($"{p} | V={p.Objetosc():F6} | S={p.Pole():F6} | A+B+C={p.A + p.B + p.C:F3}");
            }
        }

        public static int ComparePudelka(Pudelko p1, Pudelko p2)
        {
            int result = p1.Objetosc().CompareTo(p2.Objetosc());

            if (result == 0)
            {
                result = p1.Pole().CompareTo(p2.Pole());
            }

            if (result == 0)
            {
                result = (p1.A + p1.B + p1.C).CompareTo(p2.A + p2.B + p2.C);
            }

            return result;
        }
    }
}