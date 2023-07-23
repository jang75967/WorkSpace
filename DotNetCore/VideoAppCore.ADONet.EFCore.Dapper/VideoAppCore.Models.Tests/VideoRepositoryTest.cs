using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// TODO: 단일 테스트 메서드에서 리포지토리 클래스의 모든 CRUD를 한꺼번에 테스트
/// </summary>
namespace VideoAppCore.Models.Tests
{
    [TestClass]
    public class VideoRepositoryTest
    {
        [TestMethod]
        public async Task AddVideoAsyncMethodTest()
        {
            // DbContextOptions 생성
            // DbContextOptionsBuilder를 사용하여 인-메모리 데이터베이스 정보를 DbContext에 전달
            var options = new DbContextOptionsBuilder<VideoDbContext>().UseInMemoryDatabase(databaseName: "AddVideo").Options;

            // 컨텍스트 개체 생성
            using (var context = new VideoDbContext(options))
            {
                // 리포지토리 개체 생성
                var repository = new VideoRepositoryEfCoreAsync(context);
                var video = new Video { Title = "제목", Url = "URL", Company = "Hawaso", Name = "홍길동" };
                await repository.AddVideoAsync(video);
                context.SaveChanges();
            }

            using (var context = new VideoDbContext(options))
            {
                Assert.IsTrue(context.Videos.Count() > 0);
                Assert.AreEqual("URL", context.Videos.Where(v => v.Name == "홍길동").Single().Url);
            }
        }

        [TestMethod]
        public async Task GetVideosAsyncMethodTest()
        {
            // DbContextOptions 생성
            // DbContextOptionsBuilder를 사용하여 인-메모리 데이터베이스 정보를 DbContext에 전달
            var options = new DbContextOptionsBuilder<VideoDbContext>().UseInMemoryDatabase(databaseName: "GetVideos").Options;

            // 컨텍스트 개체 생성
            using (var context = new VideoDbContext(options))
            {
                var video1 = new Video { Title = "제목", Url = "URL", Company = "Hawaso", Name = "박용준" };
                context.Videos.Add(video1);
                var video2 = new Video { Title = "제목", Url = "URL", Company = "Hawaso", Name = "김태영" };
                context.Videos.Add(video2);
                var video3 = new Video { Title = "제목", Url = "URL", Company = "Hawaso", Name = "한상훈" };
                context.Videos.Add(video3);
                context.SaveChanges();
            }

            using (var context = new VideoDbContext(options))
            {
                var repository = new VideoRepositoryEfCoreAsync(context);
                var videos = await repository.GetVideosAsync();
                Assert.AreEqual(3, videos.Count());
                Assert.AreEqual("김태영", videos.Where(v => v.Id == 2).FirstOrDefault()?.Name);
            }
        }

        //[TestMethod]
        //public async Task GetVideosAsyncMethodTestWithSqlite()
        //{
        //    // In-memory database only exists while the connection is open
        //    var connection = new SqliteConnection("DataSource=:memory:");
        //    connection.Open();

        //    try
        //    {
        //        // DbContextOptions 생성
        //        // DbContextOptionsBuilder를 사용하여 인-메모리 데이터베이스 정보를 DbContext에 전달
        //        var options = new DbContextOptionsBuilder<VideoDbContext>().UseSqlite(connection).Options;

        //        using (var context = new VideoDbContext(options))
        //        {
        //            context.Database.EnsureCreated();
        //        }

        //        // 컨텍스트 개체 생성
        //        using (var context = new VideoDbContext(options))
        //        {
        //            var video1 = new Video { Title = "제목", Url = "URL", Company = "Hawaso", Name = "박용준" };
        //            context.Videos.Add(video1);
        //            var video2 = new Video { Title = "제목", Url = "URL", Company = "Hawaso", Name = "김태영" };
        //            context.Videos.Add(video2);
        //            var video3 = new Video { Title = "제목", Url = "URL", Company = "Hawaso", Name = "한상훈" };
        //            context.Videos.Add(video3);
        //            context.SaveChanges();
        //        }

        //        using (var context = new VideoDbContext(options))
        //        {
        //            var repository = new VideoRepositoryEfCoreAsync(context);
        //            var videos = await repository.GetVideosAsync();
        //            Assert.AreEqual(3, videos.Count());
        //            Assert.AreEqual("김태영", videos.Where(v => v.Id == 2).FirstOrDefault()?.Name);
        //        }
        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }
        //}

        [TestMethod]
        public async Task GetVideoByIdAsyncMethodTest()
        {
            // DbContextOptions 생성
            // DbContextOptionsBuilder를 사용하여 인-메모리 데이터베이스 정보를 DbContext에 전달
            var options = new DbContextOptionsBuilder<VideoDbContext>().UseInMemoryDatabase(databaseName: "GetVideoById").Options;

            // 컨텍스트 개체 생성
            using (var context = new VideoDbContext(options))
            {
                context.Videos.Add(new Video { Title = "제목", Url = "URL", Company = "Hawaso", Name = "박용준" });
                context.Videos.Add(new Video { Title = "제목", Url = "URL", Company = "Hawaso", Name = "김태영" });
                context.Videos.Add(new Video { Title = "제목", Url = "URL", Company = "Hawaso", Name = "한상훈" });
                context.SaveChanges();
            }

            using (var context = new VideoDbContext(options))
            {
                var repository = new VideoRepositoryEfCoreAsync(context);
                var video = await repository.GetVideoByIdAsync(3);
                Assert.AreEqual("제목", video.Title);
                Assert.AreEqual("한상훈", video.Name);
            }
        }

        [TestMethod]
        public async Task UpdateVideoAsyncMethodTest()
        {
            // DbContextOptions 생성
            // DbContextOptionsBuilder를 사용하여 인-메모리 데이터베이스 정보를 DbContext에 전달
            var options = new DbContextOptionsBuilder<VideoDbContext>().UseInMemoryDatabase(databaseName: "UpdateVideo").Options;

            // 컨텍스트 개체 생성
            using (var context = new VideoDbContext(options))
            {
                context.Videos.Add(new Video { Title = "제목", Url = "URL", Company = "Hawaso", Name = "박용준" });
                context.Videos.Add(new Video { Title = "제목", Url = "URL", Company = "Hawaso", Name = "김태영" });
                context.Videos.Add(new Video { Title = "제목", Url = "URL", Company = "Hawaso", Name = "한상훈" });
                context.SaveChanges();
            }

            using (var context = new VideoDbContext(options))
            {
                var repository = new VideoRepositoryEfCoreAsync(context);
                var han = await repository.GetVideoByIdAsync(3);
                han.Title = "넥슨개발자";
                await repository.UpdateVideoAsync(han);
                context.SaveChanges();

                var itist = await repository.GetVideoByIdAsync(3);
                Assert.AreEqual("넥슨개발자", itist.Title);
                Assert.AreEqual("한상훈", itist.Name);
            }
        }

        [TestMethod]
        public async Task DeleteVideoAsyncMethodTest()
        {
            // DbContextOptions 생성
            // DbContextOptionsBuilder를 사용하여 인-메모리 데이터베이스 정보를 DbContext에 전달
            var options = new DbContextOptionsBuilder<VideoDbContext>().UseInMemoryDatabase(databaseName: "DeleteVideo").Options;

            // 컨텍스트 개체 생성
            using (var context = new VideoDbContext(options))
            {
                context.Videos.Add(new Video { Title = "제목", Url = "URL", Company = "Hawaso", Name = "박용준" });
                context.Videos.Add(new Video { Title = "제목", Url = "URL", Company = "Hawaso", Name = "김태영" });
                context.Videos.Add(new Video { Title = "제목", Url = "URL", Company = "Hawaso", Name = "한상훈" });
                context.SaveChanges();
            }

            using (var context = new VideoDbContext(options))
            {
                var repository = new VideoRepositoryEfCoreAsync(context);
                await repository.RemoveVideoAsync(1);
                context.SaveChanges();

                var videos = await repository.GetVideosAsync();

                Assert.AreEqual(2, videos.Count());
                Assert.IsNull(videos.Where(v => v.Name == "박용준").SingleOrDefault());
            }
        }
    }
}
