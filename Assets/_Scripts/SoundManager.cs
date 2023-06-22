using CodeMonkey.Utils;
using UnityEngine;

namespace _Scripts
{
    public static class SoundManager
    {
        public enum Sound
        {
            SnakeMove,
            SnakeDie,
            SnakeEat,
            ButtonClick,
            ButtonOver,
        }

        public static void PlaySound(Sound sound)
        {
            var soundGameObject = new GameObject("Sound");
            var audioSource = soundGameObject.AddComponent<AudioSource>();
            audioSource.PlayOneShot(GetAudioClip(sound));
        }

        private static AudioClip GetAudioClip(Sound sound)
        {
            foreach (var soundAudioClip in GameAssets.Instance.soundAudioClipArray)
            {
                if (soundAudioClip.sound == sound)
                {
                    return soundAudioClip.audioClip;
                }
            }

            Debug.LogError("Sound " + sound + " not found!");
            return null;
        }

        public static void AddButtonSounds(this Button_UI buttonUI)
        {
            buttonUI.MouseOverOnceFunc += () => PlaySound(Sound.ButtonOver);
            buttonUI.ClickFunc += () => PlaySound(Sound.ButtonClick);
        }
    }
}