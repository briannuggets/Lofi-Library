using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Script to control fade-in for entering the library.
public class FadeIn : MonoBehaviour
{
    private Image filter;
    [SerializeField]
    private AnimationCurve fadeCurve;

    private void Awake() {
        filter = GameObject.Find("Fade").GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Fade());
    }

    // Coroutine: uses an animation curve to fade a filter over 10 seconds.
    private IEnumerator Fade() {
        float counter = 10f;
        float pass = 0f;
        while (counter > 0) {
            yield return new WaitForSeconds(0.1f);
            filter.color = new Color(255, 255, 255, filter.color.a - fadeCurve.Evaluate(pass));
            counter = counter - 0.1f;
            pass = pass + 0.01f;
        }
    }
}
