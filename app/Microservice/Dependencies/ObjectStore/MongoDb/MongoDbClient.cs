using System;
using System.Threading;
using System.Threading.Tasks;
using Microservice.Model;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Microservice.Dependencies.ObjectStore.MongoDb
{
   public class MongoDbClient : IMongoDbClient
   {
      private const int DefaultTimeoutMs = 3000;

      private readonly IMongoDatabase _database;

      public MongoDbClient(IConfiguration configuration)
      {
         ConfigureModel();
         
         var timeout = TimeSpan.FromMilliseconds(DefaultTimeoutMs);

         var mongoUrl = new MongoUrl(configuration["MongoDb:ConnectionString"]);
         var settings = MongoClientSettings.FromUrl(mongoUrl);

         settings.ConnectTimeout = timeout;
         settings.SocketTimeout = timeout;
         settings.ServerSelectionTimeout = timeout;

         var client = new MongoClient(settings);
         _database = client.GetDatabase(mongoUrl.DatabaseName);
      }

      public async Task Ping(CancellationToken cancellationToken)
      {
         var command = new BsonDocumentCommand<BsonDocument>(new BsonDocument("ping", 1));
            
         await _database.RunCommandAsync(command, ReadPreference.Nearest, cancellationToken);
      }

      public IMongoCollection<TDocument> GetCollection<TDocument>(string name)
      {
         return _database.GetCollection<TDocument>(name);
      }
      
      private static void ConfigureModel()
      {
         BsonClassMap.RegisterClassMap<Contact>(cm => 
         {
            cm.AutoMap();
            cm.SetIgnoreExtraElements(true);
            cm.MapIdMember(c => c.ID);
            cm.MapMember(c => c.DateOfBirth).SetSerializer(new DateTimeSerializer(dateOnly: true));
         });
      }
   }
}