using HireHive.Domain.Entities;
using HireHive.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HireHive.Infrastructure.Data.Repositories
{
    internal class ResumeRepository : IResumeRepository
    {
        private readonly AppDbContext _context;
        public ResumeRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Resume?> GetByIdAsync(Guid resumeId)
        {
            var resume = await _context.Resume.FirstOrDefaultAsync(r => r.Id == resumeId);

            return resume;
        }

        public async Task<Resume?> GetByUserIdAsync(Guid userId)
        {
            var resume = await _context.Resume.FirstOrDefaultAsync(r => r.UserId == userId);

            return resume;
        }
        public async Task AddAsync(Resume resume)
        {
            await _context.Resume.AddAsync(resume);
            await _context.SaveChangesAsync();
        }

        public void Delete(Resume resume)
        {
            _context.Remove(resume);
        }

        public async Task UpdateAsync(Resume resume)
        {
            _context.Update(resume);
            await _context.SaveChangesAsync();
        }
    }
}
