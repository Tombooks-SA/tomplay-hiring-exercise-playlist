using Cloud.Models;
using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using Utils;
using Random = UnityEngine.Random;

namespace Cloud
{
    public class Database
    {
        private const int ScoresToGenerate = 1000;
        private const int PlaylistsToGenerate = 5;
        private const int StartingSongsInPlaylist = 3;

        private readonly string[] _composers =
        {
            // Original
            "Mozart", "Beethoven", "Chopin", "Bach", "Zimmer", "Williams",
            
            // Baroque & Classical
            "Vivaldi", "Handel", "Haydn", "Schubert", "Purcell", "Scarlatti", "Telemann", "Clementi",
            
            // Romantic
            "Liszt", "Schumann", "Brahms", "Tchaikovsky", "Rachmaninoff", "Mendelssohn", 
            "Wagner", "Verdi", "Puccini", "Mahler", "Strauss", "Grieg", "Dvorak", "Sibelius", 
            "Saint-Saens", "Faure", "Bizet", "Elgar", "Holst", "Rimsky-Korsakov", "Mussorgsky",
            
            // Impressionist & Modern
            "Debussy", "Ravel", "Satie", "Stravinsky", "Prokofiev", "Shostakovich", 
            "Gershwin", "Copland", "Bernstein", "Glass", "Richter", "Einaudi", "Pärt",
            
            // Film & Contemporary
            "Morricone", "Horner", "Shore", "Elfman", "Desplat", "Djawadi", "Silvestri", 
            "Giacchino", "Powell", "Gregson-Williams", "Newman", "Junkie XL", "Goransson"
        };

        private readonly string[] _titles =
        {
            // Generic Forms
            "Sonata No. 1", "Sonata No. 2", "Symphony No. 5", "Symphony No. 9", "Concerto in D", 
            "Prelude", "Etude", "Nocturne", "Waltz", "Mazurka", "Polonaise", "Minuet", "Scherzo", 
            "Rhapsody", "Ballade", "Impromptu", "Fantasia", "Fugue", "Toccata", "Canon", 
            "Overture", "Serenade", "Requiem", "Mass", "Opera Overture", "Aria", "Variations",
            
            // Named Classical Pieces
            "Moonlight", "Pathétique", "Appassionata", "Clair de Lune", "Fur Elise", 
            "Turkish March", "Wedding March", "The Four Seasons", "The Planets", "Bolero", 
            "Adagio for Strings", "Rhapsody in Blue", "The Nutcracker Suite", "Swan Lake", 
            "1812 Overture", "New World Symphony", "Ode to Joy", "Emperor Concerto", 
            "Surprise Symphony", "Fate Symphony", "Pastoral Symphony", "Heroic Polonaise", 
            "Raindrop Prelude", "Minute Waltz", "Winter Wind", "Revolutionary Etude", 
            "Liebestraum", "La Campanella", "Gymnopedie No.1", "Pavane",
            
            // Film/Thematic
            "Epic Theme", "Main Title", "Love Theme", "Battle Theme", "End Credits", 
            "Prologue", "Epilogue", "Victory Fanfare", "The Imperial March", "Hedwig's Theme",
            "Time", "Cornfield Chase", "Now We Are Free", "The Ecstasy of Gold"
        };

        private readonly string[] _keys =
        {
            "C Major", "C Minor", 
            "C# Major", "C# Minor", 
            "D Major", "D Minor", 
            "Eb Major", "Eb Minor", 
            "E Major", "E Minor", 
            "F Major", "F Minor", 
            "F# Major", "F# Minor", 
            "G Major", "G Minor", 
            "Ab Major", "Ab Minor", 
            "A Major", "A Minor", 
            "Bb Major", "Bb Minor", 
            "B Major", "B Minor"
        };

        private readonly Difficulty[] _difficulties =
        {
            Difficulty.Easy, Difficulty.Medium, Difficulty.Hard
        };
        
        public List<ScoreDto> ScoresTable { get; }
        public Dictionary<int, List<ScoreDto>> PlaylistsTable { get; }

        public Database()
        {
            ScoresTable = new List<ScoreDto>(ScoresToGenerate);
            for (int i = 0; i < ScoresToGenerate; i++)
            {
                string title = _titles.GetRandomElement();
                string composer = _composers.GetRandomElement();
                string key = _keys.GetRandomElement();
                TimeSpan duration = TimeSpan.FromSeconds(Random.Range(60f, 300f));
                Difficulty difficulty = _difficulties.GetRandomElement();

                ScoresTable.Add(new ScoreDto
                {
                    Id = i, Title = title, Composer = composer, Key = key, DurationSeconds = duration, Difficulty = difficulty
                });
            }

            PlaylistsTable = new Dictionary<int, List<ScoreDto>>(PlaylistsToGenerate);
            for (int i = 0; i < PlaylistsToGenerate; i++)
            {
                int id = Random.Range(0, int.MaxValue);
                PlaylistsTable.Add(id, ScoresTable.OrderBy(_ => Random.value).Take(StartingSongsInPlaylist).ToList());
            }
        }
    }
}