using System.Threading.Tasks;
using Core.Base.Classes;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Scriptables.Holders;
using Scriptables.Settings;
using UnityEngine;
using Zenject;

namespace Core.Infrastructure.Services
{
    public class SoundService : BaseService
    {
        private AudioSource _audioSourceMusic;
        private AudioSource _audioSourceButtonBubble;
        private AudioSource _audioSourceMenu;
        private AudioSource _audioSourceGame;
        
        private ScriptableAudioSettings _audioSettings;
        private ScriptableSoundsHolder _sounds;

        [Inject]
        private void Construct(Camera camera, ScriptableAudioSettings audioSettings, ScriptableSoundsHolder scriptableSoundsHolder)
        {
            _audioSettings = audioSettings;
            _sounds = scriptableSoundsHolder;
            
            _audioSourceMusic = camera.gameObject.AddComponent<AudioSource>();
            _audioSourceButtonBubble = camera.gameObject.AddComponent<AudioSource>();
            _audioSourceMenu = camera.gameObject.AddComponent<AudioSource>();
            _audioSourceGame = camera.gameObject.AddComponent<AudioSource>();
        }
        
        public override void Initialize()
        {
            // throw new System.NotImplementedException();
        }
        
        public void SetMusic(AudioClip audioClip)
        {
            if(audioClip == _audioSourceMusic.clip)
            {
                return;
            }

            var currentVolume = _audioSourceMusic.volume;
            Sequence sequence = DOTween.Sequence();
            sequence.Join(_audioSourceMusic.DOFade(0, _audioSettings.audioChangeDuration).OnComplete(() =>
            {
                _audioSourceMusic.clip = audioClip;
            }));
            sequence.Append(_audioSourceMusic.DOFade(currentVolume, _audioSettings.audioChangeDuration).OnComplete(_audioSourceMusic.Play));
            sequence.Play();
        }
        public void PlayMainThemeMusic()
        {
            SetMusic(_sounds.mainThemeAudioClip);
        }
        public void SetMusicVolume(float volume)
        {
            _audioSourceMusic.volume = volume;
        }
        
        public void SetSoundVolume(float volume)
        {
            _audioSourceButtonBubble.volume = volume;
            _audioSourceMenu.volume = volume;
            _audioSourceGame.volume = volume;
        }
        private void PlayButtonBubbleSound(AudioClip audioClip)
        {
            _audioSourceButtonBubble.clip = audioClip;
            _audioSourceButtonBubble.Play();
        }
        public void PlayButtonSound()
        {
            PlayButtonBubbleSound(_sounds.buttonAudioClip);
        }
        public void PlayBubbleSound()
        {
            PlayButtonBubbleSound(_sounds.bubbleAudioClip);
        }
        public void PlayMenuSound(MenuSound menuSound)
        {
            switch (menuSound)
            {
                case MenuSound.BigTreeIntro:
                {
                    _audioSourceMenu.clip = _sounds.menuBigTreeIntro;
                    break;
                }
                case MenuSound.BigTreeOutro:
                {
                    _audioSourceMenu.clip = _sounds.menuBigTreeOutro;
                    break;
                }
                case MenuSound.SmallTreeIntro:
                {
                    _audioSourceMenu.clip = _sounds.menuSmallTreeIntro;
                    break;
                }
                case MenuSound.SmallTreeOutro:
                {
                    _audioSourceMenu.clip = _sounds.menuSmallTreeOutro;
                    break;
                }
                case MenuSound.MenuOutro:
                {
                    _audioSourceMenu.clip = _sounds.menuOutro;
                    break;
                }
            }
            _audioSourceMenu.Play();
        }
        public void PlayGameSound(AudioClip audioClip)
        {
            _audioSourceGame.clip = audioClip;
            _audioSourceGame.Play();
        }
    }

    public enum MenuSound
    {
        BigTreeIntro,
        BigTreeOutro,
        SmallTreeIntro,
        SmallTreeOutro,
        MenuOutro
    }
}