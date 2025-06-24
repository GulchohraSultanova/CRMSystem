namespace CRMSystem.Application.Dtos.Notification
{
    public class AdminNotificationDto
    {
        public string Id { get; set; }
        public string Type { get; set; }   // "create", "update" və ya "delete"
        public string? CategoryId { get; set; }   // Kategoriya ilə bağlıdırsa ID, yoxdursa null
        public string? ProductId { get; set; }   // Məhsulla bağlıdırsa ID, yoxdursa null
        public bool IsRead { get; set; }
        public string CreatedDate { get; set; }   // İndi string
    }
}
