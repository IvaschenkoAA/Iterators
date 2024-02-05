using System;
using System.Diagnostics;
using System.Linq;


namespace CheckArraysPerformanceApp
{
    class Program
    {
        private const int ElementsCount = 30_000;

        static void Main()
        {
            int[,] a2Dim = new int[ElementsCount, ElementsCount];
            int[][] aJagged = new int[ElementsCount][];

            for (int i = 0; i < ElementsCount; i++)
                aJagged[i] = new int[ElementsCount];


            CheckPerformance(() => Safe2DimArrayAccess(a2Dim),               "Safe matrix enumeration  ");
            CheckPerformance(() => Unsafe2DimArrayAccess(a2Dim),             "Unsafe matrix enumeration");
            CheckPerformance(() => ForSumForJaggedArrayAccess(aJagged), "For sum for jagged arrays    ");
            CheckPerformance(() => ForeachSumForJaggedArray(aJagged),   "Foreach sum for jagged arrays");
            CheckPerformance(() => LinqSumForJaggedArray(aJagged),      "LINQ sum for jagged arrays   ");

            Console.ReadKey();
        }

        private static void CheckPerformance(Func<int> func, string methodName)
        {
            var sw = Stopwatch.StartNew();
            int safe2DimArrayAccess = func();
            TimeSpan elapsed = sw.Elapsed;
            Console.WriteLine($"{methodName}: {elapsed} - {safe2DimArrayAccess}");
        }

        private static int Safe2DimArrayAccess(int[,] a)
        {
            int sum = 0;
            for (int i = 0; i < ElementsCount; i++)
            {
                for (int j = 0; j < ElementsCount; j++)
                    sum += a[i, j];
            }

            return sum;
        }

        private static int ForSumForJaggedArrayAccess(int[][] a)
        {
            int sum = 0;
            for (int i = 0; i < ElementsCount; i++)
            {
                for (int j = 0; j < ElementsCount; j++)
                    sum += a[i][j];

            }

            return sum;
        }

        private static int ForeachSumForJaggedArray(int[][] a)
        {
            int sum = 0;
            foreach (int[] ints in a)
            {
                foreach (int i in ints)
                {
                    sum += i;
                }
            }

            return sum;
        }

        private static int LinqSumForJaggedArray(int[][] a)
        {
            return a.Sum(i => i.Sum());
        }

        private static unsafe int Unsafe2DimArrayAccess(int[,] a)
        {
            int sum = 0;
            fixed (int* pi = a)
            {
                for (int i = 0; i < ElementsCount; i++)
                {
                    int baseOfDim = i * ElementsCount;
                    for (int j = 0; j < ElementsCount; j++)
                        sum += pi[baseOfDim + j];
                }
            }

            return sum;
        }
    }
}
