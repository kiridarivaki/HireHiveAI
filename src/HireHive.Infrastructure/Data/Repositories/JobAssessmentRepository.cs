using HireHive.Domain.Entities;
using HireHive.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HireHive.Infrastructure.Data.Repositories;

internal class JobAssessmentRepository(HireHiveDbContext context) : IJobAssessmentRepository
{
    private readonly HireHiveDbContext _context = context;

    public async Task<JobAssessment?> GetAsync(Guid jobId, Guid userId)
    {
        var assessment = await _context.JobAssessment
            .FirstOrDefaultAsync(a => a.JobId == jobId && a.UserId == userId);

        return assessment;
    }

    public async Task<List<JobAssessment>> GetByJobIdAsync(Guid jobId)
    {
        return await _context.JobAssessment
            .Where(a => a.JobId == jobId)
            .OrderByDescending(a => a.MatchPercentage)
            .ToListAsync();
    }

    public async Task AddAsync(JobAssessment assessment)
    {
        await _context.JobAssessment.AddAsync(assessment);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(JobAssessment assessment)
    {
        _context.Update(assessment);
        await _context.SaveChangesAsync();
    }
}
