
using UnityEngine;

[CreateAssetMenu(fileName = "SoundData", menuName = "ScriptableObject/SoundData")]
public class SoundData : ScriptableObject
{
    [Header("Choose Bid")]

    public AudioClip buttonClick1;
    public AudioClip buttonClick2;
    public AudioClip[] bidSounds;

    [Header("Bingo")]

    public AudioClip[] bingoSounds;

    public AudioClip jackPotSound;

    [Header("Chip")]

    public AudioClip chipOpen;
    public AudioClip chestOpen;
    public AudioClip chestSet;

    [Header("Bonus")]

    public AudioClip bonusReady;
    public AudioClip bonusUsage;

    [Header("Game")]

    public AudioClip startSound;
    
    public AudioClip[] numbersArray;

    [Header("Timer")]

    public AudioClip timerSound;
    public AudioClip timerEnd;

    public AudioClip[] beforeTimerSounds;

    [Header("Reward Screen")]

    public AudioClip roundOverSound;
    public AudioClip rewardSound;
    public AudioClip moneyPileSound;

    [Header("Puzzle Screen")]

    public AudioClip puzzlePart;
    public AudioClip puzzleFull;
    
    [Header("Gacha Screen")]

    public AudioClip gachaStart;
    public AudioClip gachaMix;
    public AudioClip capsuleAppearing;

    [Header("Music")]

    public AudioClip mainMenu;
}
