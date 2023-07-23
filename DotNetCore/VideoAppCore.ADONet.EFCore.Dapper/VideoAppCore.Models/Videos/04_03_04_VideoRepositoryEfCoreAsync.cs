using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VideoAppCore.Models
{
    /// <summary>
    /// [4][3][2] 리포지토리 클래스(비동기 방식): Full ORM인 EF Core를 사용하여 CRUD 구현
    /// </summary>
    public class VideoRepositoryEfCoreAsync : IVideoRepositoryAsync
    {
        private readonly VideoDbContext _context;

        public VideoRepositoryEfCoreAsync(VideoDbContext context) => _context = context;

        // 입력: Add
        public async Task<Video> AddVideoAsync(Video model)
        {
            _context.Add(model);
            await _context.SaveChangesAsync();
            return model;
        }

        // 출력: GetAll 
        public async Task<List<Video>> GetVideosAsync() => await _context.Videos.ToListAsync();

        // 상세보기: GetById
        public async Task<Video> GetVideoByIdAsync(int id) => await _context.Videos.Where(v => v.Id == id).SingleOrDefaultAsync();

        // 수정: Update, Edit
        public async Task<Video> UpdateVideoAsync(Video model)
        {
            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return model; 
        }

        // 삭제: Delete, Remove
        public async Task RemoveVideoAsync(int id)
        {
            var video = await _context.Videos.Where(v => v.Id == id).SingleOrDefaultAsync();
            if (video != null)
            {
                _context.Videos.Remove(video);
                await _context.SaveChangesAsync(); 
            }
        }
    }
}
