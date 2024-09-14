using System.ComponentModel.DataAnnotations;

namespace MemoriesBackend.Domain.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
