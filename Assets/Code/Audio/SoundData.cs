using System.Collections.Generic;
using UnityEngine;

namespace Example.Audio
{
    [CreateAssetMenu(fileName = "SoundData", menuName = "ScriptableObjects/SoundData", order = 1)]
    public class SoundData : ScriptableObject
    {
        [SerializeField] private string _clipName;
        [SerializeField] private List<AudioClip> _audioClips;
        
        [Header("Volume and pitch")] [Space(10)]
        [SerializeField] [Range(0.0f, 1.0f)] private float _volume = 1f;
        [SerializeField] [Range(0.0f, 2.0f)] private float _pitch;
        
        [Header("Looping and playing")] [Space(10)]
        [SerializeField] private bool _isLooping;
        [SerializeField] private bool _checkIfPlaying;

        [Header("Volume and pitch randomization")] [Space(10)] [SerializeField]
        private bool _randomize;

        [SerializeField] [Range(0.0f, 3.0f)] private float _randomVolumeMin;
        [SerializeField] [Range(0.0f, 3.0f)] private float _randomVolumeMax;
        [SerializeField] [Range(0.0f, 2.0f)] private float _randomPitchMin;
        [SerializeField] [Range(0.0f, 2.0f)] private float _randomPitchMax;

        public string GetClipName()
        {
            return _clipName;
        }
        
        public bool CheckIfPlaying()
        {
            return _checkIfPlaying;
        }
        
        public bool IsLooping()
        {
            return _isLooping;
        }

        public AudioClip GetClip(out float volume, out float pitch)
        {
            if (_audioClips.Count == 0)
            {
                volume = 0;
                pitch = 0;

                Debug.LogError("No audio clips in SoundData: " + _clipName);
                return null;
            }

            volume = _volume;
            pitch = _pitch;
            if (_randomize)
            {
                volume = Random.Range(_randomVolumeMin, _randomVolumeMax);
                pitch = Random.Range(_randomPitchMin, _randomPitchMax);
            }

            return _audioClips[Random.Range(0, _audioClips.Count)];
        }
    }
}