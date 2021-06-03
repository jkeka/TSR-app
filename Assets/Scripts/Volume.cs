using UnityEngine;
using UnityEngine.Audio;

public class Volume : MonoBehaviour
{
    // This class operates the volume slider on settings

    // Reference to volume mixer
    public AudioMixer mixer;

    //Changes slider value to logarithmic insted of linear. Converts to decibels
    public void SetLevel (float volSliderValue)
    {
        mixer.SetFloat("AudioVol", Mathf.Log10 (volSliderValue) * 20); 
    }

}
