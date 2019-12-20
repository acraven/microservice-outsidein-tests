using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Microservice.OutsideIn.Tests
{
   public class CreateContact : ScenarioBase
   {
      private HttpStatusCode _statusCode;
      private string _location;

      [OneTimeSetUp]
      public async Task SetupScenario()
      {
         var contact = new
         {
            firstName = "Ryan",
            lastName = "Eggold",
            dateOfBirth = "1984-08-10"
         };

         (_statusCode, _location) = await PostAsync(contact, $"contacts");
      }

      [Test]
      public void PostReturnsStatusCodeCreated()
      {
         Assert.That(_statusCode, Is.EqualTo(HttpStatusCode.Created));
      }

      [Test]
      public void PostReturnsLocationOfContact()
      {
         Assert.That(_location, Does.Match($"{BaseUri}contacts/{GuidRegex}"));
      }
   }
}