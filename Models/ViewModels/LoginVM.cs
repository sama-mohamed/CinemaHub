using System.ComponentModel.DataAnnotations;

namespace CinemaWebSite.Models.ViewModels
{
    public class LoginVM
    {
        public int id { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }

    }
}
