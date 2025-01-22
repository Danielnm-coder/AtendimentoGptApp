using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ProjetoChatGPT04_CSharp.Entities
{
    public class Cliente
    {
        [BsonRepresentation(BsonType.String)]
        public Guid? Id { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Informacoes { get; set; }
    }
}
