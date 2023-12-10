
using UnityEngine;

public class RewardController : MonoBehaviour
{
    private GameController gameController;

    [Header("Main Links")]

    [SerializeField] UIEndScreen endScreen;
    [SerializeField] NewLevelController levelController;

    [Header("Reward Settings")]

    private int startCoinBonus;
    private int startMoneyBonus;

    private int bingoCount;
    private int bingoBonus;

    private int[] bingoRewardArray;

    private int jackpotLevel;
    private int jackpotBonus;

    private int[] jackpotRewardArray;

    private int endCoinBonus;
    private int endMoneyBonus;

    public void Init(GameController controller)
    {
        gameController = controller;

        jackpotLevel = 1;
    }

    public void SetValues(int[] bingoRewards, int[] jackpotRewards)
    {
        bingoRewardArray = bingoRewards;
        jackpotRewardArray = jackpotRewards;
    }

    public void ResetEndBonus()
    {
        endCoinBonus = 0;
        endMoneyBonus = 0;
        
        bingoCount = 0;
        bingoBonus = 0;

        jackpotLevel = 1;
        jackpotBonus = 0;
    }

    public void UpdateEndBonus(bool isCoins, int count)
    {
        if (isCoins)
            endCoinBonus += count;

        else
            endMoneyBonus += count;
    }

    public void GetReward()
    {
        CheckBingoBonus();

        int bonusCoins = startCoinBonus + endCoinBonus + (bingoBonus * jackpotLevel) + jackpotBonus;
        int bonusMoney = startMoneyBonus + endMoneyBonus;

        gameController.CalculateCoins(true, bonusCoins);
        gameController.CalculateMoney(true, bonusMoney);

        endScreen.UpdateCardRewardText(bonusCoins, bonusMoney);

        Debug.Log($"coins = {bonusCoins}, money = {bonusMoney}");
    }

    #region Bingo Bonus

    public void AddBonus(bool isCoins, int count)
    {
        if (isCoins)
            endCoinBonus += count;
        else
            endMoneyBonus += count;
    }

    public void UpdateBingoBonus(int count) 
    { 
        bingoCount = count;
        CheckBingoBonus();
    }

    private void CheckBingoBonus()
    {
        if(bingoCount == 0)
            bingoBonus = bingoRewardArray[0];
        
        if(bingoCount == 1)
            bingoBonus = bingoRewardArray[1];
        
        if(bingoCount == 2)
            bingoBonus = bingoRewardArray[2];
        
        if(bingoCount >= 3)
            bingoBonus = bingoRewardArray[3];
    }
    #endregion

    #region Jackpot Bonus

    public void UpdateJackpotLevel(int lvl)
    {
        jackpotLevel = lvl;
    }

    public void UpdateJackpotBonus() 
    {
        if(jackpotLevel == 1)
            jackpotBonus = jackpotRewardArray[0]; 
    
        if(jackpotLevel == 2)
            jackpotBonus = jackpotRewardArray[1]; 
    
        if(jackpotLevel == 3)
            jackpotBonus = jackpotRewardArray[2]; 
    
        if(jackpotLevel == 4)
            jackpotBonus = jackpotRewardArray[3]; 
    
        if(jackpotLevel == 5)
            jackpotBonus = jackpotRewardArray[4]; 
    }

    #endregion
}
