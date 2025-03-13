using System.ComponentModel.DataAnnotations;

namespace PhoneBook.APIs.DTOs
{
    public class AssignRoleDto: BaseRoleDto
    {

        [Required]
        public string Role { get; set; }
    }
}
