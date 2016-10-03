using MongoDB.Bson;

namespace MvcApplication.DAL
{
    internal class MongoDbArticle : Article
    {
        public BsonObjectId _id { get; set; }
    }
}