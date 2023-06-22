using System;
using UnityEngine;

namespace _Scripts
{
    public class GameAssets : MonoBehaviour
    {
        public static GameAssets Instance;

        private void Awake()
        {
            Instance = this;
        }

        public Sprite snakeHeadSprite;
        public Sprite snakeBodySprite;
        public Sprite foodSprite;

        public SoundAudioClip[] soundAudioClipArray;

        [Serializable]
        public class SoundAudioClip
        {
            public SoundManager.Sound sound;
            public AudioClip audioClip;
        }
    }
}