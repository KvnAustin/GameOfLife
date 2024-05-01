namespace GameOfLife.UI.ViewModels
{
    public class GenerationViewModel : BaseViewModel
    {
        public int GenerationNum { get; set; }

        public bool[][] Value { get; set; } = new bool[5][];
    }
}
