using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhoneBook.APIs.DTOs;
using PhoneBook.Core.Entities;
using PhoneBook.Core.Repositories;

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

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Contact>>> GetContacts()
        {
            var contacts = await _unitOfWork.Repository<Contact>().GetAllAsync();
            return Ok(contacts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IReadOnlyList<Contact>>> GetContact(int id)
        {
            var contact = await _unitOfWork.Repository<Contact>().GetByIdAsync(id);
            if (contact is null) return NotFound();
            return Ok(contact);
        }


        [HttpPost]
        public async Task<ActionResult<Contact>> Create(ContactDto model)
        {
            var contact = _mapper.Map<ContactDto, Contact>(model);
            await _unitOfWork.Repository<Contact>().AddAsync(contact);
            var result = await _unitOfWork.Complete();
            if (result > 0) return Ok(contact);
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<SuccessfullyMessageDto>> Delete(int id)
        {
            var contact = await _unitOfWork.Repository<Contact>().GetByIdAsync(id);
            _unitOfWork.Repository<Contact>().Delete(contact);
            var result = await _unitOfWork.Complete();
            if (result > 0) return Ok(new SuccessfullyMessageDto("Contact Deleted Successfully"));
            return BadRequest();
        }

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
