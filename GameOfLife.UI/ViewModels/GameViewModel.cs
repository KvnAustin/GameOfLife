using System.ComponentModel.DataAnnotations;

namespace GameOfLife.UI.ViewModels
{
    public class GameViewModel : BaseViewModel
    {
        public bool[][] Input { get; set; } = new bool[5][];

        [Required]
        [Range(1, 5)]
        [Display(Name = "Number of Generations")]
        public int NumberOfGenerations { get; set; } = 1;

        public List<GenerationViewModel> Generations { get; set; } = new();
    }
}
