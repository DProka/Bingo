
using UnityEngine;

[CreateAssetMenu(fileName = "TableBoosterSettings", menuName = "ScriptableObject/Game/TableBoosterSettings")]
public class TableBoosterSettings : ScriptableObject
{
    public int fullBoosterStepCount = 4;
    public int boosterCoinBonus = 100;
    public float progressAnimDuration = 0.4f;
    public float activeAnimDuration = 0.3f;

    public float doubleXPpercentage = 20f;
    public float doubleCoinspercentage = 20f;

    public Sprite[] progressSpriteArray;
    public Sprite[] boostersSpriteArray;
    public Sprite[] startBoostersSpriteArray;
    public Sprite[] autoBoosterSpritesArray;

    [Header("Round boosters")]

    public Sprite[] roundBoosterSprites;
}
