using Example.Core.Services;
using UnityEngine;

namespace Example.Audio
{
    /// <summary>
    /// AudioService manages different audio sources for music, sound effects (SFX), UI sounds, and ambience.
    /// </summary>
    public class AudioService : MonoBehaviour, IGameService
    {
        [SerializeField] private SoundOrigin _music;
        [SerializeField] private SoundOrigin _sfx;
        [SerializeField] private SoundOrigin _ui;
        [SerializeField] private SoundOrigin _ambience;
		
        public void PlayMusic(string clip)
        {
            _music.Play(clip);
        }

        public void PlaySFX(string clip)
        {
            _sfx.Play(clip);
        }

        public void PlayUI(string clip)
        {
            _ui.Play(clip);
        }
        
        public void PlayAmbience(string clip)
        {
            _ambience.Play(clip);
        }
    }
}