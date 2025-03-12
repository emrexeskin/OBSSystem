namespace OBSSystem.Core.Entities
{
    public class ActivityLog
    {
        public int ActivityLogID { get; set; }
        public int UserID { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
        public string IpAddress { get; set; }

        public User User { get; set; }
    }
}