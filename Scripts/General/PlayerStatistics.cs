
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStatistics
{
    public int playerCoins { get; private set; }
    public int playerMoney { get; private set; }
    public int playerCrystals { get; private set; }

    public int playedRoundsCount { get; private set; }

    public int currentXPLevel { get; private set; }
    public int currentXPPoints { get; private set; }
    public int previousXPPointsValue { get; private set; }

    public int[] playerXPLevelsArray => xpCalculator.levelConditionsArray;

    private GeneralSave generalSave;
    private PlayerXPCalculator xpCalculator;

    public PlayerStatistics(GeneralSave _generalSave, int[] pointsOnLvlArray)
    {
        generalSave = _generalSave;
        xpCalculator = new PlayerXPCalculator(pointsOnLvlArray);

        ResetStatistics();
    }

    public void CalculateCoins(int count)
    {
        generalSave.CalculateCoins(count);
        playerCoins = generalSave.playerCoins;
    }

    public void CalculateMoney(int count)
    {
        generalSave.CalculateMoney(count);
        playerMoney = generalSave.playerMoney;
    }
    
    public void CalculateCrystals(int count)
    {
        generalSave.CalculateCrystals(count);
        playerCrystals = generalSave.playerCrystals;
    }
    
    public void CalculateXP(int points)
    {
        currentXPPoints += points;
        CheckPlayerXPLevel();
    }

    public void IncreaseRoundCount(int lvl)
    {
        playedRoundsCount += lvl;
        generalSave.UpdatePlayerLvl(playedRoundsCount);
    }

    public void SetParametersByTutorial(bool isTutor)
    {
        if (isTutor)
        {
            playedRoundsCount = 1;
        }
        else
        {
            playerCoins = generalSave.playerCoins;
            playerMoney = generalSave.playerMoney;
            playedRoundsCount = generalSave.playerLvl;
        }
    }

    public void ResetStatistics()
    {
        playerCoins = generalSave.playerCoins;
        playerMoney = generalSave.playerMoney;
        playerCrystals = generalSave.playerCrystals;

        playedRoundsCount = generalSave.playerLvl;

        currentXPLevel = generalSave.playerXPLevel;
        currentXPPoints = generalSave.playerXPPoints;
    }

    private void CheckPlayerXPLevel()
    {
        int[] xpStats = xpCalculator.CheckNewLevel(currentXPLevel, currentXPPoints);

        if (currentXPLevel < xpStats[0])
        {
            Dictionary<BoosterManager.Type, int> rewards = GameController.Instance.boosterManager.GetRewardsByLevel(xpStats[0]);

            BoosterManager.Type[] types = rewards.Keys.ToArray();
            int[] counts = rewards.Values.ToArray();

            UIController.Instance.StartNewLevelAnimation(xpStats, types, counts);
        }
        else
            UIController.Instance.StartXPProgressAnimation();

        previousXPPointsValue = currentXPPoints;
        currentXPLevel = xpStats[0];
        currentXPPoints = xpStats[1];

        generalSave.SavePlayerXP(new int[] { currentXPLevel, currentXPPoints });

        Debug.Log("XP Level = " + currentXPLevel + " XP points = " + currentXPPoints);
    }
}
