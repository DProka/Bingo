
using System.Collections.Generic;
using UnityEngine;

public class TableController : MonoBehaviour
{
    public bool autoGameActive { get; private set; }
    public bool autoDoubActive { get; private set; }
    public bool wildDoubActive { get; private set; }
    public bool doubleMoneyActive { get; private set; }
    public bool doubleCoinsActive { get; private set; }
    public bool doubleXpActive { get; private set; }

    [SerializeField] TableCardsPart cardsPart;
    [SerializeField] Transform boosterPos;

    private TableSettings settings;

    private RewardController rewardController;
    private BallController ballController;

    private int[] newNumbers;
    private float ballGenTimer;

    private int bingosInRound;
    private int bingosCounter;

    private int jackpotCount;
    private int jackpotLevel;

    public void Init(TableSettings _settings, RewardController _rewardController, BoosterManager _boosterManager)
    {
        EventBus.onChipClosed += CheckAutoGame;

        settings = _settings;
        rewardController = _rewardController;
        ballController = GetComponent<BallController>();
        ballController.Init(settings);
        cardsPart.Init(this, settings);

        autoGameActive = false;
    }

    public void UpdateTable()
    {
        UpdateBallGenerator();
    }

    public void UpdateAlways()
    {
        cardsPart.UpdateCards();
    }

    public bool SwitchAutoGame(bool isActive) => autoGameActive = isActive;
    public bool SwitchAutoDoub(bool isActive) => autoDoubActive = isActive;
    public bool SwitchDoubleMoney(bool isActive) => doubleMoneyActive = isActive;
    public bool SwitchDoubleCoins(bool isActive) => doubleCoinsActive = isActive;
    public bool SwitchDoubleXP(bool isActive) => doubleXpActive = isActive;

    private void CheckAutoGame(int a = 0, bool b = true)
    {
        if (autoGameActive || GameController.Instance.boosterManager.autoBonusActive)
            StartCoroutine(GameController.Instance.boosterManager.ActivateBoosterWithDelay(1f));
    }

    #region Prepare Before Round

    public void UpdateBingosCounter(int count)
    {
        bingosCounter += count;
        UpdateBingoRewardBonus(bingosCounter);
    }

    private void PrepareTableForGame()
    {
        bingosCounter = 0;
        bingosInRound = 0;

        CheckBooster();

        cardsPart.InitializeCards(jackpotLevel);
        jackpotCount = TableCalculations.CheckJackpotChance(settings.jackpotProcentage);
        RandomizeBingosInGame();
        cardsPart.GenerateCards();

        SetBallNumbersList(newNumbers);

        CheckAutobingo();
    }

    private void SetBallNumbersList(int[] list) => ballController.SetListOfNumbers(list);

    private void CheckBooster()
    {
        if (GameController.Instance.boosterManager.doubleProgressIsActive)
            UIAnimationScreen.Instance.StartDoubleProgressAnimation();

        wildDoubActive = GameController.Instance.boosterManager.wildDoubActive;

        //if(GameController.Instance.boosterManager.timeDoubleXPIsActive)
        //    doubleXpActive = true;

        GameController.Instance.boosterManager.CheckActiveBoosters();

        SetUiRoundBoosters();
    }

    private void SetUiRoundBoosters()
    {
        bool[] statuses = new bool[] { wildDoubActive, autoDoubActive, GameController.Instance.boosterManager.hintActive };
        UIController.Instance.SetRoundTableBoosters(statuses);
    }

    #endregion

    #region Bingo Randomizer

    private void RandomizeBingosInGame()
    {
        int ballsCount = GameController.Instance.boosterManager.ballsPlus5IsActive ? 30 : 25;

        bingosInRound = Random.Range(settings.minBingosInRound, settings.maxBingosInRound + 1);
        newNumbers = cardsPart.RandomizeBingosInGame(bingosInRound, jackpotCount, ballsCount);

        Debug.Log($"Bingos_In_Round = {bingosInRound}, JackPot_In_Round = {jackpotCount}, Balls count = {newNumbers.Length}");
    }

    #endregion

    #region Main Game Code

    public void StartGameLogic(int _tableCount, int _jackpotLevel)
    {
        jackpotLevel = _jackpotLevel;
        ballController.SetCardsCount(_tableCount);

        GetCardsOnTableList(_tableCount);
        PrepareTableForGame();

        UIBallsCount.Instance.UpdateCount(ballController.ballsNumList.Count);
        EventBus.onWindowOpened?.Invoke();
    }

    private void CheckEndRound()
    {
        if (ballController.ballsNumList.Count <= 0)
        {
            EventBus.onBallsIsEmpty?.Invoke();

        }
    }
    #endregion

    #region Bonuses

    public void GetChestBonus(Vector3 chestPos)
    {
        int bonus = Random.Range(settings.minChestReward, settings.maxChestReward + 1);
        int xp = settings.xpPerChip;

        if (doubleMoneyActive)
            bonus = bonus * 2;

        if (doubleXpActive)
            xp = xp * 2;

        GetBonusCount(false, bonus);
        rewardController.UpdateXPBonus(xp);
        UIAnimationScreen.Instance.GetChestBonus(chestPos, bonus, xp);
    }

    public void GetCoinBonus()
    {
        int bonus = settings.boosterSettings.boosterCoinBonus;
        if (doubleCoinsActive)
            bonus = bonus * 2;
        //UIAnimationScreen.Instance.GetCoinBonus(settings.boosterSettings.boosterCoinBonus);
        UIAnimationScreen.Instance.GetCoinBonus(bonus);
        //GetBonusCount(true, settings.boosterSettings.boosterCoinBonus);
        GetBonusCount(true, bonus);
    }

    public void GetXPPoints(Vector2 chipPos)
    {
        int count = settings.xpPerChip;

        if (doubleXpActive)
            count = count * 2;

        rewardController.UpdateXPBonus(count);
        UIAnimationScreen.Instance.GetXPBonus(chipPos, count);
    }

    public void GetPuzzleBonus(Vector3 puzzlePos) { }
    
    public void GetBonusCount(bool isCoins, int count) => rewardController.UpdateEndBonus(isCoins, count);

    public void ActivateBooster() => GameController.Instance.boosterManager.ActivateBooster();

    public void UpdateBingoRewardBonus(int bingoCount) => rewardController.UpdateBingoBonus(bingoCount);

    public void UpdateJackpotRewardBonus() => rewardController.UpdateJackpotBonus();

    public bool CheckJackpotIsInGame() { return jackpotCount >= 1; }

    public void ResetBoosters()
    {
        SwitchAutoDoub(false);
        wildDoubActive = false;
        doubleCoinsActive = false;
    }

    #endregion

    #region Card Part

    public void GetCardsOnTableList(int count) => cardsPart.GetCardsOnTableList(count);

    public void CloseRandomChip(int chipCount) => cardsPart.CloseRandomChip(chipCount);

    public void CloseRandomChipWithoutAnim(int chipCount, float animDelay) => cardsPart.CloseRandomChipWithoutAnim(chipCount, animDelay);

    public void AddRandomBonusChest() => cardsPart.AddRandomBonusChest();

    public void CheckAutobingo() => cardsPart.CheckBoostersOnStart();

    public void SetChestSprite(Sprite usedChest) => cardsPart.SetChestSprite(usedChest);

    public void SwitchCardsPlace() => cardsPart.SwitchRoundOverCardsPlace();

    public void CallBingoAnim(Transform parent, int num, int[] currency, bool isJackPot) => UIAnimationScreen.Instance.CallBingoAnimation(parent, num, currency, isJackPot);

    #endregion

    #region Ball Part

    public void CheckBallNumber(int ballNum) => cardsPart.CheckBallNumber(ballNum);

    public void GenerateNewBall()
    {
        ballController.GenerateNewBall();
        CheckBallNumber(GetNewBallNum());
        UIBallsCount.Instance.UpdateCountAnimation(ballController.ballsNumList.Count);

        CheckEndRound();
    }

    public int GetNewBallNum() { return ballController.GetBall(); }

    public void SetBallGenTimer(float time) { ballGenTimer = time; }

    private void UpdateBallGenerator()
    {
        if (ballController.CheckAvailableBalls())
        {
            if (ballGenTimer > 0)
                ballGenTimer -= Time.fixedDeltaTime;

            else
            {
                GenerateNewBall();
                SoundController.Instance.PlayNumber(GetNewBallNum());

                if (GameController.Instance.playedRoundsCount <= 3)
                    ballGenTimer = settings.ballGenSpeed;
                else if (GameController.Instance.playedRoundsCount <= 10)
                    ballGenTimer = settings.ballGenSpeedLvl3;
                else
                    ballGenTimer = settings.ballGenSpeedLvl10;
            }
        }
    }
    #endregion

    #region Tutorial

    public void StartTutorialGame()
    {
        ballController.SetCardsCount(1);
        GetCardsOnTableList(1);
        PrepareTutorialTable();
    }

    public void CloseTutorialChip(bool isBooster, int chipNum) => cardsPart.CloseTutorialChip(isBooster, chipNum);

    private void PrepareTutorialTable()
    {
        int[] ballsList = new int[15] { 6, 11, 50, 23, 47, 74, 41, 18, 9, 8, 45, 67, 15, 25, 3 };

        ballController.GetTutorialListOfNumbers(ballsList);
        cardsPart.PrepareTutorialCard();
        UIBallsCount.Instance.UpdateCount(ballController.ballsNumList.Count);
        CheckBooster();
    }
    #endregion

    private void OnDestroy()
    {
        EventBus.onChipClosed -= CheckAutoGame;
    }
}
