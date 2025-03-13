namespace PhoneBook.APIs.DTOs
{
    public class RoleDto:BaseRoleDto
    {
        public IEnumerable<string> Roles { get; set; }
    }
}
