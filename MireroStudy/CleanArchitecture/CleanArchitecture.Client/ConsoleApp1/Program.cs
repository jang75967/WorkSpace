using Grpc.Net.Client;
using GrpcClient;

namespace ConsoleApp1
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Press any Key to contiune...");
            Console.ReadLine();

            using var channel = GrpcChannel.ForAddress("https://localhost:7277");
            var client = new UsersGrpc.UsersGrpcClient(channel);
            var reply = await client.GetUsersAsync(new GetUserRequest { });

            await Console.Out.WriteLineAsync();

            foreach (var user in reply.Users)
            {
                Console.WriteLine($"{user.Id}, {user.Name}, {user.Email}");
            }

            await Console.Out.WriteLineAsync();
            Console.WriteLine("Press any Key to contiune...");
            Console.ReadLine();
        }
    }
}