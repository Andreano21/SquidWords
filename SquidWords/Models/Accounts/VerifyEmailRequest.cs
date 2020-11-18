using System.ComponentModel.DataAnnotations;

namespace SquidWords.Models.Accounts
{
    public class VerifyEmailRequest
    {
        [Required]
        public string Token { get; set; }
    }
}