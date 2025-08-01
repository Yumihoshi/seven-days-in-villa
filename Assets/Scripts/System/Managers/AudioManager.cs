using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cjr.AudioSystem
{
    public class AudioManager :cjr.Single. SingleMon<AudioManager>
    {
        public AudioSource _AudioSource;


        protected override void Awake()
        {
            base.Awake();
            _AudioSource = GetComponent<AudioSource>();
        }

        public void PlayAudio(AudioClip clip,bool loop = false,float time=0f)
        {
            _AudioSource.clip = clip;
            _AudioSource.loop = loop;
            _AudioSource.Play();
            _AudioSource.time = time; // Play ��������
        }

        public void PlayAudio(string FilePath, bool loop = false, float time = 0f)
        {
            AudioClip clip = Resources.Load<AudioClip>(FilePath); // music.wav �� music.mp3
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

}
