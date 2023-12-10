
using UnityEngine;

[CreateAssetMenu(fileName = "SoundData", menuName = "ScriptableObject/SoundData")]
public class SoundData : ScriptableObject
{
    public AudioClip startSound;

    public AudioClip[] numbersArray;
}
