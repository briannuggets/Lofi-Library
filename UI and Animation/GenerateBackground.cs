using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Sets the background image on entering the library.
public class GenerateBackground : MonoBehaviour
{
    [SerializeField]
    private List<Texture2D> backgrounds;

    private RawImage currentBackground;
    
    // Start is called before the first frame update
    void Start()
    {
        currentBackground = GetComponent<RawImage>();
        currentBackground.texture = backgrounds[Random.Range(0, backgrounds.Count - 1)];
    }
}
