using System.ComponentModel.DataAnnotations;

namespace SquidWords.Models.Accounts
{
    public class ValidateResetTokenRequest
    {
        [Required]
        public string Token { get; set; }
    }
}