
using UnityEngine;

[CreateAssetMenu(fileName = "ChipSettings", menuName = "ScriptableObject/Game/ChipSettings")]

public class ChipSettings : ScriptableObject
{
    public float animationTime = 0.3f;
    public float animationDelay = 0.1f;

    public Sprite[] chipSprites;
}
