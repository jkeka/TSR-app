using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Volume : MonoBehaviour
{
    public AudioMixer mixer;

    public void SetLevel (float volSliderValue)
    {
        mixer.SetFloat("AudioVol", Mathf.Log10 (volSliderValue) * 20); //Changes slider value to logarithmic insted of linear. Converts to decibels
    }

}
