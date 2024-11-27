
using UnityEngine;

[CreateAssetMenu(fileName = "JoinGameMenuSettings", menuName = "ScriptableObject/UI/JoinGameMenuSettings")]
public class JoinGameMenuSettings : ScriptableObject
{
    public Sprite[] playButtonSprites;

    [Header("Cards Settings")]

    public Sprite[] chestsSpritesArray;
    public Sprite[] menuChestsSpritesArray;
    public Sprite[] buttonSpritesArray;
    public Sprite[] shadowX2SpritesArray;
    public Sprite[] shadowX4SpritesArray;

    [Header("Bet Part")]

    public int[] xpLevelsArray;
    public int[] betPricesArray;
    public Sprite[] minusImageArray;
    public Sprite[] plusImageArray;

    [Header("General Bonuses Part")]

    public int[] bonusUnlockLvlArray;
    public Sprite[] boosterBackImagesArray;
    public Sprite[] boosterFrontImagesArray;
    public Sprite[] BoosterStatusImagesArray;
}
