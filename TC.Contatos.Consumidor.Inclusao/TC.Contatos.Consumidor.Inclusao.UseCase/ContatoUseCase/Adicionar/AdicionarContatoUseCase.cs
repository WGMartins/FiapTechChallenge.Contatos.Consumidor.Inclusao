using Domain.Interfaces;
using Domain.RegionalAggregate;
using UseCase.Interfaces;

namespace UseCase.ContatoUseCase.Adicionar
{
    public class AdicionarContatoUseCase : IAdicionarContatoUseCase
    {
        private readonly IContatoRepository _contatoRepository;

        public AdicionarContatoUseCase(IContatoRepository contatoRepository)
        {
            _contatoRepository = contatoRepository;
        }

        public ContatoAdicionadoDto Adicionar(AdicionarContatoDto adicionarContatoDto)
        {
            var contato = Contato.Criar(adicionarContatoDto.Nome, adicionarContatoDto.Telefone, adicionarContatoDto.Email, adicionarContatoDto.RegionalId);

            _contatoRepository.Adicionar(contato);

            return new ContatoAdicionadoDto
            {
                Id = contato.Id,
            };
        }
    }
}
