using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IResumeRepository
    {
        Task<Resume?> GetResume(int resumeId);
        Task<Resume> AddResume(Resume resume);
        Task<Resume> UpdateResume(Resume resume);
        Task<bool> DeleteResume(int resumeId);
    }
}
