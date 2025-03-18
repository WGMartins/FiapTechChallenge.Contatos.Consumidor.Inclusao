namespace UseCase.ContatoUseCase.Adicionar
{
    public class AdicionarContatoDto
    {
        public Guid Id { get; set; }
        public required string Nome { get; set; }
        public required string Telefone { get; set; }
        public required string Email { get; set; }
        public Guid RegionalId { get; set; }
    }
}
