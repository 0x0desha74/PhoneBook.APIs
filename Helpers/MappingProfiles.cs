using AutoMapper;
using PhoneBook.APIs.DTOs;
using PhoneBook.Core.Entities;

namespace PhoneBook.APIs.Helpers
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<ContactDto, Contact>().ReverseMap();
            CreateMap<AddressDto, Address>().ReverseMap();
        }
    }
}
