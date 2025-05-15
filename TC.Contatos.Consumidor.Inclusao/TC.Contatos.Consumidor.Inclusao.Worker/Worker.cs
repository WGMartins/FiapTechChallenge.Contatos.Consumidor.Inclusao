using Domain.Interfaces;
using Domain.RegionalAggregate;
using UseCase.ContatoUseCase.Adicionar;
using UseCase.Interfaces;

namespace Worker
{
    public class WorkerService : BackgroundService
    {
        private readonly IMessageConsumer _messageConsumer;
        private readonly IServiceScopeFactory _scopeFactory;

        public WorkerService(IMessageConsumer messageConsumer,
                      IServiceScopeFactory scopefactory)
        {
            _messageConsumer = messageConsumer;
            _scopeFactory = scopefactory;

            _messageConsumer.OnMessageReceived += ProcessarMensagem;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _messageConsumer.ConsumeAsync();
        }
        private async Task ProcessarMensagem(Contato contato)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var adicionarContatoUseCase = scope.ServiceProvider.GetRequiredService<IAdicionarContatoUseCase>();

                adicionarContatoUseCase.Adicionar(new AdicionarContatoDto
                {
                    Id = contato.Id,
                    Nome = contato.Nome,
                    Email = contato.Email,
                    Telefone = contato.Telefone,
                    RegionalId = contato.RegionalId,
                });
            }

            await Task.CompletedTask;
        }
    }
}
