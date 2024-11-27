
using UnityEngine;

[CreateAssetMenu(fileName = "GachaScreenSettings", menuName = "ScriptableObject/UI/GachaScreenSettings")]
public class GachaScreenSettings : ScriptableObject
{
    public int rewardADBonus = 400;
    public int adBonusAnimationStepCount = 10;
    public float adBonusAnimationTime = 2f;
    public int[] minutesToCapsuleArray;
    public float gachaMixDuration = 2f;

    public Sprite[] claimButtonSpritesArray;

    public int GetMinutesByCount(int count)
    {
        int minutes = minutesToCapsuleArray[0];

        if (count < 1)
            minutes = minutesToCapsuleArray[0];
        else
            minutes = minutesToCapsuleArray[1];

        return minutes;
    }
}
