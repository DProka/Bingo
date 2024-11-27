
using UnityEngine;

[CreateAssetMenu(fileName = "TimeBoosterMenuSettings", menuName = "ScriptableObject/UI/TimeBoosterMenuSettings")]
public class TimeBoosterMenuSettings : ScriptableObject
{
    [Header("Menu")]

    public float openAnimSpeed = 0.5f;

    [Header("Prefab")]

    public Sprite[] boostersArray;
    public Sprite[] backPrefabArray;
    public Color[] textColorsArray;
    public bool[] timerTypeArray;
}
