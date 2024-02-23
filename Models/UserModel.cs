namespace server.Models
{
    public class UserModel
    {
        public required string name { get; set; }
        public required string email { get; set; }
        public required int age { get; set; }
        public required string gender { get; set; }
        public string adhaar { get; set; }
    }
}