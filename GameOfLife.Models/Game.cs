using System.ComponentModel.DataAnnotations;

namespace GameOfLife.Models
{
    public class Game : BaseEntity
    {
        [Required]
        public string Input { get; set; }

        public ICollection<Generation> Generations { get; set; }
    }
}
