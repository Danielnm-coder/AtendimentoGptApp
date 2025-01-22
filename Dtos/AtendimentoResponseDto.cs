namespace ProjetoChatGPT04_CSharp.Dtos
{
    public class AtendimentoResponseDto
    {
        public Guid? Id { get; set; }
        public DateTime? DataHoraAtendimento { get; set; }
        public string? Resposta { get; set; }
    }
}
