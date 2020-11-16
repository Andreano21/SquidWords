using System.ComponentModel.DataAnnotations;

namespace SquidWords.Models.Accounts
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}