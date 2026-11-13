using HireHive.Domain.Entities;
using HireHive.Domain.Enums;
using HireHive.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HireHive.Infrastructure.Data.Repositories;

internal class JobRepository(HireHiveDbContext context) : IJobRepository
{
    private readonly HireHiveDbContext _context = context;

    public async Task<Job?> GetByIdAsync(Guid jobId)
    {
        var job = await _context.Job.FirstOrDefaultAsync(j => j.Id == jobId);

        return job;
    }

    public async Task<List<Job>> GetByOwnerIdAsync(Guid ownerId, JobStatus? status = null)
    {
        var query = _context.Job.Where(j => j.OwnerId == ownerId);

        if (status != null)
            query = query.Where(j => j.Status == status);

        return await query
            .OrderByDescending(j => j.Audit.CreatedAt)
            .ToListAsync();
    }

    public async Task AddAsync(Job job)
    {
        await _context.Job.AddAsync(job);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Job job)
    {
        _context.Remove(job);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Job job)
    {
        _context.Update(job);
        await _context.SaveChangesAsync();
    }
}
