﻿using System.ComponentModel.DataAnnotations;

namespace BoardGameTracker.ApiService.Model
{
    public class Game : BaseModel
    {
        [Key]
        public int GameId { get; set; }

        [MaxLength(100)]
        public required string Name { get; set; }

        public ICollection<Score> Scores { get; } = [];
    }
}
