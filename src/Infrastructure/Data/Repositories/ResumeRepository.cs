using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Data.Repositories
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
        public async Task<Resume?> GetResume(Guid resumeId)
        {
            throw new NotImplementedException();

            //var resume = await _context.Resume.FirstOrDefaultAsync(resumeId);
            //return resume == null ? null : _mapper.Map<Resume>(resume);
        }
        public Task<Resume> AddResume(Resume resume)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteResume(Guid resumeId)
        {
            throw new NotImplementedException();
        }

        public Task<Resume> UpdateResume(Resume resume)
        {
            throw new NotImplementedException();
        }
    }
}
