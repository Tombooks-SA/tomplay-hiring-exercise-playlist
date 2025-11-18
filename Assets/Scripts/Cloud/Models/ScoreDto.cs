using Data;
using System;

namespace Cloud.Models
{
    public class ScoreDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Composer { get; set; }
        public TimeSpan DurationSeconds { get; set; }
        public Difficulty Difficulty { get; set; }
        public string Key { get; set; }
    }
}