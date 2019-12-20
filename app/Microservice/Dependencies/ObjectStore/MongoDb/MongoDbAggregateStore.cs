using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microservice.Components;
using Microservice.Model;
using MongoDB.Driver;

namespace Microservice.Dependencies.ObjectStore.MongoDb
{
   public class MongoDbAggregateStore : IAggregateStore
   {
      private readonly IMongoDbClient _mongoDbClient;
      private readonly ITenantAccessor _tenantAccessor;

      public MongoDbAggregateStore(
         IMongoDbClient mongoDbClient,
         ITenantAccessor tenantAccessor)
      {
         _mongoDbClient = mongoDbClient;
         _tenantAccessor = tenantAccessor;
      }
      
      public async Task AddAsync<TAggregate>(TAggregate aggregate) where TAggregate : IAggregate
      {
         aggregate.ID = Guid.NewGuid();
         aggregate.TenantID = _tenantAccessor.TenantID;

         var collection = _mongoDbClient.GetCollection<TAggregate>(typeof(TAggregate).Name);
         
         await collection.InsertOneAsync(aggregate);
      }

      public async Task<TAggregate> GetAsync<TAggregate>(Guid id) where TAggregate : IAggregate
      {
         var collection = _mongoDbClient.GetCollection<TAggregate>(typeof(TAggregate).Name);
         
         var aggregate = await collection.Find(c => c.ID == id && c.TenantID == _tenantAccessor.TenantID).SingleOrDefaultAsync();
         return aggregate;
      }

      public async Task<IList<TAggregate>> GetAllAsync<TAggregate>() where TAggregate : IAggregate
      {
         var collection = _mongoDbClient.GetCollection<TAggregate>(typeof(TAggregate).Name);
         var cursor = await collection.FindAsync(c => c.TenantID == _tenantAccessor.TenantID);
            
         var aggregates = cursor.ToEnumerable().ToArray();
         return aggregates;
      }
   }
}