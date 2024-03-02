using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public Sprite dictateOnImage;
    public Sprite dictateOffImage;
    public Button button;
    private bool isOn = false; // Start with the dictate button off

    public AudioSource homeRec;
    public AudioSource searchRec;
    public AudioSource settingsRec;
    public AudioSource profileRec;
    public AudioSource arModeRec;
    public AudioSource videosRec;
    public AudioSource resourcesRec;

    private void Start()
    {
        button.image.sprite = dictateOffImage;
        MuteAllAudio(true);
    }

    private void Update()
    {
        // You can add any update logic here if needed
    }

    public void ButtonClicked()
    {
        isOn = !isOn;

        if (isOn)
        {
            button.image.sprite = dictateOnImage;
        }
        else
        {
            button.image.sprite = dictateOffImage;
        }

        MuteAllAudio(!isOn);
    }

    private void MuteAllAudio(bool mute)
    {
        homeRec.mute = mute;
        searchRec.mute = mute;
        settingsRec.mute = mute;
        profileRec.mute = mute;
        arModeRec.mute = mute;
        videosRec.mute = mute;
        resourcesRec.mute = mute;
    }
}

