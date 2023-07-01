namespace WebChatQ.Models
{
    public class UserChat
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string FromId { get; set; }
        public string ToId { get; set; }
        public bool ReadStatus { get; set; }
        public DateTime Waktu { get; set; }
    }
}
