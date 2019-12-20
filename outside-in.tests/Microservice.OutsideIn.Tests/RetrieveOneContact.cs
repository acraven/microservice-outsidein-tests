using System.Net;
using System.Threading.Tasks;
using FluentAssertions.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Microservice.OutsideIn.Tests
{
   public class RetrieveOneContact : ScenarioBase
   {
      private HttpStatusCode _statusCode;
      private string _content;

      [OneTimeSetUp]
      public async Task SetupScenario()
      {
         var contact = new
         {
            firstName = "Janet",
            lastName = "Montgomery",
            dateOfBirth = "1985-10-29"
         };

         var (_, location) = await PostAsync(contact, $"contacts");

         (_statusCode, _content) = await GetAsync(location);
      }

      [Test]
      public void GetReturnsStatusCodeOkay()
      {
         Assert.That(_statusCode, Is.EqualTo(HttpStatusCode.OK));
      }

      [Test]
      public void GetReturnsContact()
      {
         var contact = JObject.Parse(_content);
         
         var expectedContact = new JObject
         {
            ["firstName"] = "Janet",
            ["lastName"] = "Montgomery",
            ["dateOfBirth"] = "1985-10-29"
         };

         contact.Should().ContainSubtree(expectedContact);
      }
   }
}