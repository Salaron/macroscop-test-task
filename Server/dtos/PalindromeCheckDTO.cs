using System.ComponentModel.DataAnnotations;

namespace Server.dtos
{
    public class PalindromeCheckDTO
    {
        [Required]
        public string Input { get; set; }

        public bool? IgnoreSymbols { get; set; }
    }
}
