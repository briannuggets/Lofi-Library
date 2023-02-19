using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Video;

[Serializable]
public class TutorialClip
{
    public Texture2D image;
    public VideoClip clip;

    public TutorialClip(Texture2D texture) {
        image = texture;
    }

    public TutorialClip(VideoClip video) {
        clip = video;
    }
}
