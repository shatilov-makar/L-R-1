using System;
using System.Linq;

namespace ConsoleApp2
{
    class Program
    {
        const int matrixSize = 4;

        static void Main(string[] args)
        {
            var matrix = ChooseParameters();
            var g =  (double[,]) matrix.Clone();
            if (!IsLinearIndependence(matrix))
            {
                Console.WriteLine("Система имеет множество решений");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Метод Гаусса...");
            ShowMatrix(GetSimplifyMatrix(matrix));
            ShowRoots(GetSolve(matrix));
            Console.WriteLine("Метод прогонки...");
            new TridiagonalMatrixAlgorithm(g);
            Console.ReadKey();
        }

        private static double[,] GetSimplifyMatrix(double[,] matrix)
        {
            var h = 0;
            while (h < matrixSize - 1)
            {
                SimplifyLine(matrix, h, h);
                for (var l = h + 1; l < matrixSize; l++)
                {
                    if (matrix[h, h] == 0)
                        SwapLines(matrix, h, h + 1);
                    if (matrix[l, h] == 0)
                        continue;
                    var k = matrix[l, h];
                    for (var i = h; i < matrixSize + 1; i++)
                        matrix[l, i] = matrix[l, i] - k * matrix[h, i];
                }
                h++;
            }
            return matrix;
        }


        private static double[] GetSolve(double[,] matrix)
        {
            var roots = new double[matrixSize];
            for (var i = matrixSize - 1; i >= 0; i--)
            {
                var p = 0.0;
                for (var t = 0; t < matrixSize; t++)
                    p += matrix[i, t] * roots[t];
                roots[i] = (matrix[i, matrixSize] - p) / matrix[i, i];
            }
            return roots;
        }

        private static void SimplifyLine(double[,] matrix, int i1, int j1)
        {
            if (matrix[i1, j1] != 1)
            {
                var k = matrix[i1, j1];
                for (var j = j1; j < 5; j++)
                    matrix[i1, j] /= k;
            }
        }

        static void SwapLines(double[,] sys, int l1, int l2)
        {
            var temp = 0.0;
            for (var i = 0; i < matrixSize + 1; i++)
            {
                temp = sys[l1, i];
                sys[l1, i] = sys[l2, i];
                sys[l2, i] = temp;
            }
        }


        private static double[,] ChooseParameters()
        {
            var parameters = new double[matrixSize, matrixSize + 1];
            Console.WriteLine("Введите по порядку коэффициенты уравнений");
            for (var i = 0; i < matrixSize; i++)
            {
                while(true)
                {
                    var m = GetUserInput();
                    if (m.Length == matrixSize + 1)
                    {
                        for (var j = 0; j < matrixSize + 1; j++)
                            parameters[i, j] = m[j];
                        break;
                    }
                    ShowError();
                }

            }
            return parameters;
        }


        static double[] GetUserInput()
        {
            return Console.ReadLine().Split(' ')
                           .Where(x => !string.IsNullOrWhiteSpace(x) && double.TryParse(x, out var d))
                           .Select(x => double.Parse(x))
                           .ToArray();
        }
        static void ShowError()
        {
            Console.WriteLine("Ошибка! Следуйте командам");
            Console.WriteLine();
        }

        public static void ShowMatrix(double[,] matrix)
        {
            Console.WriteLine();
            for (var i = 0; i < matrix.GetLength(0); i++)
            {
                for (var j = 0; j < matrix.GetLength(1); j++)
                {
                    if (j == matrix.GetLength(1) - 1)
                        Console.Write(" | ");
                    if (matrix[i, j] >= 10 || matrix[i, j] < 0)
                        Console.Write(matrix[i, j] + "  ");
                    else if (matrix[i, j] != Math.Truncate(matrix[i, j]))
                        Console.Write(matrix[i, j] + " ");
                    else
                        Console.Write(matrix[i, j] + "   ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }


        public static void ShowRoots(double[] roots)
        {
            for (var i = 0; i < roots.Length; i++)
                Console.WriteLine($"x{i + 1} =  {roots[i]}");
            Console.WriteLine();
        }

        static bool IsLinearIndependence(double[,] matrix)
        {
            for (var i = 0; i < matrix.GetLength(0) - 1; i++)
            {
                for (var j = i + 1; j < matrix.GetLength(0); j++)
                {
                    var count = 0;
                    var pivot = 0.0;
                    if (matrix[j, 0] == 0)
                    {
                        if (matrix[i, 0] == 0)
                            count++;
                        else continue;
                    }
                    else pivot = matrix[i, 0] / matrix[j, 0];
                    for (var k = 1; k < matrix.GetLength(1) - 1; k++)
                    {
                        if (matrix[j, k] == 0)
                        {
                            if (matrix[i, k] == 0)
                                count++;
                            continue;
                        }
                        if (matrix[i, k] / matrix[j, k] == pivot)
                            count++;
                    }
                    if (count == matrix.GetLength(0) - 1)
                        return false;
                }
            }
            return true;

        }
    }
}
