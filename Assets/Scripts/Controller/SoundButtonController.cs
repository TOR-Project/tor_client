using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundButtonController : MonoBehaviour
{
    [SerializeField]
    Sprite soundOnSprite;
    [SerializeField]
    Sprite soundOffSprite;
    [SerializeField]
    Image soundIconImage;

    private void OnEnable()
    {
        updateButtonImage();
    }

    public void toggleSoundOn()
    {
        SoundManager.instance.toggleSoundOn();
        updateButtonImage();
    }

    private void updateButtonImage()
    {
        soundIconImage.sprite = SoundManager.instance.isSoundOn ? soundOnSprite : soundOffSprite;
    }
}
