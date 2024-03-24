using System.ComponentModel.DataAnnotations;

namespace Memories_backend.Models.DTO.Permission
{
    public class UpdatePermisionDto
    {
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
