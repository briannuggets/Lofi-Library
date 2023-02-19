using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

// Script for toggling rain sounds and particle effects using the item interface
public class ItemRain : MonoBehaviour, ItemInterface
{
    private PlaySound playSound;
    private ParticleSystem rainMaker;
    private AudioSource rainSound;
    private AudioLowPassFilter filter;
    private bool active;

    private PostProcessVolume ppVolume;
    private ColorGrading colorGrading;

    // Start is called before the first frame update
    void Awake()
    {
        rainMaker = GameObject.Find("RainMaker").GetComponent<ParticleSystem>();
        playSound = GameObject.Find("AudioPlayer").GetComponent<PlaySound>();
        rainSound = GameObject.Find("RainSound").GetComponent<AudioSource>();
        filter = rainSound.gameObject.GetComponent<AudioLowPassFilter>();
    }

    void Start() {
        ppVolume = GameObject.Find("Main Camera").GetComponent<PostProcessVolume>();
        ppVolume.profile.TryGetSettings(out colorGrading);
        if (rainSound.isPlaying) {
            colorGrading.temperature.value = -50f;
            rainMaker.Simulate(rainMaker.main.duration);
            rainMaker.Play();
            active = true;
        } else {
            colorGrading.temperature.value = 0f;
            rainMaker.Stop();
            active = false;
        }
    }

    // Use the staff
    public void Use(ItemData data) {
        StopAllCoroutines();
        if (active) {
            active = false;
            rainMaker.Stop();
            playSound.Play(4);
            rainSound.Stop();
            StartCoroutine(FadeToOrange());
        } else {
            active = true;
            rainMaker.Play();
            playSound.Play(3);
            rainSound.Play();
            StartCoroutine(FadeToBlue());
        }
    }

    // Applies a low-pass filter to the rain sounds
    public void ApplyLowPass() {
        if (active) {
            filter.cutoffFrequency = 6500;
        }
    }

    // Removes the low-pass filter from the rain sounds
    public void RemoveLowPass() {
        if (active) {
            filter.cutoffFrequency = 22000;
        }
    }

    // Coroutine: Decreases the temperature of a post-processing filter
    private IEnumerator FadeToBlue() {
        float counter = 5f;
        while (counter > 0) {
            yield return new WaitForSeconds(0.1f);
            colorGrading.temperature.value = colorGrading.temperature.value - 1f;
            if (colorGrading.temperature.value < -50f) {
                break;
            }
            counter = counter - 0.1f;
        }
    }

    // Coroutine: Increases the temperature of a post-processing filter
    private IEnumerator FadeToOrange() {
        float counter = 5f;
        while (counter > 0) {
            yield return new WaitForSeconds(0.1f);
            colorGrading.temperature.value = colorGrading.temperature.value + 1f;
            if (colorGrading.temperature.value > 0f) {
                break;
            }
            counter = counter - 0.1f;
        }
    }
}
