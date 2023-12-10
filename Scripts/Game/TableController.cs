using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableController : MonoBehaviour
{
    private GameController gameController;

    [Header("Balls Controller")]

    [SerializeField] BallController ballController;
    [SerializeField] UIBallsCount ballsCount;
    [SerializeField] BallNumberGenerator ballGenerator;

    public int[] newNumbers;

    [Header("Cards Controller")]

    [SerializeField] GameObject tableCard1;
    [SerializeField] GameObject tableCard2;
    [SerializeField] GameObject tableCard3;
    [SerializeField] GameObject tableCard4;

    private List<CardController> cards;
    private int tableCount;

    [Header("Booster Controller")]

    [SerializeField] BoosterController boosterController;
    [SerializeField] RewardController rewardController;
    private int boosterCoinBonus = 100;

    [Header("Bingos")]

    [SerializeField] int maxBingos = 3;
    [SerializeField] int minBingos = 1;
    [SerializeField] int jackpotProcentage = 100;

    private int bingosInRound;
    private int bingosCounter;

    private List<int[]> bingoStrokesList;

    private int jackpotCount;

    [Header("Chest Bonus")]

    [SerializeField] int chestMinBonus = 30;
    [SerializeField] int chestMaxBonus = 70;
    [SerializeField] ParticleSystem moneyParticles;

    public void Init(GameController gc)
    {
        gameController = gc;

        boosterController.Init(this);
        ballController.Init();
    }

    public void UpdateTable()
    {
        CheckEndRound();
    }

    public void SetValues(int _boosterCoinBonus, int _minBingos, int _maxBingos, int _chestMinBonus, int _chestMaxBonus, int _jackpot)
    {
        boosterCoinBonus = _boosterCoinBonus;
        minBingos = _minBingos;
        maxBingos = _maxBingos;
        chestMinBonus = _chestMinBonus;
        chestMaxBonus = _chestMaxBonus;
        jackpotProcentage = _jackpot;
    }

    public int GetPlayerLvl() { return gameController.GetPlayerLevel(); }

    #region Prepare Before Round

    public void InitializeCards()
    {
        GetCardsOnTableList(tableCount);

        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].Init(this, boosterController);
        }
    }

    public void GenerateNewBallsList()
    {
        ballController.ResetBalls();
        ballsCount.UpdateCount(ballController.GetBallsLeft());
        ballsCount.ResetCount();
    }

    public void PrepareTableForGame()
    {
        bingosCounter = 0;
        bingosInRound = 0;

        newNumbers = new int[25];

        InitializeCards();
        CheckJackpotChance();
        RandomizeBingosInGame();

        foreach(CardController card in cards)
        {
            card.GenerateCard();
        }

        SetBallNumbersList(newNumbers);
    }

    public void UpdateBingosCounter(int count) 
    { 
        bingosCounter += count;
        UpdateBingoRewardBonus(bingosCounter);
    }

    private void SetBallNumbersList(int[] list)
    {
        ballController.SetListOfNumbers(list);
        ballController.ResetBalls();
    }

    #endregion

    #region Bingo Randomizer

    private void RandomizeBingosInGame()
    {
        int bingos = Random.Range(minBingos, maxBingos + 1);
        bingosInRound = bingos;
        ResetBingoStrokeList();
        
        Debug.Log($"Bingos In Round = {bingosInRound}");
        Debug.Log($"JackPot in Round = {jackpotCount}");

        int[] newArray = new int[25];
        int[] cardNumbers = RandomizeBingoNumbers();

        for (int i = 0; i < bingosInRound; i++)
        {
            int card = Random.Range(0, cards.Count);
            int strokeNum = Random.Range(0, bingoStrokesList.Count);
            int[] bingoStroke = bingoStrokesList[strokeNum];
            bingoStrokesList.Remove(bingoStroke);
            
            for (int j = 0; j < bingoStroke.Length; j++)
            {
                if (newArray[bingoStroke[j]] == 0)
                {
                    newArray[bingoStroke[j]] = cardNumbers[bingoStroke[j]];
                }

                cards[card].SetBingoNum(bingoStroke[j], cardNumbers[bingoStroke[j]]);
            }
        }

        if (jackpotCount == 1)
        {
            int card = Random.Range(0, cards.Count);
            int[] condition = new int[] { 0, 1, 2, 3, 4, 6, 12, 16, 20, 21, 22, 23, 24 };
            
            for (int i = 0; i < condition.Length; i++)
            {
                if (newArray[condition[i]] == 0)
                {
                    newArray[condition[i]] = cardNumbers[condition[i]];
                }

                cards[card].SetBingoNum(condition[i], cardNumbers[condition[i]]);
            }
        }
        
        newNumbers = newArray;
    }

    private void ResetBingoStrokeList()
    {
        bingoStrokesList = new List<int[]>();

        if (jackpotCount == 0)
        {
            bingoStrokesList.Add(new int[] { 0, 1, 2, 3, 4 });
            bingoStrokesList.Add(new int[] { 20, 21, 22, 23, 24 });
        }

        bingoStrokesList.Add(new int[] { 5, 6, 7, 8, 9 });
        bingoStrokesList.Add(new int[] { 10, 11, 12, 13, 14 });
        bingoStrokesList.Add(new int[] { 15, 16, 17, 18, 19 });
        bingoStrokesList.Add(new int[] { 0, 5, 10, 15, 20 });
        bingoStrokesList.Add(new int[] { 1, 6, 11, 16, 21 });
        bingoStrokesList.Add(new int[] { 2, 7, 12, 17, 22 });
        bingoStrokesList.Add(new int[] { 3, 8, 13, 18, 23 });
        bingoStrokesList.Add(new int[] { 4, 9, 14, 19, 24 });
        bingoStrokesList.Add(new int[] { 0, 6, 12, 18, 24 });
        bingoStrokesList.Add(new int[] { 4, 8, 12, 16, 20 });
        bingoStrokesList.Add(new int[] { 0, 4, 12, 20, 24 });
    }

    private void CheckJackpotChance()
    {
        int num = Random.Range(1, 100);

        Debug.Log($"JackPot procentage - {num}");

        if (num > jackpotProcentage)
            jackpotCount = 0;

        else
            jackpotCount = 1;
    }

    private void SetJackpot()
    {
        int card = Random.Range(0, cards.Count);
        int[] condition = new int[] { 0, 1, 2, 3, 4, 6, 12, 16, 20, 21, 22, 23, 24 };

        for (int i = 0; i < condition.Length; i++)
        {
            cards[card].SetBingoNum(i, newNumbers[i]);
        }
    }

    private int[] RandomizeBingoNumbers()
    {
        int[] bingoArray = new int[25];

        int[] b = new int[5];
        int[] i = new int[5];
        int[] n = new int[5];
        int[] g = new int[5];
        int[] o = new int[5];

        b = RandomizeColumnNumbers(1, 16);
        i = RandomizeColumnNumbers(16, 31);
        n = RandomizeColumnNumbers(31, 46);
        g = RandomizeColumnNumbers(46, 61);
        o = RandomizeColumnNumbers(61, 76);

        for (int s = 0; s < bingoArray.Length; s++)
        {
            if (bingoArray[s] == 0)
            {
                if (s < 5)
                    bingoArray[s] = b[s];

                if (s >= 5 && s < 10)
                    bingoArray[s] = i[s - 5];

                if (s >= 10 && s < 15)
                    bingoArray[s] = n[s - 10];

                if (s >= 15 && s < 20)
                    bingoArray[s] = g[s - 15];

                if (s >= 20 && s < bingoArray.Length)
                    bingoArray[s] = o[s - 20];
            }
        }

        return bingoArray;
    }

    private int[] RandomizeColumnNumbers(int minValue, int maxValue)
    {
        int[] values = new int[5];

        for (int i = 0; i < values.Length; i++)
        {
            bool contains;
            int next;

            do
            {
                next = Random.Range(minValue, maxValue);
                contains = false;

                for (int index = 0; index < i; index++)
                {
                    int n = values[index];
                    if (n == next)
                    {
                        contains = true;
                        break;
                    }
                }
            }

            while (contains);

            values[i] = next;
        }

        return values;
    }

    #endregion

    #region Main Game Code

    public void StartGameLogic(int tableCount)
    {
        ballController.GetCardsCount(tableCount);

        GetCardsOnTableList(tableCount);
        PrepareTableForGame();

        boosterController.ResetBooster();
        ballsCount.UpdateCount(ballController.GetBallsLeft());
        ballsCount.ResetCount();
    }

    public void CheckEndRound()
    {
        if (ballController.GetBallsLeft() <= 0)
        {
            if (ballsCount.GetTimer() > 0)
                ballsCount.UpdateTimer();

            else
                gameController.EndRound();
        }
    }

    public void SwitchGameIsActive(bool isPause) { gameController.SwitchGameIsActive(isPause); }

    #endregion

    #region Bonuses

    public void GetBonusCount(bool isCoins, int count) { rewardController.UpdateEndBonus(isCoins, count); }

    public void GetChestBonus(Vector3 chestPos) 
    {
        int bonus = Random.Range(chestMinBonus, chestMaxBonus + 1);
        GetBonusCount(false, bonus);
        moneyParticles.transform.position = new Vector3(chestPos.x, chestPos.y, 0);
        moneyParticles.Play();
    }

    public void GetCoinBonus() { GetBonusCount(true, boosterCoinBonus); }

    public void UpdateBingoRewardBonus(int bingoCount) { rewardController.UpdateBingoBonus(bingoCount); }

    public void UpdateJackpotRewardBonus() { rewardController.UpdateJackpotBonus(); }

    public bool CheckJackpotIsInGame() 
    { 
        if(jackpotCount >= 1) 
            return true; 
        else
            return false;
    }

    #endregion

    #region Card Part

    public void GetCardsOnTableList(int _tableCount)
    {
        cards = new List<CardController>();
        tableCount = _tableCount;
        GameObject table = tableCard1;

        if (tableCount == 1)
            table = tableCard1;

        if (tableCount == 2)
            table = tableCard2;

        if (tableCount == 3)
            table = tableCard3;

        if (tableCount == 4)
            table = tableCard4;

        for (int i = 0; i < tableCount; i++)
        {
            cards.Add(table.transform.GetChild(i).GetComponent<CardController>());
        }
    }

    public void CloseRandomChip()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].CloseRandomChip();
        }
    }
    
    public void AddRandomBonusChest()
    {
        foreach(CardController card in cards)
        {
            card.AddRandomBonusChest();
        }
    }

    public void SetChestSprite(Sprite usedChest)
    {
        foreach(CardController card in cards)
        {
            card.SetChestSprite(usedChest);
        }
    }
    #endregion

    #region Ball Part

    public void CheckBallNumber(int ballNum)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].CheckBallNumber(ballNum);
        }
    }

    public void GenerateNewBall()
    {
        ballController.GenerateNewBall();
        CheckBallNumber(GetNewBallNum());
        ballsCount.UpdateCountAnimation(ballController.GetBallsLeft());
    }

    public bool CheckAvailableBalls() { return ballController.CheckAvailableBalls(); }

    public int GetNewBallNum() { return ballController.GetBall(); }
    #endregion

    #region Tutorial

    public void StartTutorialGame()
    {
        ballController.GetCardsCount(1);
        GetCardsOnTableList(1);
        PrepareTutorialTable();
        boosterController.ResetBooster();
    }

    public void PrepareTutorialTable()
    {
        int[] ballsList = new int[15] { 6, 11, 50, 23, 47, 74, 41, 18, 9, 8, 45, 67, 15, 25, 3};

        ballController.ResetBalls();
        ballController.GetTutorialListOfNumbers(ballsList);
        cards[0].Init(this, boosterController);
        cards[0].GenerateTutorialCard();
        ballsCount.UpdateCount(ballController.GetBallsLeft());
        ballsCount.ResetCount();
    }

    public void CloseTutorialChip(bool isBooster, int chipNum)
    {
        cards[0].CloseTutorialChip(isBooster, chipNum);
    }

    public void SwitchKeepGoingScreen()
    {
        gameController.SwitchKeepGoingScreen();
    }
    #endregion
}
