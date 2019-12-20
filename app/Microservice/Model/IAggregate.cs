using System;

namespace Microservice.Model
{
   public interface IAggregate
   {
      Guid ID { get; set; }
      
      string TenantID { get; set; }
   }
}