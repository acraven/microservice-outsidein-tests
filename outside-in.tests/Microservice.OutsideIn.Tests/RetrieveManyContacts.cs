using System.Net;
using System.Threading.Tasks;
using FluentAssertions.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Microservice.OutsideIn.Tests
{
   public class RetrieveManyContacts : ScenarioBase
   {
      private HttpStatusCode _statusCode;
      private string _content;

      [OneTimeSetUp]
      public async Task SetupScenario()
      {
         var contact1 = new
         {
            firstName = "Freema",
            lastName = "Agyeman",
            dateOfBirth = "1979-03-20"
         };

         var contact2 = new
         {
            firstName = "Jocko",
            lastName = "Sims",
            dateOfBirth = "1981-02-20"
         };

         var contact3 = new
         {
            firstName = "Tyler",
            lastName = "Labine",
            dateOfBirth = "1978-04-29"
         };

         await PostAsync(contact1, "contacts");
         await PostAsync(contact2, "contacts");
         await PostAsync(contact3, "contacts");

         (_statusCode, _content) = await GetAsync("contacts");
      }

      [Test]
      public void GetReturnsStatusCodeOkay()
      {
         Assert.That(_statusCode, Is.EqualTo(HttpStatusCode.OK));
      }

      [Test]
      public void GetReturnsManyContacts()
      {
         var contacts = JArray.Parse(_content);

         var expectedContacts = new JArray
         {
            new JObject
            {
               ["firstName"] = "Freema",
               ["lastName"] = "Agyeman",
               ["dateOfBirth"] = "1979-03-20"
            },
            new JObject
            {
               ["firstName"] = "Tyler",
               ["lastName"] = "Labine",
               ["dateOfBirth"] = "1978-04-29"
            },
            new JObject
            {
               ["firstName"] = "Jocko",
               ["lastName"] = "Sims",
               ["dateOfBirth"] = "1981-02-20"
            }
         };

         contacts.Should().ContainSubtree(expectedContacts);
      }
   }
}