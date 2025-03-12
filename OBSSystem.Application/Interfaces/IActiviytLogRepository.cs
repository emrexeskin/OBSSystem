using OBSSystem.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OBSSystem.Application.Interfaces
{
    public interface IActivityLogRepository
    {
        Task LogActivityAsync(ActivityLog activityLog);
        Task<IEnumerable<ActivityLog>> GetAllActivitiesAsync();
    }
}