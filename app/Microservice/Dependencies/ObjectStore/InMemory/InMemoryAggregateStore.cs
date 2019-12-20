using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microservice.Components;
using Microservice.Model;

namespace Microservice.Dependencies.ObjectStore.InMemory
{
   public class InMemoryAggregateStore : IAggregateStore
   {
      private readonly IList<IAggregate> _aggregates = new List<IAggregate>();
      private readonly ITenantAccessor _tenantAccessor;

      public InMemoryAggregateStore(ITenantAccessor tenantAccessor)
      {
         _tenantAccessor = tenantAccessor;
      }
      
      public Task AddAsync<TAggregate>(TAggregate aggregate) where TAggregate : IAggregate
      {
         aggregate.ID = Guid.NewGuid();
         aggregate.TenantID = _tenantAccessor.TenantID;

         _aggregates.Add(aggregate);

         return Task.CompletedTask;
      }

      public Task<TAggregate> GetAsync<TAggregate>(Guid id) where TAggregate : IAggregate
      {
         return Task.FromResult(ItemsOf<TAggregate>().SingleOrDefault(c => c.ID == id));
      }

      public Task<IList<TAggregate>> GetAllAsync<TAggregate>() where TAggregate : IAggregate
      {
         return Task.FromResult<IList<TAggregate>>(ItemsOf<TAggregate>().ToList());
      }
      
      private IEnumerable<TAggregate> ItemsOf<TAggregate>() where TAggregate : IAggregate
      {
         return _aggregates.OfType<TAggregate>().Where(c => c.TenantID == _tenantAccessor.TenantID);
      }
   }
}