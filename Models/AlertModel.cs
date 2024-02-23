namespace server.Models
{
    public class AlertModel
    {
        public int id { get; set; }
        public required string title { get; set; }
        public required string message { get; set; }
    }
}