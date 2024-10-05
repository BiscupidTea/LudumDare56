using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAudio : MonoBehaviour
{
    [SerializeField] private AudioClip shootSFX;

    private AudioManager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void PlayShootSound()
    {
        audioManager.PlaySFX(shootSFX);
    }
}
