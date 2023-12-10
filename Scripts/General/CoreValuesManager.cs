using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreValuesManager : MonoBehaviour
{
    [Header("Links")]

    private GameController gameController;
    private TableController tableController;
    private RewardController rewardController;

    [Header("Game Settings")]

    [SerializeField] float ballStartSpeed = 4.5f;
    [SerializeField] float ballSpeedLvl3 = 4f;
    [SerializeField] float ballSpeedLvl10 = 3.5f;

    [Header("Bingo Settings")]

    [SerializeField] int maxBingosInRound = 3;
    [SerializeField] int minBingosInRound = 1;
    [SerializeField] int[] bingoRewardArray;

    [Header("JackPot Settings")]

    [SerializeField] int jackpotProcentage = 100;
    [SerializeField] int[] jackpotRewardArray;

    [Header("Booster Values")]

    [SerializeField] int boosterCoinBonus = 100;

    [Header("Reward Chest Values")]

    [SerializeField] int minChestReward = 30;
    [SerializeField] int maxChestReward = 70;

    public void Init(GameController gc, TableController tc, RewardController rc)
    {
        gameController = gc;
        tableController = tc;
        rewardController = rc;

        gameController.SetValues(ballStartSpeed, ballSpeedLvl3, ballSpeedLvl10);
        tableController.SetValues(boosterCoinBonus, minBingosInRound, maxBingosInRound, minChestReward, maxChestReward, jackpotProcentage);
        rewardController.SetValues(bingoRewardArray, jackpotRewardArray);
    }
}
