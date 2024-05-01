using System.ComponentModel.DataAnnotations;

namespace GameOfLife.Models
{
    public class Generation : BaseEntity
    {
        public Guid GameId { get; set; }
        public Game Game { get; set; }

        [Required]
        public int GenerationNum { get; set; }

        [Required]
        public string Value { get; set; }
    }
}
