
using UnityEngine;

public class RewardController
{
    [Header("Main Links")]

    private int[] bingoRewardArray;
    private int[] moneyRewardArray;
    private int[] jackpotRewardArray;

    private int bingoCount;
    private int bingoBonus;
    private int moneyBonus;
    private int jackpotLevel;
    private int jackpotBonus;
    private int endCoinBonus;
    private int endMoneyBonus;

    private int xpBonus;

    private bool doubleCoinsActive;

    public RewardController(TableSettings settings)
    {
        EventBus.onRoundStarted += ResetEndBonus;
        EventBus.onRewardRecieved += GetReward;

        bingoRewardArray = settings.bingoRewardArray;
        jackpotRewardArray = settings.jackpotRewardArray;
        moneyRewardArray = settings.moneyRewardArray;

        jackpotLevel = 1;
    }
    
    public void ResetEndBonus(int cardsCount = 0)
    {
        endCoinBonus = 0;
        endMoneyBonus = 0;
        
        bingoCount = 0;
        bingoBonus = 0;
        moneyBonus = 0;

        jackpotLevel = 1;
        jackpotBonus = 0;

        xpBonus = 0;
    }

    public void UpdateEndBonus(bool isCoins, int count)
    {
        if (isCoins)
            endCoinBonus += count;
        else
            endMoneyBonus += count;
    }

    public void UpdateXPBonus(int count)
    {
        xpBonus += count;

        Debug.Log("Xp bonus in round = " + xpBonus);
    }

    public void GetReward()
    {
        CheckBingoBonus();

        int[] reward = CheckReward();
        int bonusCoins = reward[0];
        int bonusMoney = reward[1];

        if (GameController.tutorialIsActive && GameController.tutorialManager.GetRoomProgress() < 3)
        {
            bonusCoins = 2000;   
            bonusMoney = 50;   
        }

        GameController.Instance.CalculateCoins(bonusCoins);
        GameController.Instance.CalculateMoney(bonusMoney);
        GameController.Instance.CalculateXP(xpBonus);

        Debug.Log($"coins = {bonusCoins}, money = {bonusMoney}, xp = {xpBonus}");
        AppmetricaTimeTracking.reportLevelEnd(bonusCoins, bonusMoney);
        DesignManager.updateRoomCash?.Invoke(bonusCoins, bonusMoney);
    }

    public int[] CheckReward()
    {
        //if (GameController.Instance.boosterManager.doubleCoinsIsActive)
        //    bingoBonus = bingoBonus * 2;

        int bonusCoins = endCoinBonus + (bingoBonus * jackpotLevel) + jackpotBonus;
        int bonusMoney = (endMoneyBonus + (moneyBonus * 1)) * jackpotLevel;

        if (doubleCoinsActive)
            bonusCoins = bonusCoins * 2;

        return new int[] { bonusCoins, bonusMoney };
    }

    public void SwitchDoubleCoinsActive(bool isActive) => doubleCoinsActive = isActive;

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
        if(bingoCount <= 3)
        {
            bingoBonus = bingoRewardArray[bingoCount];
            moneyBonus = moneyRewardArray[bingoCount];
        }
        else
        {
            bingoBonus = bingoRewardArray[3];
            moneyBonus = moneyRewardArray[3];
        }
    }
    #endregion

    #region Jackpot Bonus

    public void UpdateJackpotLevel(int lvl) { jackpotLevel = lvl; }
    
    public void UpdateJackpotBonus() { jackpotBonus = jackpotRewardArray[jackpotLevel - 1]; }
    #endregion
}
