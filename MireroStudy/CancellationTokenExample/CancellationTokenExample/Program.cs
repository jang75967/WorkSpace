
using System.Numerics;

namespace CancellationTokenExample
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            CancellationTokenSource cancellationTokenSource = new();

            LongWork(long.MaxValue - 1, cancellationTokenSource.Token);

            Console.WriteLine("Press any key to cancel...");
            Console.ReadLine();
            cancellationTokenSource.Cancel();
            Console.WriteLine("End-of-work");
        }

        static async Task LongWork(long to, CancellationToken token)
        {
            Console.WriteLine("Starting...");
            await Task.Factory.StartNew(() => GetSum(to, token));
            Console.WriteLine("Completed...");
        }

        private static BigInteger GetSum(long last, CancellationToken token)
        {
            BigInteger sum = 0;

            for (long i = 0; i < last; i++)
            {
                if (token.IsCancellationRequested == true)
                {
                    break;
                }

                i++;
                sum += i;
            }

            return sum;
        }
    }
}
