
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationScreenSettings", menuName = "ScriptableObject/UI/AnimationScreenSettings")]
public class AnimationScreenSettings : ScriptableObject
{
    [Header("Puzzle")]

    public float puzzleRewardAnimTime = 1.2f;

    [Header("Comet")]

    public CometScript cometPrefab;
    public float cometAnimationTime = 0.6f;

    [Header("BonusText")]

    public Color[] bonusCoinsTextColorsArray;
    public Color[] bonusMoneyTextColorsArray;
    public Color[] bonusXPTextColorsArray;
    public Color[] bonusCrystalsTextColorsArray;
}
