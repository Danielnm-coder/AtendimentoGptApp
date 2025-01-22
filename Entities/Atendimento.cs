using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ProjetoChatGPT04_CSharp.Entities
{
    public class Atendimento
    {
        [BsonRepresentation(BsonType.String)]
        public Guid? Id { get; set; }
        public string? EmailCliente { get; set; }
        public string? Role { get; set; }
        public DateTime? DataHoraAtendimento { get; set; }
        public string? Conversa { get; set; }
    }
}
