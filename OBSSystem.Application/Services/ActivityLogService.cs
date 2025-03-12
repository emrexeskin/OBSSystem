using OBSSystem.Application.Interfaces;
using OBSSystem.Core.Entities;

namespace OBSSystem.Application.Services
{
    public class ActivityLogService
    
    {
        private readonly IActivityLogRepository _activityLogRepository;

        public ActivityLogService(IActivityLogRepository activityLogRepository)
        {
            _activityLogRepository = activityLogRepository;
        }

        // Kullanıcı etkinliğini asenkron olarak loglamak
        public async Task LogUserActivityAsync(int userId, string action, string description, string ipAddress)
        {
            var activityLog = new ActivityLog
            {
                UserID = userId,
                Action = action,
                Description = description,
                Timestamp = DateTime.UtcNow,
                IpAddress = ipAddress
            };

            await _activityLogRepository.LogActivityAsync(activityLog);
        }

        // Tüm aktiviteleri asenkron olarak almak
        public async Task<IEnumerable<ActivityLog>> GetAllActivitiesAsync()
        {
            return await _activityLogRepository.GetAllActivitiesAsync();
        }
    }
}