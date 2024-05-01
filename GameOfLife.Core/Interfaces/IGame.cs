using GameOfLife.Models;

namespace GameOfLife.Core.Interfaces
{
    public interface IGame
    {
        IEnumerable<Game> GetAll();

        Game GetById(Guid id);

        Task<Game> Save(Game game);
    }
}
