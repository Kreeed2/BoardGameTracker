using System.ComponentModel.DataAnnotations.Schema;

namespace BoardGameTracker.ApiService.Model
{
    public class BaseModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }

        //public required Player CreatedBy { get; set; } = null!;

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime UpdatedAt { get; set; }

        //public Player? UpdatedBy { get; set; }
    }
}
