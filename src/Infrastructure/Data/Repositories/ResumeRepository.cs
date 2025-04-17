using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

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
        public async Task<Resume?> GetResume(int resumeId)
        {
            var resume = await _context.Resume.FirstOrDefaultAsync(resumeId);
            return resume == null ? null : _mapper.Map<Resume>(resume);
        }
        public Task<Resume> AddResume(Resume resume)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteResume(int resumeId)
        {
            throw new NotImplementedException();
        }

        public Task<Resume> UpdateResume(Resume resume)
        {
            throw new NotImplementedException();
        }
    }
}
