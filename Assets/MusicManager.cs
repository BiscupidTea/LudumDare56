using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip music;

    private AudioManager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        audioManager.PlayMusic(music);
    }
}
