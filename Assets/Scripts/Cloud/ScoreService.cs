using Cloud.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Cloud
{
    public class ScoreService
    {
        private const int ItemsToRetrieve = 50;

        
        private Database Database { get; } = new();

        private static ScoreService instance;
        public static ScoreService Instance => instance ??= new ScoreService();

        /// <summary>
        ///     Retrieves the IDs of the user's playlists.
        /// </summary>
        /// <returns></returns>
        public async Task<List<int>> GetUserPlaylistIds(CancellationToken cancellationToken = default)
        {
            await TalkToServer(cancellationToken);

            return Database.PlaylistsTable.Keys.ToList();
        }

        /// <summary>
        ///     Retrieves the user's playlist by ID.
        /// </summary>
        /// <param name="playlistId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<ScoreDto>> GetUserPlaylist(int playlistId, CancellationToken cancellationToken = default)
        {
            await TalkToServer(cancellationToken);

            return Database.PlaylistsTable[playlistId];
        }

        /// <summary>
        ///     Returns a list of scores from the database, optionally filtered by title or composer.
        ///     Only returns up to <see cref="ItemsToRetrieve" /> items. This simulates paging/ranking from a server.
        ///     Do not implement paging on your side; This is to encourage the use of filters and to avoid UI overload.
        ///     Expect this to take some time and occasionally fail to simulate real-world network conditions.
        ///     Throws an exception on failure.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<List<ScoreDto>> FetchScoresFromCatalogue(string filter = "", CancellationToken cancellationToken = default)
        {
            await TalkToServer(cancellationToken);

            return Database.ScoresTable
                           .Where(score => score.Title.Contains(filter, StringComparison.OrdinalIgnoreCase)
                                           || score.Composer.Contains(filter, StringComparison.OrdinalIgnoreCase))
                           .Take(ItemsToRetrieve)
                           .ToList();
        }

        /// <summary>
        ///     Updates the user's playlist with the provided score IDs.
        ///     Expect this to take some time and occasionally fail to simulate real-world network conditions.
        ///     Throws an exception on failure.
        /// </summary>
        /// <param name="playlistId"></param>
        /// <param name="scoreIds"></param>
        /// <param name="cancellationToken"></param>
        public async Task UpdatePlaylist(int playlistId, List<int> scoreIds, CancellationToken cancellationToken = default)
        {
            await TalkToServer(cancellationToken);
            
            if (Database.PlaylistsTable.ContainsKey(playlistId) == false)
            {
                throw new ArgumentException($"404 Not Found: Playlist {playlistId} does not exist.");
            }

            Debug.Log($"Playlist {playlistId} updated with Score IDs: {string.Join(", ", scoreIds)}");
        }

        private static async Task TalkToServer(CancellationToken cancellationToken = default)
        {
            // Simulate Network Latency
            await Task.Delay(Random.Range(500, 1500), cancellationToken);

            // Simulate Random Network Failure (20% chance)
            if (Random.value < 0.2f)
            {
                throw new TimeoutException("500 Internal Server Error: Connection dropped.");
            }
        }

        public ScoreService()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
#endif
        }
        
        
#if UNITY_EDITOR
        private static void OnPlayModeStateChanged(UnityEditor.PlayModeStateChange state)
        {
            if (state is UnityEditor.PlayModeStateChange.ExitingPlayMode or UnityEditor.PlayModeStateChange.ExitingEditMode)
            {
                instance = null;
            }
        }
#endif
    }
}