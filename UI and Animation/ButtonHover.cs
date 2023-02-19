using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Script to control button color on mouse hover
public class ButtonHover : MonoBehaviour
{   
    // Color is yellow on hover
    public void MouseHover() {
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 0.5f, 1f);
    }

    // Color is white on de-hover
    public void MouseExit() {
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(1f, 1f, 1f, 1f);
    }
}
