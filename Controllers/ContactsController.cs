using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhoneBook.Core.Entities;
using PhoneBook.Core.Repositories;

namespace PhoneBook.APIs.Controllers
{
    
    public class ContactsController : BaseApiController
    {
        private readonly IGenericRepository<Contact> _contactRepo;

        public ContactsController(IGenericRepository<Contact> contactRepo)
        {
            _contactRepo = contactRepo;
        }

        //[HttpPost]
        //public async Task<ActionResult<Contact>> Create(Contact model)
        //{

        //}
    }
}
