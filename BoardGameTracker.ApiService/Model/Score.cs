using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoardGameTracker.ApiService.Model
{
    /// <summary>  
    /// Represents the score of multiple players in a specific game.  
    /// This is a joining entity between Player and Game, and holds the score information.  
    /// </summary>  
    public class Score : BaseModel
    {
        [Key]
        public int ScoreId { get; set; }

        public required int GameId { get; set; }
        public required Game Game { get; set; }

        public ICollection<PlayerScore> PlayerScores { get; set; } = [];
    }

    /// <summary>  
    /// Represents the score of an individual player in a game.  
    /// </summary>  
    public class PlayerScore
    {
        [Key]
        public int PlayerScoreId { get; set; }

        public required int PlayerId { get; set; }
        public required Player Player { get; set; }

        public int Points { get; set; }

        public required int ScoreId { get; set; }
        public required Score Score { get; set; }
    }
}
