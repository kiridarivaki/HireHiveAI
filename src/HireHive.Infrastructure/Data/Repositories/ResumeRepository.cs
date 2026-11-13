using HireHive.Domain.Entities;
using HireHive.Domain.Enums;
using HireHive.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HireHive.Infrastructure.Data.Repositories
{
    internal class ResumeRepository : IResumeRepository
    {
        private readonly HireHiveDbContext _context;
        public ResumeRepository(HireHiveDbContext context)
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

        public async Task DeleteAsync(Resume resume)
        {
            _context.Remove(resume);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Resume resume)
        {
            _context.Update(resume);
            await _context.SaveChangesAsync();
        }

        public List<Resume> GetResumesToAssess(JobType jobType, int skip, int take)
        {
            return _context.Resume
                .Join(_context.Users,
                      resume => resume.UserId,
                      user => user.Id,
                      (resume, user) => new { Resume = resume, User = user })
                .Where(joined => joined.User.JobTypes.Contains(jobType))
                .OrderBy(joined => joined.User.Id)
                .Select(joined => new Resume
                {
                    UserId = joined.Resume.UserId,
                    Text = joined.Resume.Text
                })
                .Skip(skip)
                .Take(take)
                .ToList();
        }

    }
}
