using Cloud;
using Cloud.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Example
{
    /// <summary>
    /// Note: This is a simple example of how to use the ScoreService.
    ///
    /// This is NOT production-ready code.
    /// Only used to demonstrate the usage of the ScoreService.
    /// 
    /// This is not how you should structure your code in a real project.
    /// Consider using proper architecture patterns (e.g., MVVM, MVC, etc.)
    /// </summary>
    public class ScoreServiceUsageExample : MonoBehaviour
    {
        private string _shortcutMessage;
        private string _statusMessage;
        
        private readonly Vector3 _shortcutInfoPosition = new(10f, 10f, 0f);
        private readonly Vector3 _statusInfoPosition = new(10f, 50f, 0f);

        private void Start()
        {
            _shortcutMessage = "Press Space to fetch 'Mozart' scores...";
            _shortcutMessage += "\nPress P to fetch user playlist...";
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                FetchScoresFromCatalogue().ContinueWith(task => 
                {
                    if (task.Exception != null)
                    {
                        _statusMessage = "Error fetching scores: " + task.Exception.Message;
                    }
                });
            }
            
            if (Input.GetKeyDown(KeyCode.P))
            {
                FetchAndUpdatePlaylistFromServer().ContinueWith(task => 
                {
                    if (task.Exception != null)
                    {
                        _statusMessage = "Error fetching/updating playlist: " + task.Exception.Message;
                    }
                });
            }
        }

        private async Task FetchAndUpdatePlaylistFromServer()
        {
            _statusMessage = "Loading playlists...";
                
            List<int> playlistIds = await ScoreService.Instance.GetUserPlaylistIds();
            List<ScoreDto> scores = await ScoreService.Instance.GetUserPlaylist(playlistIds[0]);
                
            _statusMessage = $"Success! Playlist contains {scores.Count} scores.\n";
            foreach (ScoreDto score in scores)
            {
                _statusMessage += $"\n  {score.Title} by {score.Composer} [{score.Difficulty}]";
            }
            
            await ScoreService.Instance.UpdatePlaylist(playlistIds[0], scores.Select(score => score.Id).ToList());
            
            _statusMessage += "\n\nPlaylist updated successfully.";
        }

        private async Task FetchScoresFromCatalogue()
        {
            _statusMessage = "Loading...";
                
            List<ScoreDto> scores = await ScoreService.Instance.FetchScoresFromCatalogue("Mozart");
                
            _statusMessage = $"Success! Found {scores.Count} scores.\n";
            foreach (ScoreDto score in scores)
            {
                _statusMessage += $"\n  {score.Title} by {score.Composer} [{score.Difficulty}]";
            }
        }

        // Simple debug UI to visualize the result
        private void OnGUI()
        {
            GUI.matrix = Matrix4x4.TRS(_shortcutInfoPosition, Quaternion.identity, Vector3.one * 3);
            GUILayout.Label(_shortcutMessage);
            
            GUI.matrix = Matrix4x4.TRS(_statusInfoPosition, Quaternion.identity, Vector3.one * 3);
            GUILayout.Label(_statusMessage);
        }
    }
}