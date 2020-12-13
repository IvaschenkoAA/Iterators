using System;
using System.Diagnostics;


namespace CheckArraysPerformanceApp
{
	class Program
	{
		private const int c_numElements = 10000;

		static void Main()
		{
			int[,] a2Dim = new int[c_numElements, c_numElements];
			int[][] aJagged = new int[c_numElements][];

			for (int i = 0; i < c_numElements; i++)
				aJagged[i] = new int[c_numElements];


			CheckPerformance(() => Safe2DimArrayAccess(a2Dim));
			CheckPerformance(() => SafeJaggedArrayAccess(aJagged));
			CheckPerformance(() => Unsafe2DimArrayAccess(a2Dim));

            Console.ReadKey();
		}

		private static void CheckPerformance(Func<int> func)
		{
			var sw = Stopwatch.StartNew();
			int safe2DimArrayAccess = func();
			TimeSpan elapsed = sw.Elapsed;
			Console.WriteLine($"{elapsed} - {safe2DimArrayAccess}");
		}

		private static int Safe2DimArrayAccess(int[,] a)
		{
			int sum = 0;
			for (int i = 0; i < c_numElements; i++)
			{
				for (int j = 0; j < c_numElements; j++)
					sum += a[i, j];

			}

			return sum;
		}

		private static int SafeJaggedArrayAccess(int[][] a)
		{
			int sum = 0;
			for (int i = 0; i < c_numElements; i++)
			{
				for (int j = 0; j < c_numElements; j++)
					sum += a[i][j];

			}

			return sum;
		}

		private static unsafe int Unsafe2DimArrayAccess(int[,] a)
		{
			int sum = 0;
			fixed (int* pi = a)
			{
				for (int i = 0; i < c_numElements; i++)
				{
					int baseOfDim = i * c_numElements;
					for (int j = 0; j < c_numElements; j++)
						sum += pi[baseOfDim + j];
				}
			}

			return sum;
		}
	}
}
