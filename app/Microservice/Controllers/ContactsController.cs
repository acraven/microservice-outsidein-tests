using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microservice.Controllers.Model;
using Microservice.Dependencies.ObjectStore;
using Microservice.Model;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Controllers
{
   [ApiController]
   [Route("contacts")]
   public class ContactsController : Controller
   {
      private readonly IAggregateStore _aggregateStore;

      public ContactsController(IAggregateStore aggregateStore)
      {
         _aggregateStore = aggregateStore;
      }

      [HttpPost]
      public async Task<ActionResult> PostAsync([FromBody, Required] ContactDto contactDto)
      {
         var contact = new Contact
         {
            FirstName = contactDto.FirstName,
            LastName = contactDto.LastName,
            DateOfBirth = DateTime.ParseExact(contactDto.DateOfBirth, "yyyy-MM-dd", CultureInfo.InvariantCulture)
         };

         await _aggregateStore.AddAsync(contact);

         return Created($"{Request.Scheme}://{Request.Host}/contacts/{contact.ID}", null);
      }
      
      [HttpGet("{contactId}")]
      public async Task<ActionResult> GetAsync([FromRoute] Guid contactId)
      {
         var contact = await _aggregateStore.GetAsync<Contact>(contactId);

         return new JsonResult(MapContact(contact));
      }
      
      [HttpGet]
      public async Task<ActionResult> GetManyAsync()
      {
         var contacts = await _aggregateStore.GetAllAsync<Contact>();

         return new JsonResult(contacts.Select(MapContact).ToArray());
      }

      private static ContactDto MapContact(Contact contact)
      {
         if (contact == null)
         {
            return null;
         }
         
         return new ContactDto
         {
            FirstName = contact.FirstName,
            LastName = contact.LastName,
            DateOfBirth = contact.DateOfBirth.ToString("yyyy-MM-dd")
         };
      }
   }
}