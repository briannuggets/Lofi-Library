using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Static script: Plays short audioclips for UI and other interactions.
public class PlaySound : MonoBehaviour
{
    private static PlaySound playSoundInstance;

    [SerializeField]
    private List<AudioClip> sounds;

    private AudioSource audioPlayer;

    void Awake() {
        DontDestroyOnLoad(gameObject);
        if (playSoundInstance == null) {
            playSoundInstance = this;
            audioPlayer = GetComponent<AudioSource>();
        } else {
            Destroy(gameObject);
        }
    }

    void Start() {
        DontDestroyOnLoad(gameObject);
    }

    // Plays a sound effect given the clip index
    public void Play(int index) {
        audioPlayer.clip = sounds[index];
        audioPlayer.Play();
    }
}
