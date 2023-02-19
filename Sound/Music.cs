using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Static script: Plays background music
public class Music : MonoBehaviour
{
    [SerializeField]
    private List<AudioClip> musicList;

    [SerializeField]
    private AudioClip defaultMusic;

    private PlaySound playSound;

    private AudioSource source;

    private static Music musicInstance;

    private IEnumerator delayCoroutine;

    void Awake() {
        DontDestroyOnLoad(gameObject);

        if (musicInstance == null) {
            delayCoroutine = null;
            musicInstance = this;
            playSound = GameObject.Find("AudioPlayer").GetComponent<PlaySound>();
            source = GetComponent<AudioSource>();
            source.clip = defaultMusic;

            // Play default music on first load-in
            var defaultTimeStamps = new float[] {0f, 75f, 348f, 478f, 601f, 800f, 928f, 1129f, 
                1355f, 1501f, 1614f, 1751f, 1954f, 2097f, 2232f, 2372f, 2504f, 2733f, 2890f, 3024f, 
                3160f, 3309f, 3456f, 3636f, 3798f, 3919f, 4067f, 4226f, 4381f, 4568f, 4705f, 4890f, 
                5046f, 5169f, 5294f, 5437f};
            int start = Random.Range(0, defaultTimeStamps.Length);
            source.time = defaultTimeStamps[start];
            source.Play();
        } else {
            Destroy(gameObject);
        }
    }

    // Plays music given an audioclip index
    public void Play(int index) {
        if (delayCoroutine != null) {
            StopCoroutine(delayCoroutine);
        }
        source.Pause();
        playSound.Play(17);
        delayCoroutine = Delay(index);
        StartCoroutine(delayCoroutine);
    }

    // Coroutine: Fades in music over 3 seconds
    public IEnumerator FadeInMusic() {
        source.volume = 0f;
        float counter = 3f;
        while (counter > 0) {
            yield return new WaitForSeconds(0.1f);
            source.volume = source.volume + 0.04f;
            counter = counter - 0.1f;
        }
    }

    // Coroutine: Plays music given the index, delays the start time by 1 second.
    private IEnumerator Delay(int index) {
        float counter = 1f;
        while (counter > 0) {
            yield return new WaitForSeconds(0.1f);
            counter = counter - 0.1f;
        }
        source.clip = musicList[index];
        source.Play();
        delayCoroutine = null;
    }
}
