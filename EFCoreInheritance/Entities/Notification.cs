using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCoreInheritance.Entities
{
    public class Notification
    {
        public int Id { get; set; }                              
        public string Message { get; set; } = null!;
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public bool IsDelivered { get; set; }
        public int UserId { get; set; }                          
        public string Priority { get; set; } = "Normal";
        public string? TemplateId { get; set; }                  
        public string? SentBy { get; set; }                      
        public int RetryCount { get; set; } = 0;
        public string? FailureReason { get; set; }
        public string? ResponseCode { get; set; }
    }
    public class EmailNotification : Notification
    {
        public string RecipientEmail { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string? Cc { get; set; }
        public string? Bcc { get; set; }
        public bool IsHtml { get; set; }
    }
    public class SmsNotification : Notification
    {
        public string RecipientNumber { get; set; } = null!;
        public string SenderId { get; set; } = null!;
        public string Provider { get; set; } = null!;
    }
    public class PushNotification : Notification
    {
        public string DeviceToken { get; set; } = null!;
        public string AppPlatform { get; set; } = null!;
        public string? AppVersion { get; set; }
    }
}