
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelScreenSettings", menuName = "ScriptableObject/UI/NewLevelScreenSettings")]
public class NewLevelScreenSettings : ScriptableObject
{
    [Header("Sprites")]

    public Sprite ballsPlus5;
    public Sprite trippleDoub;
    public Sprite airplane;
    public Sprite autoBingo;
    public Sprite doubleProgress;
    public Sprite crystal;

    [Header("Count Colors")]

    public Color purple;
    public Color orange;
}
