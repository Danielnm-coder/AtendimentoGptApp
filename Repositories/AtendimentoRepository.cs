using MongoDB.Driver;
using ProjetoChatGPT04_CSharp.Entities;

namespace ProjetoChatGPT04_CSharp.Repositories
{
    public class AtendimentoRepository
    {
        private IMongoDatabase _database;

        public AtendimentoRepository(IConfiguration configuration)
        {
            var settings = MongoClientSettings.FromUrl(new MongoUrl(configuration["MongoDB:Url"]));
            var client = new MongoClient(settings);

            _database = client.GetDatabase(configuration["MongoDB:Database"]);
        }

        public void Add(Atendimento atendimento)
        {
            var atendimentos = _database.GetCollection<Atendimento>("atendimentos");
            atendimentos.InsertOne(atendimento);
        }

        public List<Atendimento> Find(string emailCliente)
        {
            var atendimentos = _database.GetCollection<Atendimento>("atendimentos");

            return atendimentos.Find(a => a.EmailCliente == emailCliente)
                .SortByDescending(a => a.DataHoraAtendimento)
                .Limit(3)
                .ToList();
        }

        public List<Atendimento> FindByDatas(DateTime dataInicio, DateTime dataFim)
        {
            var atendimentos = _database.GetCollection<Atendimento>("atendimentos");

            return atendimentos.Find(a => a.DataHoraAtendimento >= dataInicio
                                       && a.DataHoraAtendimento <= dataFim)
                .ToList();
        }
    }
}



