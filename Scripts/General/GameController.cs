
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public FirebaseController firebaseController { get; private set; }
    public GeneralSave generalSave { get; private set; }

    public BoosterManager boosterManager { get; private set; }

    public int playerCoins => statistics.playerCoins;
    public int playerMoney => statistics.playerMoney;
    public int playerCrystals => statistics.playerCrystals;

    public int playedRoundsCount => statistics.playedRoundsCount;

    public int currentXPLevel => statistics.currentXPLevel;
    public int currentXPPoints => statistics.currentXPPoints;
    public int previousXPPointsValue => statistics.previousXPPointsValue;
    public int xpPointsToNextLevel => statistics.playerXPLevelsArray[currentXPLevel];

    public bool gameIsActive { get; private set; }

    [SerializeField] CoreGameSettings settings;
    [SerializeField] UIController uiController;
    [SerializeField] TableController tableController;
    [SerializeField] SoundController soundController;

    private PlayerStatistics statistics;
    private RewardController rewardController;

    private float loadTimer;
    private bool gameIsLoaded;
    private int bonusMoney;
    private int bonusCrystals;
    private int jackpotLevel;

    public static TutorialManager tutorialManager;
    public static bool tutorialIsActive;

    private int tutorialRoomProgress;
    private int tutorialGameProgress;

    private void Awake()
    {
        Instance = this;
        gameIsLoaded = false;
        SwitchGameIsActive(false);

        tutorialIsActive = false;
        tutorialManager = GetComponent<TutorialManager>();

        generalSave = new GeneralSave();
        generalSave.Load();

        statistics = new PlayerStatistics(generalSave, settings.statsSettings.xpPointsOnLevelArray);
        rewardController = new RewardController(settings.tableSettings);
        boosterManager = new BoosterManager(tableController, settings.tableSettings);
        
        tableController.Init(settings.tableSettings, rewardController, boosterManager);
        soundController.Init();
        uiController.Init(settings.uiSettings, settings.tableSettings);
        tutorialManager.Init(this, uiController.GetCurrentRoom());

        firebaseController = new FirebaseController();

        EventBus.onRoundEnded += CloseEndRoundScreen;
    }

    private void FixedUpdate()
    {
        ActivateLoadingTimer();

        if (gameIsActive)
        {
            tableController.UpdateTable();
        }
        else
        {
            boosterManager.UpdateTimer();
        }

        if (tutorialIsActive)
        {
            tutorialManager.UpdateTutorial();
        }

        tableController.UpdateAlways();


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            uiController.CallExitScreen();
        }
    }

    public void ActivateLoadingTimer()
    {
        if (!gameIsLoaded)
        {
            if (loadTimer < settings.uiSettings.firstLoadingTime)
                loadTimer += Time.fixedDeltaTime;

            else
            {
                gameIsLoaded = true;
                uiController.CallScreen(UIController.Menu.MainMenu);
                uiController.CheckRoomIsPassed();
                CheckTutorial();

                AppMetrica.reportEvent("Loading_end", "");
            }
        }
    }

    public void SetFirebaseSettings()
    {
        uiController.SetFirebaseSettings();
    }

    public void SwitchGameIsActive(bool isActive) => gameIsActive = isActive;

    #region Game Code

    public void StartGameLogic(int _tableCount)
    {
        tutorialManager.SwitchTutorialVisibility(false);

        if (tutorialIsActive && tutorialManager.GetGameProgress() < 5)
            StartTutorialGame();

        else
        {
            EventBus.onRoundStarted?.Invoke(_tableCount);

            tableController.StartGameLogic(_tableCount, jackpotLevel);
            rewardController.UpdateJackpotLevel(jackpotLevel);
            soundController.PlayStart();
            SetBallGenTimer(4f);
            SwitchGameIsActive(true);
        }

        AppMetrica.reportEvent("Level_start", "" + playedRoundsCount);
    }

    public void CheckBallNumber(int ballNum) { tableController.CheckBallNumber(ballNum); }

    public void SetBallGenTimer(float time) { tableController.SetBallGenTimer(time); }

    #endregion

    #region Victory State

    public void EndRound()
    {
        SwitchGameIsActive(false);

        IncreaseRoundCount(1);
        int[] reward = rewardController.CheckReward();
        UIController.Instance.UpdateEndRoundScreen(reward[0], reward[1]);
        uiController.OpenRoundOverScreen();
        uiController.UpdateMainUI();
    }

    public void StartCardsEndRoundMove() => tableController.SwitchCardsPlace();

    public void CheckTutorialSteps()
    {
        if (tutorialIsActive)
        {
            tutorialManager.HideGameTutorial();
            tutorialManager.SwitchTutorialVisibility(true);

            if (tutorialManager.GetRoomProgress() < 4)
            {
                tutorialManager.UpdateRoomProgress(4);
                tutorialManager.UpdateGameProgress(5);
            }
        }
    }

    public void SetJackpotLevel(int lvl) { jackpotLevel = lvl; }

    private void CloseEndRoundScreen()
    {
        tableController.ResetBoosters();
        boosterManager.ResetActiveBoosters();

        if (generalSave.rateUs == 0)
            EventBus.onRewardRecieved?.Invoke();
        else
            MaxSdkManager.Instance.ShowInterstitial("EndOfRound");

        CheckNumStep();
    }

    #endregion

    #region Tutorial

    public void FinishTutorial()
    {
        uiController.SwitchDesignManagerVisible(true);
        tutorialIsActive = false;
        generalSave.SetTutorialStatus(1);
    }

    public void PrepareTutorialGame()
    {
        uiController.StartTutorialGame();

        tableController.StartTutorialGame();
        EventBus.onRoundStarted?.Invoke(1);

        rewardController.AddBonus(false, 100);
    }

    public void StartTutorialGame()
    {
        if (!gameIsActive)
        {
            soundController.PlayStart();
            SetBallGenTimer(3f);
            SwitchGameIsActive(true);
        }
    }

    public void CloseTutorialChip(int chipNum)
    {
        tableController.CloseTutorialChip(false, chipNum);
    }

    private void CheckTutorial()
    {
        tutorialManager.HideSteps();

        if (generalSave.tutorStatus == 1)
        {
            tutorialIsActive = false;
        }
        else if (generalSave.tutorStatus == 0)
        {
            tutorialIsActive = true;

            tutorialRoomProgress = generalSave.GetTutorialProgress()[0];
            tutorialGameProgress = generalSave.GetTutorialProgress()[1];

            if (tutorialRoomProgress < 4)
            {
                ResetProgress();
                statistics.SetParametersByTutorial(true);
                tutorialManager.StartTutorial();
                uiController.SetTutorialMenu();

                tutorialRoomProgress = 0;
                tutorialGameProgress = 0;
                UpdateTutorialProgress();
            }
            else
            {
                statistics.SetParametersByTutorial(false);
                tutorialManager.UpdateRoomProgress(tutorialRoomProgress);
                tutorialManager.UpdateGameProgress(5);
            }
        }
    }

    private void CheckNumStep()
    {
        if (playedRoundsCount == 20)
        {
            tutorialManager.CallScreenByNum(6);
        }
    }

    public void UpdateTutorialProgress()
    {
        tutorialRoomProgress = tutorialManager.GetRoomProgress();
        tutorialGameProgress = tutorialManager.GetGameProgress();

        generalSave.UpdateTutorialProgress(tutorialRoomProgress, tutorialGameProgress);
    }

    public void SkipTutorial()
    {
        ResetProgress();
        generalSave.SetTutorialStatus(1);
        IncreaseRoundCount(1);
        SceneManager.LoadScene(0);
    }
    #endregion

    #region Player Data

    public void CalculateCoins(int count)
    {
        statistics.CalculateCoins(count);
        uiController.UpdateMainUI();
    }

    public void CalculateMoney(int count)
    {
        statistics.CalculateMoney(count);
        uiController.UpdateMainUI();
        bonusMoney = count;
    }
    
    public void CalculateCrystals(int count)
    {
        statistics.CalculateCrystals(count);
        uiController.UpdateMainUI();
        bonusCrystals = count;
    }
    
    public void CalculateXP(int points)
    {
        statistics.CalculateXP(points);
        uiController.StartXPProgressAnimation();
        Debug.Log("XP points = " + currentXPPoints + " XP bonus = " + points);
    }

    public int GetCurrentRoomNum() { return generalSave.currentRoomNum + 1; }

    public void IncreaseRoundCount(int lvl)
    {
        statistics.IncreaseRoundCount(lvl);
        uiController.UpdateMainUI();
    }

    public void CheckRateUs()
    {
        if (generalSave.rateUs == 1)
            return;

        if (playedRoundsCount > 4 && bonusMoney > 100)
        {
            generalSave.UpdateRateUs();
            UIController.Instance.CallScreen(UIController.Menu.RateUsScreen);
        }
    }

    #endregion

    public void ResetProgress()
    {
        generalSave.ResetSave();
        uiController.ResetSave();
        statistics.ResetStatistics();
        boosterManager.ResetSave();
        PlayerPrefs.DeleteAll();
    }

    private void OnDestroy()
    {
        EventBus.onRoundEnded -= CloseEndRoundScreen;
    }
}
