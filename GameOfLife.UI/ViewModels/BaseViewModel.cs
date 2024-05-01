namespace GameOfLife.UI.ViewModels
{
    public class BaseViewModel
    {
        public BaseViewModel()
        {
            Id = Guid.NewGuid();
            CreatedOn = DateTime.Now;
        }

        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
