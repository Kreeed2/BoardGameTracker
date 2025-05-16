using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoardGameTracker.ApiService.Model
{
    // Represents the score of a player in a specific game.  This is a joining entity
    // between Player and Game, and holds the score information.
    public class Score : BaseModel
    {
        [Key]
        public int ScoreId { get; set; }

        public required int PlayerId { get; set; }
        public required int GameId { get; set; }

        public required Player Player { get; set; }
        public required Game Game { get; set; }

        public int Points { get; set; }
    }
}
