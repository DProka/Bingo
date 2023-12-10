
using System;
using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Data Links")]

    [SerializeField] GeneralSave generalSave;
    [SerializeField] RoomSave roomSave;

    [Header("Game Settings")]

    [SerializeField] CoreValuesManager valuesManager;

    [SerializeField] float firstLoadTime;
    private float loadTimer;
    private bool gameIsLoaded;

    public int playerCoins;
    public int playerMoney;
    public int playerLvl;

    private bool gameIsActive;

    [Header("Reward")]

    [SerializeField] RewardController rewardController;

    [Header("UI Links")]

    [SerializeField] UIController uiController;

    [Header("Table")]

    [SerializeField] TableController tableController;
    private float ballGenSpeed;
    private float ballGenSpeedLvl3;
    private float ballGenSpeedLvl10;
    
    private float ballGenTimer;

    private int openedCards;
    private int bingosCount;
    private int jackpotLevel;

    [Header("Sound Controller")]

    [SerializeField] SoundController soundController;

    [Header("Tutorial")]

    public static TutorialManager tutorialManager;

    public static bool tutorialIsActive;

    void Awake()
    {
        gameIsLoaded = false;
        gameIsActive = false;

        tutorialIsActive = false;

        generalSave.Load();
        roomSave.Load();
        playerCoins = generalSave.GetCoins();
        playerMoney = generalSave.GetMoney();
        playerLvl = generalSave.GetLevel();

        tableController.Init(this);
        ballGenTimer = ballGenSpeed;

        rewardController.Init(this);

        soundController.Init();

        uiController.Init();

        tutorialManager = GetComponent<TutorialManager>();
        tutorialManager.Init(this, uiController);

        valuesManager.Init(this, tableController, rewardController);
    }

    private void FixedUpdate()
    {
        ActivateLoadingTimer();

        if (gameIsActive)
        {
            tableController.UpdateTable();
            UpdateBallGenerator();
        }

        if (tutorialIsActive)
        {
            tutorialManager.UpdateTutorial();
        }
    }

    public void ActivateLoadingTimer()
    {
        if (!gameIsLoaded)
        {
            if (loadTimer < firstLoadTime)
            {
                loadTimer += Time.fixedDeltaTime;
                uiController.UpdateLoadinrScreenBar();
            }
            else
            {
                gameIsLoaded = true;
                uiController.CloseLoadingScreen();
                uiController.OpenMainMenu();
                CheckTutorial();
            }
        }
    }

    public void SwitchGameIsActive(bool isActive) { gameIsActive = isActive; }

    public void SetValues(float _ballGenSpeed, float _ballGenSpeedLvl3, float _ballGenSpeedLvl10) 
    { 
        ballGenSpeed = _ballGenSpeed;
        ballGenSpeedLvl3 = _ballGenSpeedLvl3;
        ballGenSpeedLvl10 = _ballGenSpeedLvl10;
    }

    #region Game Code

    public IEnumerator StartGameLogic(int _tableCount)
    {
        tableController.StartGameLogic(_tableCount);

        rewardController.ResetEndBonus();
        rewardController.UpdateJackpotLevel(jackpotLevel);

        soundController.PlayStart();

        yield return new WaitForSeconds(3f);
        
        ballGenTimer = 0.2f;
        gameIsActive = true;
    }

    public void UpdateBallGenerator()
    {
        if (tableController.CheckAvailableBalls())
        {
            if (ballGenTimer > 0)
                ballGenTimer -= Time.fixedDeltaTime;

            else
            {
                tableController.GenerateNewBall();
                soundController.PlayNumber(tableController.GetNewBallNum());

                if (playerLvl <= 3)
                    ballGenTimer = ballGenSpeed;
                else if (playerLvl <= 10)
                    ballGenTimer = ballGenSpeedLvl3;
                else
                    ballGenTimer = ballGenSpeedLvl10;
            }
        }
    }

    public void CheckBallNumber(int ballNum) { tableController.CheckBallNumber(ballNum); }

    public void SetBallGenTimer(float time) { ballGenTimer = time; }

    public void SetChestSprites(Sprite usedChest) { tableController.SetChestSprite(usedChest); }
    #endregion

    #region Victory State

    public void EndRound()
    {
        gameIsActive = false;

        IncreasePlayerLvl(1);
        uiController.OpenRoundOverScreen();
        uiController.UpdateMainUI();
        rewardController.GetReward();
    }

    public void CloseEndRoundScreen()
    {
        playerLvl++;

        if (tutorialIsActive)
            UpdateTutorialProgress(9);
    }

    public void SetJackpotLevel(int lvl) { jackpotLevel = lvl; }
    #endregion

    #region Tutorial

    public void SetTutorialDone() 
    {
        uiController.SwitchDesignManagerVisible(true);
        tutorialIsActive = false;
        generalSave.SetTutorialStatus(1);
    }

    public void PrepareTutorialGame()
    {
        uiController.StartTutorialGame();

        tableController.StartTutorialGame();

        rewardController.ResetEndBonus();
        rewardController.AddBonus(false, 100);
    }

    public void StartTutorialGame()
    {
        if (!gameIsActive)
        {
            soundController.PlayStart();
            ballGenTimer = 3f;
            gameIsActive = true;
        }
    }

    public void CloseTutorialChip(int chipNum)
    {
        tableController.CloseTutorialChip(false, chipNum);
    }

    public void UpdateTutorialProgress(int step)
    {
        if (tutorialIsActive)
            tutorialManager.UpdateTutorialProgress(step);
    }

    private void CheckTutorial()
    {
        if (generalSave.GetTutorialStatus() == 1)
        {
            tutorialIsActive = false;
        }
        else if (generalSave.GetTutorialStatus() == 0)
        {
            tutorialIsActive = true;
            playerLvl = 1;
            tutorialManager.StartTutorial();
            uiController.SetTutorialMenu();
        }
    }

    public void SwitchKeepGoingScreen()
    {
        if(tutorialIsActive && generalSave.GetTutorialStatus() <= 9)
        {
            tutorialManager.SwitchKeepGoingScreen(true);
        }
    }

    #endregion

    #region Player Data

    public void CalculateCoins(bool isPlus, int count)
    {
        generalSave.CalculateCoins(isPlus, count);

        playerCoins = generalSave.GetCoins();
        uiController.UpdateMainUI();
    }

    public void CalculateMoney(bool isPlus, int count)
    {
        generalSave.CalculateMoney(isPlus, count);

        playerMoney = generalSave.GetMoney();
        uiController.UpdateMainUI();
    }

    public int GetPlayerLevel() { return playerLvl; }

    public void IncreasePlayerLvl(int lvl)
    {
        playerLvl += lvl;
        generalSave.UpdatePlayerLvl(playerLvl);
        uiController.UpdateMainUI();
    }

    public void ResetProgress()
    {
        generalSave.ResetSave();
        roomSave.ResetSave();
    }
    #endregion
}
