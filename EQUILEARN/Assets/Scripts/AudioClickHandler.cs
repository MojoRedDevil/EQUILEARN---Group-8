using UnityEngine;
using UnityEngine.UI;

public class AudioClickHandler : MonoBehaviour
{
    public AudioSource AudioSource;
    public AudioClip HomeRec, SearchRec, SettingsRec, profilerec, profiledetailsrec, PaSrec;

    public void homeButton()
    {
        AudioSource.clip = HomeRec;
        AudioSource.Play();
    }

    public void searchButton()
    {
        AudioSource.clip = SearchRec;
        AudioSource.Play();
    }

    public void settingsButton()
    {
        AudioSource.clip = SettingsRec;
        AudioSource.Play();
    }

    public void profileButton()
    {
        AudioSource.clip = profilerec;
        AudioSource.Play();
    }

    public void personalDetailsButton()
    {
        AudioSource.clip = profiledetailsrec;
        AudioSource.Play();
    }

    public void privacySecurityButton()
    {
        AudioSource.clip = PaSrec;
        AudioSource.Play();
    }

}

