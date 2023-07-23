using System.Collections.Generic;

namespace VideoAppCore.Models
{
    /// <summary>
    /// [4][3][1] 리포지토리 클래스(동기 방식): Full ORM인 EF Core를 사용하여 CRUD 구현
    /// </summary>
    public class VideoRepositoryEfCore : IVideoRepository
    {
        public Video AddVideo(Video model)
        {
            throw new System.NotImplementedException();
        }

        public Video GetVideoById(int id)
        {
            throw new System.NotImplementedException();
        }

        public List<Video> GetVideos()
        {
            throw new System.NotImplementedException();
        }

        public void RemoveVideo(int id)
        {
            throw new System.NotImplementedException();
        }

        public Video UpdateVideo(Video model)
        {
            throw new System.NotImplementedException();
        }
    }
}
