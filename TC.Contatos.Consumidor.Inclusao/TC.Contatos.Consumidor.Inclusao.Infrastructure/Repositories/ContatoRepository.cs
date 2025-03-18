using Domain.Interfaces;
using Domain.RegionalAggregate;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ContatoRepository : IContatoRepository
    {
        protected ApplicationDbContext _context;
        protected DbSet<Contato> _dbSet;

        public ContatoRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Contato>();
        }

        public void Adicionar(Contato contato)
        {
            _dbSet.Add(contato);
            _context.SaveChanges();
        }
    }
}
