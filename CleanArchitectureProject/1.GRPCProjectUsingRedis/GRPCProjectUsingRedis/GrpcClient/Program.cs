using Api.Users;
using Grpc.Net.Client;

namespace GrpcClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Press any Key to contiune...");
            Console.ReadLine();

            using var channel = GrpcChannel.ForAddress("https://localhost:7066");
            var client = new UsersGrpc.UsersGrpcClient(channel);

            var getUserReply = await client.GetUsersAsync(new GetUserRequest { });
            await Console.Out.WriteLineAsync();

            foreach (var user in getUserReply.Users)
            {
                Console.WriteLine($"{user.Id}, {user.Name}, {user.Email}");
            }

            var addUserReply = await client.AddUserAsync(new AddUserRequest
            {
                Id = 4,
                Name = "jdg4",
                Email = "jdg4@gmail.com"
            });

            getUserReply = await client.GetUsersAsync(new GetUserRequest { }); // User 목록 최신화
            await Console.Out.WriteLineAsync();

            foreach (var user in getUserReply.Users)
            {
                Console.WriteLine($"{user.Id}, {user.Name}, {user.Email}");
            }

            var deleteUserReply = await client.DeleteUserAsync(new DeleteUserRequest 
            { 
                Id = 1 
            });

            getUserReply = await client.GetUsersAsync(new GetUserRequest { }); // User 목록 최신화
            await Console.Out.WriteLineAsync();

            foreach (var user in getUserReply.Users)
            {
                Console.WriteLine($"{user.Id}, {user.Name}, {user.Email}");
            }

            Console.WriteLine("Press any Key to contiune...");
            Console.ReadLine();
        }
    }
}
