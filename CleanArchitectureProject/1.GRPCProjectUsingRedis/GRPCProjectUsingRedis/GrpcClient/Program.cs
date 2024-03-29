﻿using Api.Users;
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

            // 외부 네트워크를 통해 포워딩했을 때 사용 (도커 네트워크 생성했으므로 자신의 PC IP 사용)
            //using var channel = GrpcChannel.ForAddress("https://192.168.100.142:7066");

            // 클라이언트, 서버가 같은 환경에서 실행중일 때 사용 (디버깅 테스트 시)
            using var channel = GrpcChannel.ForAddress("https://localhost:7066");

            var client = new UsersGrpc.UsersGrpcClient(channel);

            var getUserReply = await client.GetUsersAsync(new GetUserRequest { });
            await Console.Out.WriteLineAsync();

            foreach (var user in getUserReply.Users)
            {
                Console.WriteLine($"{user.Id}, {user.Name}, {user.Email}");
            }

            Console.WriteLine();
            Console.WriteLine("Add user jdg4...");

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

            Console.WriteLine();
            Console.WriteLine("Remove user jdg1...");

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
