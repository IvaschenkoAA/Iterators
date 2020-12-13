using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace HandleEnumerationApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] lines = { "1", "a", "b", "2", "3" };

            var loader = new LinqNumbersLoader();
            IEnumerable<int> numbers = loader.ParseNumbers(lines).HandleEnumerable(ParseLineHandler);
            foreach (int number in numbers)
            {
                Console.WriteLine(number);
            }
        }

        private static bool ParseLineHandler(Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex.Message);
            Console.ResetColor();

            return true;
        }
    }

    internal interface INumbersLoader
    {
        IEnumerable<int> ParseNumbers(IEnumerable<string> lines);
    }

    internal class YieldReturnNumbersLoader : INumbersLoader
    {
        public IEnumerable<int> ParseNumbers(IEnumerable<string> lines)
        {
            foreach (string line in lines)
                yield return int.Parse(line);
        }
    }

    internal class LinqNumbersLoader
    {
        public IEnumerable<int> ParseNumbers(IEnumerable<string> lines) => lines.Select(int.Parse);
    }

    public static class EnumerableExtension
    {
        public static IEnumerable<T> HandleEnumerable<T>(this IEnumerable<T> enumerable, Func<Exception, bool> handleFunc) =>
            new EnumerableHandler<T>(enumerable, handleFunc);
    }

    public class EnumerableHandler<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> _enumerable;
        private readonly Func<Exception, bool> _handleFunc;

        public EnumerableHandler(IEnumerable<T> enumerable, Func<Exception, bool> handleFunc)
        {
            _enumerable = enumerable;
            _handleFunc = handleFunc;
        }

        public IEnumerator<T> GetEnumerator() => new EnumeratorHandler<T>(_enumerable.GetEnumerator(), _handleFunc);

        IEnumerator IEnumerable.GetEnumerator() => new EnumeratorHandler<T>(_enumerable.GetEnumerator(), _handleFunc);
    }

    public class EnumeratorHandler<T> : IEnumerator<T>
    {
        private readonly IEnumerator<T> _innerEnumerator;
        private readonly Func<Exception, bool> _handleFunc;

        public EnumeratorHandler(IEnumerator<T> innerEnumerator, Func<Exception, bool> handleFunc)
        {
            _innerEnumerator = innerEnumerator;
            _handleFunc = handleFunc;
        }

        public bool MoveNext()
        {
            try
            {
                return _innerEnumerator.MoveNext();
            }
            catch (Exception ex)
            {
                if (!_handleFunc(ex))
                    throw;

                MoveNext();
                return true;
            }
        }

        public void Reset() => _innerEnumerator.Reset();

        public T Current => _innerEnumerator.Current;

        object IEnumerator.Current => ((IEnumerator)_innerEnumerator).Current;

        public void Dispose() => _innerEnumerator.Dispose();
    }
}
