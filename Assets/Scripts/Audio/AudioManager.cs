using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
   [SerializeField] private AudioSource musicSource;
   [SerializeField] private AudioSource sfxSource;
   
   private static AudioManager instance;

   private void Awake()
   {
      if (instance == null)
      {
         instance = this;
         DontDestroyOnLoad(gameObject);
      }
      else
      {
         Destroy(gameObject);
      }
   }

   public void PlayMusic(AudioClip musicClip)
   {
         musicSource.Stop();
         musicSource.clip = musicClip;
         musicSource.Play();
   }

   public void PlaySFX(AudioClip sfxClip)
   {
      sfxSource.PlayOneShot(sfxClip);
   }
}
