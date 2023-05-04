using ContactsAPI.Data;
using ContactsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : Controller
    {
        private readonly ContactsAPIDbContext _context;

        public ContactsController(ContactsAPIDbContext contactsAPIDbContext)
        {
            this._context=contactsAPIDbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetContacts()
        {
            return Ok(await _context.Contacts.ToListAsync());
        }
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetContact([FromRoute] int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact != null)
            {
                return Ok(contact);
            }
            return NotFound();

        }

        [HttpPost]
        public async Task<IActionResult> AddContacts(AddContactRequest addContactRequest)
        {
            var contact = new Contact()
            {
                FullName = addContactRequest.FullName,
                Email = addContactRequest.Email,
                Phone = addContactRequest.Phone,
                Address = addContactRequest.Address,
            };
            await _context.Contacts.AddAsync(contact);
            await _context.SaveChangesAsync();
            return Ok(contact);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateContacts([FromRoute] int id, UpdateContactRequest updateContactRequest)
        {
            var contact = await _context.Contacts.FindAsync(id);

            if (contact != null)
            {

                contact.FullName = updateContactRequest.FullName;
                contact.Email = updateContactRequest.Email;
                contact.Phone = updateContactRequest.Phone;
                contact.Address = updateContactRequest.Address;
                await _context.SaveChangesAsync();
                return Ok(contact);
            }
            return NotFound();



        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteContact([FromRoute] int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact != null)
            {
                _context.Contacts.Remove(contact);
                await _context.SaveChangesAsync();
                return Ok(contact);
            }
            return NotFound();

        }
    }
}
