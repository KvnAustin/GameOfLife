using GameOfLife.Core.Interfaces;
using GameOfLife.Models;
using GameOfLife.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace GameOfLife.Core.Services
{
    public class GameService : IGame
    {
        private readonly ApplicationDbContext _context;

        public GameService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Game> GetAll()
            => Query()
                .ToList();

        public Game GetById(Guid id)
            => Query()
                .FirstOrDefault(x => x.Id == id);

        public async Task<Game> Save(Game game)
        {
            _context.Add(game);
            await _context.SaveChangesAsync();

            return game;
        }

        #region [ Helper Methods ]

        private IQueryable<Game> Query()
            => _context.Games
                .Include(x => x.Generations);

        #endregion
    }
}
