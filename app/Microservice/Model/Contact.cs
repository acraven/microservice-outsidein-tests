using System;

namespace Microservice.Model
{
   public class Contact : IAggregate
   {
      public Guid ID { get; set; }

      public string TenantID { get; set; }

      public string FirstName { get; set; }

      public string LastName { get; set; }

      public DateTime DateOfBirth { get; set; }
   }
}