namespace server.Models
{
    public class SigninModel
    {
        public required string mobile { get; set; }
        public required string otp { get; set; }
        public required string fireBaseId { get; set; }
    }
}