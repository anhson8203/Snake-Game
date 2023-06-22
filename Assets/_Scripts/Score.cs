using System;
using UnityEngine;

namespace _Scripts
{
    public static class Score
    {
        public static event EventHandler OnHighscoreChanged;
        private static int _score;

        public static void InitializeStatic()
        {
            OnHighscoreChanged = null;
            _score = 0;
        }

        public static int GetScore() => _score;

        public static void AddScore() => _score += 100;

        public static int GetHighscore() => PlayerPrefs.GetInt("highscore", 0);

        public static bool TrySetNewHighscore() => TrySetNewHighscore(_score);

        private static bool TrySetNewHighscore(int score)
        {
            var highscore = GetHighscore();
            
            if (score <= highscore) return false;
            
            PlayerPrefs.SetInt("highscore", score);
            PlayerPrefs.Save();
            OnHighscoreChanged?.Invoke(null, EventArgs.Empty);
            
            return true;
        }
    }
}