using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] SoundData soundData;


    private AudioSource audioSource;

    private bool voice;

    public void Init()
    {
        audioSource = GetComponent<AudioSource>();
    }

    #region Switches

    public void SwitchVoice(bool isOn)
    {
        voice = isOn;
    }
    #endregion

    public void PlayStart()
    {
        if (voice)
            audioSource.PlayOneShot(soundData.startSound);
    }

    public void PlayNumber(int num)
    {
        if(voice)
            audioSource.PlayOneShot(soundData.numbersArray[num - 1]);
    }
}
