using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using CG.QuickSave;
using CG.Starter;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace CG.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        #region Singleton

        public static AudioManager instance;

        private void Awake()
        {
            if (AudioManager.instance != null)
            {
                return;
            }

            instance = this;
        }

        #endregion

        #region Local

        private AudioSource _source;

        #endregion

        #region Local Variables

        private IEnumerator Process;

        #endregion

        #region Clips

        [Header("Clips")] 
        [SerializeField] private AudioClipStorage ClipStorage;
        #endregion
        private VolumeSave VolumeData = new VolumeSave();

        #region Unity

        private void Start()
        {
            _source = GetComponent<AudioSource>();
        }

        #endregion
        
        #region Save & Load

        public void Load()
        {
            VolumeData = QuickSaver.LoadData<VolumeSave>(QuickSaver.GetAppDataPath(CGConstants.AudioSave));
        }

        public void Save()
        {
            QuickSaver.SaveData(QuickSaver.GetAppDataPath(CGConstants.AudioSave),VolumeData);
        }

        #endregion

        #region Play
        public void PlayOneShot(AudioClip clip, AudioType type)
        {
            int volume = GetVolume(type);
            
            _source.PlayOneShot(clip,volume);
        }
        public void PlayWithTime(AudioClip clip,int playTime, AudioType type)
        {
            CheckPlay();
            
            int volume = GetVolume(type);

            _source.clip = clip;
            _source.volume = volume;
            _source.PlayScheduled(playTime);
        }
        public void PlayAndCall(AudioClip clip, int playTime, AudioType type, params Action[] callbacks)
        {
            CheckPlay();
            int volume = GetVolume(type);
            Process = PlayAndCallMethods();
            StartCoroutine(Process);
            
            IEnumerator PlayAndCallMethods()
            {
                _source.clip = clip;
                _source.volume = volume;
                _source.PlayScheduled(playTime);
                yield return new WaitForSeconds(playTime);
                foreach (var callback in callbacks)
                {
                    
                    ((Action)callback).Invoke();
                    
                }
            }
                    
        }
        #endregion

        #region Button
        public void ButtonHover(int index = 0)
        {
            if (ClipStorage.ButtonHoverAudioClips.Length < 0)
            {
                UnityEngine.Debug.Log("Please add multiple Audio Clips to call this method! The path could be CG's Starter > Packages > QuickAudio > Scriptables");
            }
            PlayOneShot(ClipStorage.ButtonHoverAudioClips[index],AudioType.Sfx);
        }
        public void ButtonExit(int index = 0)
        {
            if (ClipStorage.ButtonExitAudioClips.Length < 0)
            {
                UnityEngine.Debug.Log("Please add multiple Audio Clips to call this method! The path could be CG's Starter > Packages > QuickAudio > Scriptables");
            }
            PlayOneShot(ClipStorage.ButtonExitAudioClips[index],AudioType.Sfx);
        }
        #endregion

        #region Methods

        private void CheckPlay()
        {
            if(Process!=null)
             StopCoroutine(Process);
            if (_source.isPlaying)
            {
                _source.Stop();
            }
        }
        

        #endregion
        
        #region Update

        public void UpdateVolume(int newValue, AudioType type)
        {
            switch (type)
            {
                case AudioType.Sfx:
                    VolumeData.Sfx = newValue;
                    break;
                case AudioType.Music:
                    VolumeData.Music = newValue;
                    break;
                case AudioType.Voice:
                    VolumeData.Voice = newValue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        #endregion
        
        #region Gets

        private int GetVolume(AudioType type)
        {
            switch (type)
            {
                case AudioType.Sfx:
                    return VolumeData.Sfx;
                    break;
                case AudioType.Music:
                    return VolumeData.Music;
                    break;
                case AudioType.Voice:
                    return VolumeData.Voice;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        #endregion

        public enum AudioType
        {
            Sfx,
            Music,
            Voice
        }
    }

    [CreateAssetMenu(menuName = "Audio Manager",fileName = "Audio Clips")]
    public class AudioClipStorage : ScriptableObject
    {
        public AudioClip[] ButtonHoverAudioClips;
        public AudioClip[] ButtonExitAudioClips;
    }

}
