using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

// Script for manipulating the title-screen and its camera.
public class PanCamera : MonoBehaviour
{
    [SerializeField]
    private Vector3 cameraStart;

    [SerializeField]
    private float panTime;

    public bool inUse;
    private GameObject titleScreen;
    private Image transition;

    private PostProcessVolume ppVolume;
    private ColorGrading colorGrading;
    private DepthOfField depthOfField;
    private ChromaticAberration chromaticAberration;

    private PlaySound playSound;
    private StartTutorial startTutorial;

    void Awake() {
        titleScreen = GameObject.Find("TitleScreen");
        transition = GameObject.Find("TitleTransition").GetComponent<Image>();
        playSound = GameObject.Find("AudioPlayer").GetComponent<PlaySound>();
        startTutorial = GameObject.Find("Tutorial").GetComponent<StartTutorial>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ppVolume = GetComponent<PostProcessVolume>();
        ppVolume.profile.TryGetSettings(out colorGrading);
        ppVolume.profile.TryGetSettings(out depthOfField);
        ppVolume.profile.TryGetSettings(out chromaticAberration);
        colorGrading.temperature.value = -50f;
        transition.gameObject.SetActive(false);
        inUse = true;
        gameObject.transform.position = cameraStart;
        Invoke("DisablePlayerActions", 0.1f);
        StartCoroutine(Pan(true));
    }

    // Coroutine: Pan the camera repeatedly to the left and right.
    private IEnumerator Pan(bool direction) {
        float counter = panTime;
        float increment = 0;
        if (direction) {
            increment = 0.05f;
        } else {
            increment = -0.05f;
        }
        while (counter > 0) {
            yield return new WaitForSeconds(0.1f);
            gameObject.transform.position = new Vector3(gameObject.transform.position.x + increment, gameObject.transform.position.y, gameObject.transform.position.z);
            counter = counter - 0.1f;
        }
        StartCoroutine(Pan(!direction));
    }

    // Disable player movement on title screen
    private void DisablePlayerActions() {
        GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().canMove = false;
    }

    // Enable player movement on title screen
    private void EnablePlayerActions() {
        GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().canMove = true;
    }

    // Switch from title-screen camera to the main camera.
    public void SwitchCamera() {
        playSound.Play(18);
        StartCoroutine(StartTransition());
    }

    // Coroutine: Uses a transition screen to hide the switch between cameras
    private IEnumerator StartTransition() {
        transition.gameObject.SetActive(true);
        float counter = 0.1f;
        while (counter > 0) {
            yield return new WaitForSeconds(0.002f);
            transition.color = new Color(1, 1, 1, transition.color.a + 0.03f);
            counter = counter - 0.002f;
        }

        GetComponent<Camera>().depth = -2;
        titleScreen.SetActive(false);
        inUse = false;
        EnablePlayerActions();
        depthOfField.focusDistance.value = 10;
        chromaticAberration.active = false;

        counter = 0.9f;
        while (counter > 0) {
            yield return new WaitForSeconds(0.01f);
            transition.color = new Color(1, 1, 1, transition.color.a - 0.02f);
            colorGrading.temperature.value = colorGrading.temperature.value + 0.555f;
            counter = counter - 0.01f;
        }
        transition.gameObject.SetActive(false);

        // Start tutorial if on first load
        if (!File.Exists(Application.persistentDataPath + "/save.json")) {
            startTutorial.OpenTutorial();
        }
        DisableCamera();
    }

    // Disables the title screen; used when returning from the library
    public void DisableTitle() {
        playSound.Play(18);
        GetComponent<Camera>().depth = -2;
        titleScreen.SetActive(false);
        inUse = false;
        Invoke("EnablePlayerActions", 0.2f);
        Invoke("DisableCamera", 0.3f);
    }

    // Sets this object inactive
    private void DisableCamera() {
        gameObject.SetActive(false);
    }
}
