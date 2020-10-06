using System;
using System.Linq;

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
            var r1 = double.IsNaN(res1) ? "В заданном диапозоне корней нет" : "Результат, полученный методом деления отрезка: " + res1;
            Console.WriteLine(r1);
            var r2 = double.IsNaN(res2) ? "В заданном диапозоне корней нет" : "Результат, полученный методом Ньютона: " + res2;
            Console.WriteLine(r2);
            Console.ReadKey();
        }

        static double HalveMethod(Func<double, double> f, double e1, double e2)
        {
            double x;
            var y = double.MaxValue;
            while(true)
            {
                x = (e1 + e2) / 2;
                y = Math.Abs(f(x));
                if (Math.Abs(e2 - e1) < eps) 
                    return x;
                var halfEdge = GetHalveEdge(f, e1, e2, x);
                if (halfEdge == null)
                    return double.NaN;
                e1 = halfEdge[0];
                e2 = halfEdge[1];
            }
        }

        static double[] GetHalveEdge(Func<double, double> f, double i1, double i2, double x0)
        {
            var d1 = f(i1);
            var d2 = f(x0);
            if (d1 * d2 < 0)
                return new double[2] { i1, x0 };
            d1 = d2;
            d2 = f(i2);
            if (d1 * d2 < 0)
                return new double[2] { x0, i2 };
            return null;
        }
       
       static double NewtonMethod(Func<double, double> f, double e1, double e2)
       {
            double x0;
            Func<double, double> secondDir; 
            if (isTrigonomerical)
                 secondDir = new Func<double, double>((x)=> -coefficient[0]*Math.Sin(x) + coefficient[1]);
            else 
                secondDir = new Func<double, double>((x) => 3 * coefficient[0] * Math.Pow(x, 2) + coefficient[1]);
            if (f(e1) * secondDir(e1) > 0)
                x0 = e1;
            if (f(e2) * secondDir(e2) > 0)
                x0 = e2;
            else
                return double.NaN;
            while (Math.Abs(f(x0)) >= eps)
            {
                var d = (f(x0 + dx) - f(x0)) / dx;
                x0 = x0 - (f(x0) / d);
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
                        Console.WriteLine("Введите диапозон нахождения корня");
                        edges = GetUserInput();
                        if (edges.Length == 2)
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
