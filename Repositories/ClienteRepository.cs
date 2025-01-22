using MongoDB.Driver;
using ProjetoChatGPT04_CSharp.Entities;

namespace ProjetoChatGPT04_CSharp.Repositories
{
    public class ClienteRepository
    {
        private IMongoDatabase _database;

        public ClienteRepository(IConfiguration configuration)
        {
            var settings = MongoClientSettings.FromUrl(new MongoUrl(configuration["MongoDB:Url"]));
            var client = new MongoClient(settings);

            _database = client.GetDatabase(configuration["MongoDB:Database"]);
        }

        public void Add(Cliente cliente)
        {
            var clientes = _database.GetCollection<Cliente>("clientes");
            clientes.InsertOne(cliente);
        }

        public Cliente? Find(string email)
        {
            var clientes = _database.GetCollection<Cliente>("clientes");
            return clientes.Find(c => c.Email.Equals(email)).FirstOrDefault();
        }
    }
}
