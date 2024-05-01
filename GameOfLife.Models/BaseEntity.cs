using System.ComponentModel.DataAnnotations;

namespace GameOfLife.Models
{
    public abstract class BaseEntity
    {
        public BaseEntity()
        {
            Id = Guid.NewGuid();
            CreatedOn = DateTime.Now;
        }

        public Guid Id { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }
    }
}
