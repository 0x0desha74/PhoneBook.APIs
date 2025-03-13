using PhoneBook.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace PhoneBook.APIs.DTOs
{
    public class ContactDto
    {
        public string Name { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public ICollection<AddressDto> Adresses { get; set; }
    }
}
