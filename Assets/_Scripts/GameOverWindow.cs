using CodeMonkey.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    public class GameOverWindow : MonoBehaviour
    {
        private static GameOverWindow _instance;

        private void Awake()
        {
            _instance = this;
            transform.Find("retryBtn").GetComponent<Button_UI>().ClickFunc = () => { Loader.Load(Loader.Scene.GameScene); };

            Hide();
        }

        private void Show(bool isNewHighScore)
        {
            gameObject.SetActive(true);

            transform.Find("newHighscoreText").gameObject.SetActive(isNewHighScore);

            transform.Find("scoreText").GetComponent<Text>().text = Score.GetScore().ToString();
            transform.Find("highscoreText").GetComponent<Text>().text = "HIGHSCORE " + Score.GetHighscore();
        }

        private void Hide() => gameObject.SetActive(false);

        public static void ShowStatic(bool isNewHighScore) => _instance.Show(isNewHighScore);
    }
}