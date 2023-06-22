using System;
using UnityEngine.SceneManagement;

namespace _Scripts
{
    public static class Loader
    {
        public enum Scene
        {
            GameScene,
            Loading,
            MainMenu
        }

        private static Action _loaderCallbackAction;

        public static void Load(Scene scene)
        {
            _loaderCallbackAction = () =>
            {
                SceneManager.LoadScene(scene.ToString());
            };
            
            SceneManager.LoadScene(Scene.Loading.ToString());
        }

        public static void LoaderCallback()
        {
            if (_loaderCallbackAction == null) return;
            
            _loaderCallbackAction();
            _loaderCallbackAction = null;
        }
    }
}