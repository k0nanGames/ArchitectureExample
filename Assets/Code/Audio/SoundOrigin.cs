using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Example.Audio
{
    [Serializable]
    public class SoundOrigin
    {
        [SerializeField] private AudioSource _source;
        [SerializeField] private List<SoundData> _sounds;
        
        public void Play(string clipName)
        {
            SoundData soundData = _sounds.FirstOrDefault(sound => sound.GetClipName() == clipName);
            if(soundData == null)
            {
                Debug.LogError($"Sound clip not found: {clipName}");
                return;
            }

            AudioClip nextClip = soundData.GetClip(out float volume, out float pitch);;
            if (soundData.CheckIfPlaying())
            {
                if (_source.isPlaying && _source.clip.name == nextClip.name)
                    return;
            }
            
            _source.Stop();
            
            _source.clip = nextClip;
            _source.loop = soundData.IsLooping();
            _source.volume = volume;
            _source.pitch = pitch;
            
            _source.Play();
        }
    }
}