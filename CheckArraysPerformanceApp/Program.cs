using System;
using System.Diagnostics;
using System.Linq;

namespace CheckArraysPerformanceApp;

class Program
{
    private static int _arrayLength = 10000;
    private const int Attempts = 2;

    private static void Main()
    {
        _arrayLength = PromptArrayLength();
        int[,] matrix = InitMatrix();
        int[][] jaggedArray = InitJaggedArray();

        for (int attempt = 1; attempt <= Attempts; attempt++)
        {
            Console.WriteLine($"Attempt number: {attempt}");
            CheckAllPerformance(matrix, jaggedArray);
            Console.WriteLine();
        }
        
        Console.ReadKey();
    }

    private static int[,] InitMatrix() => new int[_arrayLength, _arrayLength];

    private static int[][] InitJaggedArray()
    {
        int[][] jaggedArray = new int[_arrayLength][];
        for (int i = 0; i < _arrayLength; i++)
            jaggedArray[i] = new int[_arrayLength];

        return jaggedArray;
    }

    private static void CheckAllPerformance(int[,] matrix, int[][] jaggedArray)
    {
        CheckPerformance(() => Safe2DimArrayAccess(matrix),             "MATRIX Get sum with FOR enumeration");
        CheckPerformance(() => Unsafe2DimArrayAccess(matrix),           "MATRIX Get sum with UNSAFE pointer enumeration");
        CheckPerformance(() => ForSumForJaggedArrayAccess(jaggedArray), "JAGGED ARRAY Get sum with FOR enumeration");
        CheckPerformance(() => ForeachSumForJaggedArray(jaggedArray),   "JAGGED ARRAY Get sum with FOREACH enumeration");
        CheckPerformance(() => LinqSumForJaggedArray(jaggedArray),      "JAGGED ARRAY Get sum with LINQ enumeration");
    }

    private static int PromptArrayLength()
    {
        Console.Write("Enter array length: ");

        int lenght = 0;
        while (lenght == 0)
        {
            var lengthString = Console.ReadLine();
            if (!int.TryParse(lengthString, out lenght) || lenght <= 0)
            {
                Console.WriteLine("Array length should be greater than 0");
            }
        }

        return lenght;
    }

    private static void CheckPerformance(Func<int> func, string methodName)
    {
        var sw = Stopwatch.StartNew();
        int safe2DimArrayAccess = func();
        TimeSpan elapsed = sw.Elapsed;

        Console.Write(methodName);
        Console.CursorLeft = 50;
        Console.WriteLine($": {elapsed} - {safe2DimArrayAccess}");
    }

    private static int Safe2DimArrayAccess(int[,] a)
    {
        int sum = 0;
        for (int i = 0; i < _arrayLength; i++)
        {
            for (int j = 0; j < _arrayLength; j++)
                sum += a[i, j];
        }

        return sum;
    }

    private static int ForSumForJaggedArrayAccess(int[][] a)
    {
        int sum = 0;
        for (int i = 0; i < _arrayLength; i++)
        {
            for (int j = 0; j < _arrayLength; j++)
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
            for (int i = 0; i < _arrayLength; i++)
            {
                int baseOfDim = i * _arrayLength;
                for (int j = 0; j < _arrayLength; j++)
                    sum += pi[baseOfDim + j];
            }
        }

        return sum;
    }
}