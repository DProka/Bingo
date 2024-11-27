
using UnityEngine;

[CreateAssetMenu(fileName = "TableSettings", menuName = "ScriptableObject/Game/TableSettings")]
public class TableSettings : ScriptableObject
{
    [Header("Bingo Settings")]

    public int maxBingosInRound = 3;
    public int minBingosInRound = 1;
    public int[] bingoRewardArray;
    public int[] moneyRewardArray;
    public float bingoAnumationTimescale = 0.7f;

    [Header("JackPot Settings")]

    public int jackpotProcentage = 100;
    public int[] jackpotRewardArray;

    [Header("XP Settings")]

    public int xpPerChip;

    [Header("Booster Settings")]

    public TableBoosterSettings boosterSettings;

    [Header("Reward Chest Values")]

    public int minChestReward = 30;
    public int maxChestReward = 70;

    [Header("Ball Settings")]

    public float ballGenSpeed = 4.5f;
    public float ballGenSpeedLvl3 = 4f;
    public float ballGenSpeedLvl10 = 3.5f;
    public float ballAppearSpeed = 0.5f;
    public float ballMoveSpeed = 0.5f;
    public float alarmTimeBeforeEnd = 5.5f;
    public BallObject ballPrefab;
    public Color[] textColors;
    public Sprite[] ballsSprites;
}
