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

            Console.WriteLine();

            // gRPC 클라이언트 생성 시 HttpClientHandler를 사용하여 SSL 인증을 무시
            // 경고: 보안상의 이유로 실제 환경에서는 권장하지 않음
            //var httpHandler = new HttpClientHandler();
            //httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            //using var channel = GrpcChannel.ForAddress("http://192.168.100.142:7066", new GrpcChannelOptions { HttpHandler = httpHandler });
            //using var channel = GrpcChannel.ForAddress("http://localhost:7066", new GrpcChannelOptions { HttpHandler = httpHandler });



            //using var channel = GrpcChannel.ForAddress("https://192.168.100.142:7066");

            // 디버깅 테스트 시
            using var channel = GrpcChannel.ForAddress("https://localhost:7066");

            // 컨테이너 서비스 명
            //using var channel = GrpcChannel.ForAddress("https://jdgworkerservice-1:7066");

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

            Console.WriteLine("Add user jdg4...");

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

            Console.WriteLine("Add user jdg1...");

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
