using HireHive.Domain.Entities;
using HireHive.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HireHive.Infrastructure.Data.Repositories
{
    internal class ResumeRepository : IResumeRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public ResumeRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Resume?> GetByIdAsync(Guid resumeId)
        {
            var resume = await _context.Resume.FirstOrDefaultAsync(r => r.Id == resumeId);

            return resume;
        }
        public async Task AddAsync(Resume resume)
        {
            await _context.Resume.AddAsync(resume);
            var result = await _context.SaveChangesAsync();

            if (result == 0)
            {
                throw new Exception("Failed to delete resume.");
            }
        }

        public void Delete(Resume resume)
        {
            _context.Remove(resume);
        }

        public async Task UpdateAsync(Resume resume)
        {
            _context.Update(resume);
            var result = await _context.SaveChangesAsync();

            if (result == 0)
            {
                throw new Exception("Failed to update resume.");
            }
        }
    }
}
