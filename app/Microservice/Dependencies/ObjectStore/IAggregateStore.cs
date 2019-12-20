using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microservice.Model;

namespace Microservice.Dependencies.ObjectStore
{
   public interface IAggregateStore
   {
      Task AddAsync<TAggregate>(TAggregate aggregate) where TAggregate : IAggregate;

      Task<TAggregate> GetAsync<TAggregate>(Guid id) where TAggregate : IAggregate;
      
      Task<IList<TAggregate>> GetAllAsync<TAggregate>() where TAggregate : IAggregate;
   }
}