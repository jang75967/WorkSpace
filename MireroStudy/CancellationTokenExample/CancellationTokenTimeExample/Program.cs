using System.Numerics;

namespace CancellationTokenTimeExample;

internal class Program
{
    static async Task Main(string[] args)
    {
        CancellationTokenSource cancellationTokenSource = new();

        // 5초 후에 작업이 취소되도록 설정
        cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(5));

        try
        {
            await LongWork(long.MaxValue - 1, cancellationTokenSource.Token);
            Console.WriteLine("Completed...");
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Task was canceled due to timeout.");
        }

        Console.WriteLine("Press any key to exit...");
        Console.ReadLine();
    }

    static async Task LongWork(long to, CancellationToken token)
    {
        Console.WriteLine("Starting...");
        await Task.Run(() => GetSum(to, token), token);
    }

    private static BigInteger GetSum(long last, CancellationToken token)
    {
        BigInteger sum = 0;

        for (long i = 0; i < last; i++)
        {
            // 작업 취소 확인
            token.ThrowIfCancellationRequested();

            i++;
            sum += i;
        }

        return sum;
    }
}