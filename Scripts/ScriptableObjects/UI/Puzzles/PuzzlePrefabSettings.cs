
using UnityEngine;

[CreateAssetMenu(fileName = "Puzzle", menuName = "ScriptableObject/Puzzle/PuzzlePrefabSettings")]
public class PuzzlePrefabSettings : ScriptableObject
{
    public Sprite mainSprite;
    public Sprite[] partSpritesArray;
}
