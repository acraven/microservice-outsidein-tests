using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Microservice.Dependencies.ObjectStore.MongoDb
{
   public interface IMongoDbClient
   {
      Task Ping(CancellationToken cancellationToken);

      IMongoCollection<TDocument> GetCollection<TDocument>(string name);
   }
}