using Microsoft.EntityFrameworkCore;
using OBSSystem.Application.Interfaces;
using OBSSystem.Core.Entities;
using OBSSystem.Infrastructure.Configurations;


namespace OBSSystem.Infrastructure.Repositories
{
    public class ActivityLogRepository : IActivityLogRepository
    {
        private readonly OBSContext _context;

        public ActivityLogRepository(OBSContext context)
        {
            _context = context;
        }

        public async Task LogActivityAsync(ActivityLog activityLog)
        {
            await _context.ActivityLogs.AddAsync(activityLog);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ActivityLog>> GetAllActivitiesAsync()
        {
            return await _context.ActivityLogs
                .OrderByDescending(a => a.Timestamp)
                .ToListAsync();
        }
    }
}