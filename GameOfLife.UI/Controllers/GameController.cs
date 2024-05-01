using GameOfLife.Core.Interfaces;
using GameOfLife.Models;
using GameOfLife.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Newtonsoft.Json;

namespace GameOfLife.UI.Controllers
{
    public class GameController : Controller
    {
        private readonly IGame _gameService;

        public GameController(IGame gameService)
        {
            _gameService = gameService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new GameViewModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(GameViewModel model)
        {
            if (ModelState.IsValid)
            {
                model = GetGenerationsForModel(model);
                
                var game = FromViewModel(model);
                game = await _gameService.Save(game);

                return RedirectToAction(
                    nameof(History), 
                    new { id = game.Id }
                    );
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult AllHistory()
        {
            var games = _gameService.GetAll();

            var model = ToViewModel(games);

            return View(model);
        }

        [HttpGet]
        public IActionResult History(Guid id)
        {
            var game = _gameService.GetById(id);
            if (game == null)
                return RedirectToAction(nameof(Index));

            var model = ToViewModel(game);

            return View(model);
        }

        #region [ Helper Methods ]
        private GameViewModel GetGenerationsForModel(GameViewModel model)
        {
            var value = model.Input;

            for (var i = 0; i < model.NumberOfGenerations; i++)
            {
                value = GetNextGeneration(value);

                model.Generations.Add(new GenerationViewModel
                {
                    GenerationNum = i + 1,
                    Value = value
                });
            }

            return model;
        }

        public bool[][] GetNextGeneration(bool[][] currentGeneration)
        {
            var nextGeneration = new bool[5][];

            for (var i = 0; i < 5; i++)
            {
                nextGeneration[i] = new bool[5];

                for (var j = 0; j < 5; j++)
                {
                    var neighborsNum = 0;
                    
                    neighborsNum += IsAlive(currentGeneration, i - 1, j - 1); // northwest
                    neighborsNum += IsAlive(currentGeneration, i    , j - 1); // north
                    neighborsNum += IsAlive(currentGeneration, i + 1, j - 1); // northeast
                    
                    neighborsNum += IsAlive(currentGeneration, i - 1, j); // west
                    neighborsNum += IsAlive(currentGeneration, i + 1, j); // east

                    neighborsNum += IsAlive(currentGeneration, i - 1, j + 1); // southwest
                    neighborsNum += IsAlive(currentGeneration, i    , j + 1); // south
                    neighborsNum += IsAlive(currentGeneration, i + 1, j + 1); // southeast

                    var current = currentGeneration[i][j];

                    nextGeneration[i][j] = (current, neighborsNum) switch
                    {
                        (true, < 2) => false,
                        (true, 2) => true,
                        (true, 3) => true,
                        (true, > 3) => false,
                        (false, 3) => true,
                        _ => false
                    };
                }
            }

            return nextGeneration;
        }

        private int IsAlive(bool[][] generation, int indexA, int indexB)
        {
            try
            {
                return generation[indexA][indexB] ? 1 : 0;
            }
            catch (IndexOutOfRangeException)
            {
                return 0;
            }
        }
        #endregion

        #region [ Mapper Methods ]
        private Game FromViewModel(GameViewModel model)
            => new Game
            {
                Id = model.Id,
                Input = JsonConvert.SerializeObject(model.Input),
                Generations = FromViewModels(model.Generations),
                CreatedOn = model.CreatedOn
            };

        private List<Generation> FromViewModels(IEnumerable<GenerationViewModel> models)
            => (models ?? Enumerable.Empty<GenerationViewModel>())
                .Select(FromViewModel)
                .ToList();

        private Generation FromViewModel(GenerationViewModel model)
            => new Generation
            {
                Id = model.Id,
                GenerationNum = model.GenerationNum,
                Value = JsonConvert.SerializeObject(model.Value),
                CreatedOn = model.CreatedOn
            };

        private IEnumerable<GameViewModel> ToViewModel(IEnumerable<Game> games)
            => (games ?? Enumerable.Empty<Game>())
                .Select(ToViewModel)
                .OrderBy(x => x.CreatedOn)
                .ToList();

        private GameViewModel ToViewModel(Game model)
            => new GameViewModel
            {
                Id = model.Id,
                Input = JsonConvert.DeserializeObject<bool[][]>(model.Input),
                Generations = ToViewModels(model.Generations),
                CreatedOn = model.CreatedOn
            };

        private List<GenerationViewModel> ToViewModels(ICollection<Generation> models)
            => (models ?? Enumerable.Empty<Generation>())
                .Select(ToViewModel)
                .OrderBy(x => x.GenerationNum)
                .ToList();

        private GenerationViewModel ToViewModel(Generation model)
            => new GenerationViewModel
            {
                Id = model.Id,
                GenerationNum = model.GenerationNum,
                Value = JsonConvert.DeserializeObject<bool[][]>(model.Value),
                CreatedOn = model.CreatedOn
            };
        #endregion
    }
}
