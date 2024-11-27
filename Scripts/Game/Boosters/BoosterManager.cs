using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterManager
{
    public bool hintActive => timeBoosters.hintActive;
    public bool batteryActive => timeBoosters.batteryActive;
    public bool autoBonusActive => timeBoosters.autoBonusActive;
    public bool wildDoubActive => timeBoosters.wildDoubActive;
    public bool timeDoubleXPIsActive => timeBoosters.doubleXpActive;

    public int autoDoubCount => countBoosters.autoDoubCount;
    public int tripleDoubCount => countBoosters.tripleDoubCount;
    public int airplaneCount => countBoosters.airplaneCount;
    public int doubleMoneyCount => countBoosters.doubleMoneyCount;

    public bool puzzleIsActive { get; private set; }
    public bool doubleCoinsIsActive { get; private set; }
    public bool airplaneIsActive { get; private set; }

    public int ballsPlus5Count { get; private set; }
    public int doubleProgressCount { get; private set; }
    public int autoBingoCount { get; private set; }

    public bool ballsPlus5IsActive { get; private set; }
    public bool doubleProgressIsActive { get; private set; }
    public bool autobingoIsActive { get; private set; }
    public bool tripleDoubIsActive { get; private set; }
    public bool roundDoubleXPIsActive { get; private set; }

    private TableController tableController;
    private TableSettings settings;
    private BoosterSave boosterSave;

    private BoosterTimersPart timeBoosters;
    private BoosterCountPart countBoosters;

    private float progress;
    private bool isActive;
    private Booster nextBooster;

    private float boosterTimeUpdate = 10f;
    private float boosterUpdateTimer;
    private int tutorialprogress;

    public BoosterManager(TableController table, TableSettings _settings)
    {
        EventBus.onRoundStarted += ResetBooster;
        EventBus.onChipClosed += UpdateProgress;
        EventBus.onRoundEnded += ResetActiveBoosters;

        tableController = table;
        settings = _settings;

        boosterSave = new BoosterSave();
        Load();

        timeBoosters = new BoosterTimersPart(boosterSave);
        countBoosters = new BoosterCountPart(boosterSave);

        boosterUpdateTimer = boosterTimeUpdate;
    }

    public void UpdateTimer()
    {
        if (boosterUpdateTimer > 0)
            boosterUpdateTimer -= Time.deltaTime;
        else
            UpdateBoostersTime();
    }

    public void SwitchEnhancerActive(bool isActive) => doubleProgressIsActive = isActive;

    public void CheckActiveBoosters()
    {
        timeBoosters.CheckTimeBoosters();
        tableController.SwitchDoubleXP(timeDoubleXPIsActive);

        CheckMenuBoosters();
        ResetProgressBar();
    }

    public System.TimeSpan GetTimerByType(Type type) { return timeBoosters.GetTimersByType(type); }

    public int GetCountByType(Type type) => countBoosters.GetCountByType(type);

    public void SetBoosterTime(Type type, float newTime) => timeBoosters.SetTimerByType(type, newTime);

    public void UseOneAirplane(bool isTutor)
    {
        tableController.CloseRandomChipWithoutAnim(1, 1.5f);

        if (!isTutor)
            countBoosters.UseOneAirplane();
    }

    public void SetBoosterCount(Type type, int count)
    {
        switch (type)
        {
            case Type.Plus5Balls:
                ballsPlus5Count += count;
                break;

            case Type.DoubleProgress:
                doubleProgressCount += count;
                break;

            case Type.AutoBingo:
                autoBingoCount += count;
                break;

            case Type.DoubleMoney:
            case Type.AutoDoub:
            case Type.TripleDoub:
            case Type.Airplane:
                countBoosters.SetCountByType(type, count);
                break;

            case Type.Crystal:
                GameController.Instance.CalculateCrystals(count);
                break;
        }

        Save();
    }

    public enum Type
    {
        Plus5Balls,
        DoubleProgress,
        AutoBingo,

        Hint,
        Battery,
        AutoBonus,
        WildDaub,
        DoubleXp,
        DoubleMoney,

        AutoDoub,
        TripleDoub,
        Airplane,

        Crystal
    }

    public Dictionary<Type, int> GetRewardsByLevel(int level)
    {
        Dictionary<Type, int> rewards = new Dictionary<Type, int>();

        switch (level)
        {
            case 2:
                rewards.Add(Type.Plus5Balls, 3);
                rewards.Add(Type.TripleDoub, 3);
                rewards.Add(Type.Crystal, 10);
                break;

            case 3:
                rewards.Add(Type.Airplane, 20);
                rewards.Add(Type.Crystal, 20);
                rewards.Add(Type.Plus5Balls, 3);
                break;

            case 4:
                rewards.Add(Type.DoubleProgress, 3);
                rewards.Add(Type.Plus5Balls, 3);
                rewards.Add(Type.Airplane, 30);
                break;

            case >= 5:
                rewards.Add(Type.AutoBingo, 3);
                rewards.Add(Type.DoubleProgress, 3);
                rewards.Add(Type.Plus5Balls, 3);
                break;
        }

        foreach (var reward in rewards)
            SetBoosterCount(reward.Key, reward.Value);

        return rewards;
    }

    public void ResetActiveBoosters()
    {
        ballsPlus5IsActive = false;
        doubleProgressIsActive = false;
        autobingoIsActive = false;
        airplaneIsActive = false;
        tripleDoubIsActive = false;
        doubleCoinsIsActive = false;
        roundDoubleXPIsActive = false;
    }

    private void UpdateBoostersTime()
    {
        timeBoosters.CheckTimeBoosters();
        boosterUpdateTimer = boosterTimeUpdate;
    }

    #region Menu Boosters

    public void CheckMenuBoosters()
    {
        if (ballsPlus5IsActive)
        {
            if (ballsPlus5Count > 0)
                ballsPlus5Count -= 1;
        }

        if (doubleProgressIsActive)
        {
            if (doubleProgressCount > 0)
                doubleProgressCount -= 1;
        }

        if (autobingoIsActive)
        {
            if (autoBingoCount > 0)
            {
                autoBingoCount -= 1;
            }
        }

        if (autoDoubCount > 0)
        {
            tableController.SwitchAutoDoub(true);
            countBoosters.UseOneBoosters(Type.AutoDoub);
        }

        if (tripleDoubCount > 0)
        {
            //tableController.CloseRandomChip(3);
            tripleDoubIsActive = true;
            countBoosters.UseOneBoosters(Type.TripleDoub);
            Debug.Log("Triple Doub = " + tripleDoubIsActive);
        }

        if (airplaneCount > 0)
        {
            airplaneIsActive = true;
        }

        if (doubleMoneyCount > 0)
        {
            tableController.SwitchDoubleMoney(true);
            countBoosters.UseOneBoosters(Type.DoubleMoney);
        }

        Save();
    }

    public void SwitchMenuBooster(Type type, bool isActive)
    {
        switch (type)
        {
            case Type.Plus5Balls:
                ballsPlus5IsActive = isActive;
                break;

            case Type.DoubleProgress:
                doubleProgressIsActive = isActive;
                break;

            case Type.AutoBingo:
                autobingoIsActive = isActive;
                break;
        }
    }

    #endregion

    #region Game Progress Bar

    public void ResetProgressBar()
    {
        progress = 0;
        isActive = false;

        UpdateProgressSprite(false);
    }

    private void UpdateProgress(int chipNum = 0, bool isTap = false)
    {
        if (!isActive && isTap)
        {
            if (progress < 4)
            {
                if (!doubleProgressIsActive)
                    progress += 1;
                else
                    progress += 2;

                isActive = false;
            }

            if (progress >= 4)
            {
                if (!GameController.tutorialIsActive)
                {
                    RandomizeType();
                }
                else
                {
                    if (GameController.tutorialManager.GetGameProgress() >= 5)
                        tutorialprogress = 2;

                    SetTutorialBooster();
                }

                progress = 4;
                isActive = true;
            }

            UpdateProgressSprite(true);
        }
    }

    private void UpdateProgressSprite(bool isActive)
    {
        //int boosterNum = ((int)nextBooster);

        UIController.Instance.UpdateTableBooster(progress / 4, nextBooster, isActive);
    }

    #endregion

    #region Game Booster

    public void ActivateBooster()
    {
        if (isActive)
        {
            if (GameController.tutorialIsActive)
                ActivateTutorialBooster();
            else
                ActivateBooster(nextBooster);

            ResetProgressBar();
            SoundController.Instance.PlaySound(SoundController.Sound.BonusUse);
        }
    }

    public IEnumerator ActivateBoosterWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ActivateBooster();
    }

    public enum Booster
    {
        GetX1,
        GetX2,
        Coins,
        Chest,

        Puzzle,
        DoubleXP,
        DoubleCoins,
        Dubber,
    }

    private void RandomizeType()
    {
        if (!puzzleIsActive)
        {
            List<Booster> boosterPull = new List<Booster>() { Booster.GetX1, Booster.GetX2, Booster.Coins, Booster.Chest };
            List<Booster> checkedList = CheckStartBonuses(boosterPull);

            int nextNum = Random.Range(0, checkedList.Count);

            nextBooster = checkedList[nextNum];
        }
        else
            nextBooster = Booster.Puzzle;
    }

    private void ActivateBooster(Booster type)
    {
        switch (type)
        {
            case Booster.GetX1:
                tableController.CloseRandomChip(1);
                break;

            case Booster.GetX2:
                tableController.CloseRandomChip(2);
                break;

            case Booster.Coins:
                tableController.GetCoinBonus();
                break;

            case Booster.Chest:
                tableController.AddRandomBonusChest();
                break;

            case Booster.Puzzle:
                EventBus.onPuzzleOpened?.Invoke();
                puzzleIsActive = false;
                break;

            case Booster.DoubleCoins:
                //tableController.ActivateDoubleCoins();
                doubleCoinsIsActive = true;
                tableController.SwitchDoubleCoins(true);
                break;

            case Booster.DoubleXP:
                tableController.SwitchDoubleXP(true);
                roundDoubleXPIsActive = true;
                //timeBoosters.SwitchDoubleXp(true);
                break;
        }
    }

    private List<Booster> CheckStartBonuses(List<Booster> boosterPull)
    {
        //if (doubleCoinsIsActive)
        //{
        //    boosterPull.Add(Booster.DoubleCoins);
        //}

        //if (!timeDoubleXPIsActive)
        //    boosterPull.Add(Booster.DoubleXP);

        if (!doubleCoinsIsActive)
        {
            float coinsRandom = Random.Range(0, 100);

            if (coinsRandom <= settings.boosterSettings.doubleCoinspercentage)
            {
                boosterPull.Add(Booster.DoubleCoins);
            }
        }

        if (!timeDoubleXPIsActive && !roundDoubleXPIsActive)
        {
            float xpRandom = Random.Range(0, 100);

            if (xpRandom <= settings.boosterSettings.doubleXPpercentage)
            {
                boosterPull.Add(Booster.DoubleXP);
            }
        }

        //if (doubleCoinsIsActive)
        //{
        //    float coinsRandom = Random.Range(0, 100);

        //    if (coinsRandom <= settings.boosterSettings.doubleCoinspercentage)
        //        doubleCoinsIsActive = true;
        //}

        //if (!doubleCoinsIsActive)
        //{
        //    float coinsRandom = Random.Range(0, 100);

        //    if (coinsRandom <= settings.boosterSettings.doubleCoinspercentage)
        //        doubleCoinsIsActive = true;
        //}

        //if (!timeDoubleXPIsActive && !roundDoubleXPIsActive)
        //{
        //    float xpRandom = Random.Range(0, 100);

        //    if (xpRandom <= settings.boosterSettings.doubleXPpercentage)
        //    {
        //        roundDoubleXPIsActive = true;
        //    }
        //}

        return boosterPull;
    }

    private void ResetBooster(int cardsCount = 0)
    {
        if (GameController.tutorialIsActive && GameController.tutorialManager.GetGameProgress() < 5)
            tutorialprogress = 0;

        if (GameController.Instance.playedRoundsCount > 4)
        {
            puzzleIsActive = true;
            //CheckDoubleBonusesActive();
        }

        ResetProgressBar();
    }

    #endregion

    #region Tutorial

    private void ActivateTutorialBooster()
    {
        if (GameController.tutorialManager.GetGameProgress() == 5)
            tutorialprogress = 2;

        switch (tutorialprogress)
        {
            case 0:
                tableController.CloseTutorialChip(true, 18);
                GameController.tutorialManager.ContinueBoosterStep();
                tutorialprogress++;
                break;

            case 1:
                ActivateBooster(nextBooster);
                tutorialprogress++;
                break;

            case 2:
                ActivateBooster(nextBooster);
                break;
        }
    }

    private void SetTutorialBooster()
    {
        switch (tutorialprogress)
        {
            case 0:
                nextBooster = Booster.GetX1;
                GameController.tutorialManager.UpdateGameProgress(3);
                break;

            case 1:
                nextBooster = Booster.Coins;
                break;

            case 2:
                RandomizeType();
                break;
        }
    }
    #endregion

    #region Save Load

    public void Save()
    {
        boosterSave.SaveMenuBoosters(new int[] { ballsPlus5Count, doubleProgressCount, autoBingoCount });
    }

    private void Load()
    {
        boosterSave.Load();

        ballsPlus5Count = boosterSave.ballsPlus5Count;
        doubleProgressCount = boosterSave.doubleProgressCount;
        autoBingoCount = boosterSave.autoBingoCount;
    }

    public void ResetSave()
    {
        boosterSave.ResetSave();
        Load();
    }

    #endregion
}
