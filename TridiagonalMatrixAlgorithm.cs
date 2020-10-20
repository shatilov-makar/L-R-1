using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class TridiagonalMatrixAlgorithm
    {
        const int matrixSize = 4;

        public TridiagonalMatrixAlgorithm(double[,] matrix)
        {
            if (!isTriDiagonal(matrix))
            {
                Console.WriteLine("Матрица не трехдиагональная");
                Console.ReadKey();
            }
            var coefficients = GetCoefficients(matrix);
            var s = FirstStep(coefficients);

            var res = SecondStep(s[0], s[1]);
            Program.ShowRoots(res);
            Console.ReadKey();
        }

        public static Dictionary<char, double>[] GetCoefficients(double[,] matrix)
        {
            var res = new Dictionary<char, double>[matrixSize];
            var k = -1;
            for (var i = 0; i < matrixSize; i++, k++)
            {
                var t = new Dictionary<char, double>();
                if (k == -1)
                    t.Add('a', 0);
                else t.Add('a', matrix[i, k]);
                t.Add('b', matrix[i, k + 1]);
                if (k + 2 == matrixSize)
                    t.Add('c', 0);
                else t.Add('c',matrix[i, k+2]);
                t.Add('d', matrix[i, matrixSize]);
                res[i] = t;
            }
            return res;
        }

        static double[][] FirstStep(Dictionary<char, double>[] dic)
        {
            var u = new double[matrixSize];
            var v = new double[matrixSize];
            u[0] = - dic[0]['c'] / dic[0]['b'];
            v[0] = dic[0]['d'] / dic[0]['b'];
            for (int i = 1; i < matrixSize; i++)
            {
               var y = dic[i]['a'] * u[i - 1] + dic[i]['b'];
                u[i] = -dic[i]['c'] / y;
                v[i] = (dic[i]['d'] - dic[i]['a'] * v[i - 1]) / y;
            }
            return new double[][] { u, v };
        }

        static double[] SecondStep(double[] u, double[] v)
        {
            var res = new double[matrixSize];
            res[matrixSize - 1] = v[matrixSize - 1];
            for (var i = matrixSize - 2; i >= 0; i--)
                res[i] = u[i] * res[i + 1] + v[i];
            return res;
        }

        public bool isTriDiagonal(double[,] matrix)
        {
            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                for (int y = 0; y < matrix.GetLength(1) - 1; y++)
                {
                    var cell = matrix[x, y];
                    if ((x == y) || (x - 1 == y) || (x + 1 == y))
                    {
                        if (cell == 0)
                            return false;
                    }
                    else if (cell != 0) 
                        return false;
                }
            }
            return true;
        }
    }
}
