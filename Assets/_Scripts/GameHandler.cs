using UnityEngine;

namespace _Scripts
{
    public class GameHandler : MonoBehaviour
    {
        private static GameHandler _instance;

        [SerializeField] private Snake snake;

        private LevelGrid _levelGrid;

        private void Awake()
        {
            _instance = this;
            Score.InitializeStatic();
            Time.timeScale = 1f;
        }

        private void Start()
        {
            _levelGrid = new LevelGrid(20, 20);
            snake.Setup(_levelGrid);
            _levelGrid.Setup(snake);
        }

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Escape)) return;
            
            if (IsGamePaused())
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        public static void SnakeDied()
        {
            var isNewHighScore = Score.TrySetNewHighscore();
            GameOverWindow.ShowStatic(isNewHighScore);
            ScoreWindow.HideStatic();
        }

        public static void ResumeGame()
        {
            PauseWindow.HideStatic();
            Time.timeScale = 1f;
        }

        private static void PauseGame()
        {
            PauseWindow.ShowStatic();
            Time.timeScale = 0f;
        }

        private static bool IsGamePaused() => Time.timeScale == 0f;
    }
}