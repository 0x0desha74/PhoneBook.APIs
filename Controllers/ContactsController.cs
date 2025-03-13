using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhoneBook.APIs.DTOs;
using PhoneBook.Core.Entities;
using PhoneBook.Core.Repositories;
using PhoneBook.Core.Specifications;

namespace PhoneBook.APIs.Controllers
{
    
    public class ContactsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ContactsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        [Authorize(Roles="Admin")]
        [HttpGet("search")]
        public async Task<ActionResult<IReadOnlyList<Contact>>> Search(string? query)
        {
            var contacts = await _unitOfWork.ContactRepository.SearchAsync(query);
            return Ok(contacts);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Contact>>> GetContacts()
        {
            var spec = new ContactWithAddressSpecifications();
            var contacts = await _unitOfWork.Repository<Contact>().GetAllWithSpecAsync(spec);
            return Ok(contacts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IReadOnlyList<Contact>>> GetContact(int id)
        {
            var spec = new ContactWithAddressSpecifications(id);
            var contact = await _unitOfWork.Repository<Contact>().GetEntityWithSpecAsync(spec);
            if (contact is null) return NotFound();
            return Ok(contact);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Contact>> Create(ContactDto model)
        {
            var contact = _mapper.Map<ContactDto, Contact>(model);
            await _unitOfWork.Repository<Contact>().AddAsync(contact);
            var result = await _unitOfWork.Complete();
            if (result > 0) return Ok(contact);
            return BadRequest();
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<SuccessfullyMessageDto>> Delete(int id)
        {
            var contact = await _unitOfWork.Repository<Contact>().GetByIdAsync(id);
            _unitOfWork.Repository<Contact>().Delete(contact);
            var result = await _unitOfWork.Complete();
            if (result > 0) return Ok(new SuccessfullyMessageDto("Contact Deleted Successfully"));
            return BadRequest();
        }


        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<Contact>> Update(Contact model)
        {
            
            _unitOfWork.Repository<Contact>().Update(model);
            var result = await _unitOfWork.Complete();
            if (result > 0) return Ok(model);
            return BadRequest();
        }

    }
}
