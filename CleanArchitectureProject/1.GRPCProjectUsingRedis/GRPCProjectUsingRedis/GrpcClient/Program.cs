using Api.Groups;
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

            // 외부 네트워크를 통해 포워딩했을 때 사용 (도커 네트워크 생성했으므로 자신의 PC IP 사용)
            //using var channel = GrpcChannel.ForAddress("https://192.168.100.142:7066");

            // 클라이언트, 서버가 같은 환경에서 실행중일 때 사용 (디버깅 테스트 시)
            using var channel = GrpcChannel.ForAddress("https://localhost:7066");

            var userclient = new UsersGrpc.UsersGrpcClient(channel);

            var getUserReply = await userclient.GetUsersAsync(new GetUserRequest { });
            await Console.Out.WriteLineAsync();

            foreach (var user in getUserReply.Users)
            {
                Console.WriteLine($"{user.Id}, {user.Name}, {user.Email}");
            }

            Console.WriteLine();
            Console.WriteLine("Add user jdg4...");

            var addUserReply = await userclient.AddUserAsync(new AddUserRequest
            {
                Id = 4,
                Name = "jdg4",
                Email = "jdg4@gmail.com"
            });

            getUserReply = await userclient.GetUsersAsync(new GetUserRequest { }); // User 목록 최신화
            await Console.Out.WriteLineAsync();

            foreach (var user in getUserReply.Users)
            {
                Console.WriteLine($"{user.Id}, {user.Name}, {user.Email}");
            }

            Console.WriteLine();
            Console.WriteLine("Remove user jdg4...");


            // 테스트용 예외 처리 코드
            try
            {
                var deleteUserReply = await userclient.DeleteUserAsync(new DeleteUserRequest
                {
                    Id = 4
                });

                Console.WriteLine("사용자 삭제 성공");
            }
            catch (Grpc.Core.RpcException rpcEx) when (rpcEx.StatusCode == Grpc.Core.StatusCode.Unknown)
            {
                Console.WriteLine($"gRPC 예외 발생: {rpcEx.Status.Detail}");
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine($"타임아웃 예외 발생: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"예외 발생: {ex.Message}");
            }


            getUserReply = await userclient.GetUsersAsync(new GetUserRequest { }); // User 목록 최신화
            await Console.Out.WriteLineAsync();

            foreach (var user in getUserReply.Users)
            {
                Console.WriteLine($"{user.Id}, {user.Name}, {user.Email}");
            }

            // Update 테스트 추가
            await Console.Out.WriteLineAsync("Update User DataBase Test....");
            var updateUserReply = await userclient.UpdateUserAsync(new UpdateUserRequest
            {
                Id=1,
                Name = "jdgUpdateTest",
                Email = "jdg@UpdateTest.com"
            });

            getUserReply = await userclient.GetUsersAsync(new GetUserRequest { }); // User 목록 최신화
            foreach (var user in getUserReply.Users)
            {
                Console.WriteLine($"{user.Id}, {user.Name}, {user.Email}");
            }






            // Group 테스트
            var groupclient = new GroupsGrpc.GroupsGrpcClient(channel);
            var getGroupReply = await groupclient.GetGroupsAsync(new GetGroupRequest { });

            Console.WriteLine();
            Console.WriteLine("Add Group 족구...");

            var addGroupReply = await groupclient.AddGroupAsync(new AddGroupRequest
            {
                Id = 4,
                Name = "족구",
            });

            getGroupReply = await groupclient.GetGroupsAsync(new GetGroupRequest { });
            await Console.Out.WriteLineAsync();

            foreach (var group in getGroupReply.Groups)
            {
                Console.WriteLine($"{group.Id}, {group.Name}");
            }

            // Update 테스트 추가
            Console.WriteLine();
            await Console.Out.WriteLineAsync("Update Group DataBase Test....");
            var updateGroupReply = await groupclient.UpdateGroupAsync(new UpdateGroupRequest
            {
                Id = 1,
                Name = "축구_수정",
            });

            getGroupReply = await groupclient.GetGroupsAsync(new GetGroupRequest { });
            foreach (var group in getGroupReply.Groups)
            {
                Console.WriteLine($"{group.Id}, {group.Name}");
            }

            // 삭제
            // 테스트용 예외 처리 코드
            try
            {
                var deleteGroupReply = await groupclient.DeleteGroupAsync(new DeleteGroupRequest
                {
                    Id = 4
                });

                Console.WriteLine("그룹 삭제 성공");
            }
            catch (Grpc.Core.RpcException rpcEx) when (rpcEx.StatusCode == Grpc.Core.StatusCode.Unknown)
            {
                Console.WriteLine($"gRPC 예외 발생: {rpcEx.Status.Detail}");
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine($"타임아웃 예외 발생: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"예외 발생: {ex.Message}");
            }

            getGroupReply = await groupclient.GetGroupsAsync(new GetGroupRequest { });
            foreach (var group in getGroupReply.Groups)
            {
                Console.WriteLine($"{group.Id}, {group.Name}");
            }

            Console.WriteLine("Press any Key to contiune...");
            Console.ReadLine();
        }
    }
}
