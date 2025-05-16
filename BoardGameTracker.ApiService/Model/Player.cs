using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace BoardGameTracker.ApiService.Model
{
    public class Player : BaseModel
    {
        [Key]
        public int PlayerId { get; set; }

        [MaxLength(100)]
        public required string Name { get; set; }

        //public ICollection<Score> Scores { get; } = new List<Score>();
    }
}
