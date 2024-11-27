
using UnityEngine;

[CreateAssetMenu(fileName = "PuzzleMenuSettings", menuName = "ScriptableObject/Puzzle/PuzzleMenuSettings")]
public class PuzzleMenuSettings : ScriptableObject
{
    [Header("Buttons")]

    public float timeBetweenGetAnotherAnim = 3f;

    [Header("Puzzle Settings")]

    public PuzzlePrefabScript puzzlePrefab;
    public Vector2 puzzleCellSize;
    public Vector2 puzzleSpacing;
    public PuzzlePrefabSettings[] puzzlesArray;
    public Sprite[] puzzleFramesArray;

    [Header("Part Settings")]

    public PuzzlePartPrefabScript partPrefab;
    public Vector2 partCellSize;
    public Vector2 partSpacing;
    public Sprite[] partBackArray;
    public Sprite[] partStatusArray;

    [Header("Puzzle Group Settings")]

    public string[] groupNamesArray;
    public Sprite[] groupMainColorArray;
    public Sprite[] groupMainGreyArray;
    public Sprite[] groupFrameArray;
}

