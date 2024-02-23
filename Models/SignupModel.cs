using System.ComponentModel.DataAnnotations;

namespace server.Models
{
    public class SignupModel
    {
        [Required]
        public required string mobile { get; set; }
        public string otp { get; set; }
        public required string name { get; set; }
        public required string age { get; set; }
        public required string email { get; set; }
        public required string gender { get; set; }

        public string department { get; set; }
    }
}