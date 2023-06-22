using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    public class ScoreWindow : MonoBehaviour
    {
        private static ScoreWindow _instance;
        private Text _scoreText;

        private void Awake()
        {
            _instance = this;
            _scoreText = transform.Find("scoreText").GetComponent<Text>();
        }

        private void Start()
        {
            Score.OnHighscoreChanged += Score_OnHighscoreChanged;
            UpdateHighscore();
        }

        private void Score_OnHighscoreChanged(object sender, System.EventArgs e) => UpdateHighscore();

        private void Update() => _scoreText.text = Score.GetScore().ToString();

        private void UpdateHighscore()
        {
            var highscore = Score.GetHighscore();
            transform.Find("highscoreText").GetComponent<Text>().text = "HIGHSCORE\n" + highscore;
        }

        public static void HideStatic() => _instance.gameObject.SetActive(false);
    }
}