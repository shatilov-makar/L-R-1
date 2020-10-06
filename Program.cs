using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        const double eps = 0.01;
        const double dx = 10e-8;
        static bool isTrigonomerical;
        static double[] coefficient;
        static double[] edges;

        static void Main(string[] args)
        {
            СhooseTypeEquation();
            ChooseParameters();
            var f = isTrigonomerical ? new Func<double, double>((x) => coefficient[0] * Math.Cos(x) + coefficient[1] * x + coefficient[2])
                : new Func<double, double>((x) => coefficient[0] * Math.Pow(x, 3) + coefficient[1] * x + coefficient[2]);
            var res1 = HalveMethod(f, edges[0], edges[1]);
            var res2 = NewtonMethod(f, edges[0], edges[1]);
            Console.WriteLine("Результат, полученный методом деления отрезка: " + res1);
            Console.WriteLine("Результат, полученный методом Ньютона: " + res2);
            Console.ReadKey();
        }

     

        static double HalveMethod(Func<double, double> f, double i1, double i2)
        {
            double x0;
            do
            {
                x0 = (i1 + i2) / 2;
                var halfEdge = GetHalveEdge(f, i1, i2, x0);
                if (halfEdge == null)
                    throw new Exception("В заданном диапозоне корней нет");
                i1 = halfEdge[0];
                i2 = halfEdge[1];
            }
            while (Math.Abs(i1 - i2) > eps);
            return Math.Round(x0, 2);
        }

        static double[] GetHalveEdge(Func<double, double> f, double i1, double i2, double x0)
        {
            var d1 = f(i1);
            var d2 = f(x0);
            if (d1 * d2 <= 0)
                return new double[2] { i1, x0 };
            d1 = d2;
            d2 = f(i2);
            if (d1 * d2 <= 0)
                return new double[2] { x0, i2 };
            return null;
        }
       
 
       static double NewtonMethod(Func<double, double> f, double i1, double i2)
       {
            double x0;
            Func<double, double> secondDir; 
            if (isTrigonomerical)
                 secondDir = new Func<double, double>((x)=> -coefficient[0]*Math.Sin(x) + coefficient[1]);
            else 
                secondDir = new Func<double, double>((x) => 3 * coefficient[0] * Math.Pow(x, 2) + coefficient[1]);
            if (f(i1) * secondDir(i1) > 0)
                x0 = i1;
            if (f(i2) * secondDir(i2) > 0)
                x0 = i2;
            else throw new Exception("В заданном диапозоне корней нет");
            while (Math.Abs(f(x0)) >= eps)
            {
                var d = (f(x0 + dx) - f(x0)) / dx;
                x0 = Math.Round(x0 - (f(x0) / d), 2);
            }
            return x0;
        }

        private static void ChooseParameters()
        {
            while (true)
            {
                Console.WriteLine("Введите по порядку коэффициенты a, b и с");
                coefficient = GetUserInput();
                if (coefficient.Length == 3)
                {
                    bool flag = false;
                    while (true)
                    {
                        Console.WriteLine("Введите диапозон нахождения корня. Ширина диапозона должна быть не менее 0.01");
                        edges = GetUserInput();
                        if (edges.Length == 2 && Math.Round(Math.Abs(edges[0] - edges[1]), 3) >= eps)
                        {
                            flag = true;
                            break;
                        }
                        ShowError();
                    }
                    if (flag)
                        break;
                }
                ShowError();
            }
        }

        private static void СhooseTypeEquation()
        {
            while (true)
            {
                Console.WriteLine("Выберите 0 для выбора уровнения вида \n   acos(x) + bx + c = 0 \nили 1 для уравнения вида \n    ax^3 + bx + c = 0");
                var chose = GetUserInput();
                if (chose.Length > 0 && chose[0] == 0)
                {
                    isTrigonomerical = true;
                    break;
                }
                if (chose.Length > 0 && chose[0] == 1)
                {
                    isTrigonomerical = false;
                    break;
                }
                ShowError();
            }
        }
        static double[] GetUserInput()
        {
            return Console.ReadLine().Split(' ')
                           .Where(x => !string.IsNullOrWhiteSpace(x) && double.TryParse(x, out var d) )
                           .Select(x => double.Parse(x))
                           .ToArray();
        }
        static void ShowError()
        {
            Console.WriteLine("Ошибка! Следуйте командам");
            Console.WriteLine();
        }

    }
}
