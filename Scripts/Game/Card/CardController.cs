
using System.Collections;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class CardController : MonoBehaviour
{
    [Header("Main")]

    [SerializeField] CardNumbers cardNumbers;
    [SerializeField] CardPlayerGrid playerGrid;

    private TableController tableController;
    private TableSettings settings;
    private int id;
    private bool cardIsOpen;
    private int jackpotLevel;

    private int[] cardNumbersArray;
    private int[] openNumbersArray;
    private int[] chestArray;

    private int openedBingoCount;
    private int[] openedBingosArray;
    private int openedJackpot;
    private int bingoCount;

    private bool bingoScreenActive;
    private float bingoScreenTimer;

    private bool isJackPot;

    public void Init(TableController table, TableSettings _settings, int _id, int _jackpotLevel)
    {
        tableController = table;
        settings = _settings;
        id = _id;
        jackpotLevel = _jackpotLevel;

        cardNumbersArray = new int[25];
        openNumbersArray = new int[25];
        chestArray = new int[25];

        openedBingosArray = new int[13];
        openedJackpot = 0;
        bingoCount = 0;

        cardNumbers.Init();
        playerGrid.Init(this);
    }

    public void UpdateCard()
    {
        UpdateBingoScreenAnimation();
    }

    public void GenerateCard()
    {
        cardNumbers.ResetNumbersVisibility();
        cardNumbers.FillEmptyNumbers(cardNumbersArray);

        openNumbersArray = new int[25];
        openedBingosArray = new int[13];
        playerGrid.ResetChips();

        if (GameController.Instance.playedRoundsCount < 4)
        {
            openNumbersArray[12] = 1;
            cardNumbers.TurnOffTextNum(12);
            playerGrid.CloseChip(12, false);
        }

        SetRandomStartChests();
        cardIsOpen = true;
        openedBingoCount = 0;
    }

    public void SetBingoNum(int place, int num) { cardNumbersArray[place] = num; }

    public void CheckBallNumber(int bNum)
    {
        if (cardIsOpen)
        {
            for (int i = 0; i < cardNumbersArray.Length; i++)
            {
                if (cardNumbersArray[i] == bNum && openNumbersArray[i] == 0)
                {
                    if (GameController.Instance.playedRoundsCount >= 4)
                    {
                        playerGrid.SetChipClick(i);

                        if (GameController.Instance.boosterManager.hintActive)
                            playerGrid.ActivateChipFrame(i);

                        if (tableController.autoGameActive || tableController.autoDoubActive)
                            StartCoroutine(CloseChipWithDelay(i));
                    }
                    else
                        StartCoroutine(playerGrid.OpenChip(i));

                    GameController.tutorialManager.ActivateStepWithHand(i, playerGrid.chipArray[i].transform.position);
                    break;
                }
            }
        }
    }

    public void ConfirmOpenedChip(int num)
    {
        openNumbersArray[num] = 1;
        GameController.tutorialManager.CloseStepWithHand();
    }

    public int[] GetNumbersArray() { return cardNumbersArray; }

    public bool CheckNumberIsFree(int number)
    {
        bool isFree = false;

        for (int i = 0; i < cardNumbersArray.Length; i++)
        {
            if (cardNumbersArray[i] == number && openNumbersArray[i] == 0)
            {
                isFree = true;
                return isFree;
            }
        }

        return isFree;
    }

    #region Chip Code

    public IEnumerator OpenChip(int num)
    {
        cardNumbers.TurnOffTextNum(num);
        JackpotDisplay.Instance.OpenChip(id, num);

        yield return new WaitForSeconds(0.2f);

        if (playerGrid.chipArray[num].currentType != ChipScript.Type.AutoBingo)
            CheckVictoryCondition();
        else
            GetAutobingo(num);
    }

    public IEnumerator CloseRandomChip(float delay)
    {
        if (cardIsOpen && CheckFreeChips())
        {
            int chipNum = GetRandomFreeChipNum();
            openNumbersArray[chipNum] = 1;

            yield return new WaitForSeconds(delay);

            //openNumbersArray[chipNum] = 1;
            UIAnimationScreen.Instance.StartCometAnimation(playerGrid.chipArray[chipNum].transform.position);

            yield return new WaitForSeconds(settings.bingoAnumationTimescale);

            playerGrid.CloseChip(chipNum, false);
        }
    }
    
    public IEnumerator CloseRandomChipWithoutAnim(float delay)
    {
        if (cardIsOpen && CheckFreeChips())
        {
            int chipNum = GetRandomFreeChipNum();
            openNumbersArray[chipNum] = 1;

            yield return new WaitForSeconds(delay);

            playerGrid.CloseChip(chipNum, false);
        }
    }

    public void SetPuzzleChip(int[] ballNums)
    {
        List<int> numbers = new List<int>();

        for (int i = 0; i < ballNums.Length; i++)
        {
            for (int j = 0; j < cardNumbersArray.Length; j++)
            {
                if (cardNumbersArray[j] == ballNums[i] && openNumbersArray[j] == 0)
                {
                    numbers.Add(j);
                    continue;
                }
            }
        }

        int rand = Random.Range(0, numbers.Count);
        playerGrid.SetPuzzleChip(numbers[rand]);
    }

    public IEnumerator AddRandomAutoBingo(float delay)
    {
        if (cardIsOpen && CheckFreeChips())
        {
            yield return new WaitForSeconds(delay);

            int num = GetRandomFreeChipNum();

            UIAnimationScreen.Instance.StartCometAnimation(playerGrid.chipArray[num].transform.position);

            yield return new WaitForSeconds(settings.bingoAnumationTimescale);

            playerGrid.SetAutoBingoChip(num);

            Debug.Log("Autobingo ChipNum = " + num);
        }
    }

    public bool CheckFreeChips()
    {
        bool hasChips = false;

        if(openNumbersArray.Contains(0))
            hasChips = true;

        return hasChips;
    }

    private IEnumerator CloseChipWithDelay(int num)
    {
        yield return new WaitForSeconds(1f);

        playerGrid.CloseChip(num, true);
    }

    private int GetRandomFreeChipNum()
    {
        int chipNum = 0;

        List<int> openedNumbers = new List<int>();

        for (int i = 0; i < openNumbersArray.Length; i++)
        {
            if (openNumbersArray[i] == 0)
            {
                openedNumbers.Add(cardNumbersArray[i]);
            }
        }

        int num = Random.Range(0, openedNumbers.Count);

        for (int i = 0; i < cardNumbersArray.Length; i++)
        {
            if (cardNumbersArray[i] == openedNumbers[num])
            {
                chipNum = i;

                break;
            }
        }

        return chipNum;
    }

    #endregion

    #region Chest Code

    private void SetRandomStartChests()
    {
        List<int> openedNumbers = new List<int>();

        for (int i = 0; i < openNumbersArray.Length; i++)
        {
            if (openNumbersArray[i] == 0 && chestArray[i] == 0)
            {
                openedNumbers.Add(i);
            }
        }

        int chestCount = Random.Range(1, 4);

        for (int i = 0; i < chestCount; i++)
        {
            int num = Random.Range(0, openedNumbers.Count);

            chestArray[openedNumbers[num]] = 1;
            playerGrid.SetChestChip(openedNumbers[num]);
            openedNumbers.Remove(openedNumbers[num]);
        }
    }

    public void GetChestBonus(Vector3 chestPos) { tableController.GetChestBonus(chestPos); }
    public void GetXPBonus(Vector3 chestPos) { tableController.GetXPPoints(chestPos); }
    public void GetPuzzleBonus(Vector3 chestPos) { tableController.GetPuzzleBonus(chestPos); }
    //public void GetAutoBingo(int chipNum) { tableController.GetPuzzleBonus(chestPos); }

    public IEnumerator AddRandomBonusChest(float delay)
    {
        if (cardIsOpen && CheckFreeChips())
        {
            yield return new WaitForSeconds(delay);

            int num = GetRandomFreeChipNum();

            chestArray[num] = 1;
            UIAnimationScreen.Instance.StartCometAnimation(playerGrid.chipArray[num].transform.position);

            yield return new WaitForSeconds(settings.bingoAnumationTimescale);

            playerGrid.SetChestChip(num);
        }
    }

    public void SetChestSprite(Sprite usedChest) { playerGrid.SetChestSprite(usedChest); }

    #endregion

    #region Victory Conditions

    public void CheckVictoryCondition()
    {
        isJackPot = CheckJackpotCondition();

        if (!isJackPot)
            CheckBingoCondition();
    }

    private void CheckBingoCondition()
    {
        for (int i = 0; i < openedBingosArray.Length; i++)
        {
            if (openedBingosArray[i] == 0)
            {
                int[] nums = TableCalculations.GetConditionNumbers(i);
                int openedChips = 0;

                for (int j = 0; j < nums.Length; j++)
                {
                    openedChips += openNumbersArray[nums[j]];
                }

                if (openedChips == 5)
                {
                    openedBingosArray[i] = 1;
                    openedBingoCount++;
                    tableController.UpdateBingosCounter(1);
                    playerGrid.AnimateBingoChips(nums);

                    Debug.Log("BINGO");
                }
                else if (tableController.wildDoubActive && openedChips == 4)
                {
                    for (int j = 0; j < nums.Length; j++)
                    {
                        if (openNumbersArray[nums[j]] == 0)
                        {
                            openNumbersArray[nums[j]] = 1;
                            playerGrid.CloseChip(nums[j], false);
                        }
                    }
                }
            }
        }
    }

    private bool CheckJackpotCondition()
    {
        int conditions = openNumbersArray[0] + openNumbersArray[1] + openNumbersArray[2] + openNumbersArray[3] + openNumbersArray[4] +
            openNumbersArray[6] + openNumbersArray[12] + openNumbersArray[16] +
            openNumbersArray[20] + openNumbersArray[21] + openNumbersArray[22] + openNumbersArray[23] + openNumbersArray[24];

        if (conditions == 13 && openedJackpot == 0)
        {
            openedJackpot = 1;

            playerGrid.CheckJackpotChips();
            tableController.UpdateJackpotRewardBonus();

            Debug.Log("JackPot");

            return true;
        }
        else
            return false;
    }

    private void GetAutobingo(int chipNum)
    {
        List<int[]> bingos = new List<int[]>();
        List<int> index = new List<int>();

        for (int i = 0; i < 10; i++)
        {
            if (openedBingosArray[i] == 0)
            {
                int[] nums = TableCalculations.GetConditionNumbers(i);
                if (nums.Contains(chipNum))
                {
                    bingos.Add(nums);
                    index.Add(i);
                }
            }
        }

        int random = Random.Range(0, bingos.Count);
        int[] numsArray = bingos[random];

        if(numsArray.Length > 0)
        {
            for (int j = 0; j < numsArray.Length; j++)
            {
                if (openNumbersArray[numsArray[j]] == 0)
                {
                    openNumbersArray[numsArray[j]] = 1;
                    playerGrid.CloseChip(numsArray[j], false);
                }
            }
        }
        
        openedBingosArray[index[random]] = 1;
        openedBingoCount++;
        tableController.UpdateBingosCounter(1);
        playerGrid.AnimateBingoChips(bingos[random]);

        Debug.Log("BINGO");
    }

    #endregion

    #region Bingo Screen

    public void ActivateBingoScreen(float delay)
    {
        GameController.Instance.SwitchGameIsActive(false);
        bingoScreenTimer = delay + 0.2f;
        bingoScreenActive = true;

        if (!isJackPot)
            bingoCount += 1;
    }

    private void UpdateBingoScreenAnimation()
    {
        if (bingoScreenActive)
        {
            if (bingoScreenTimer > 0)
                bingoScreenTimer -= Time.fixedDeltaTime;

            else
            {
                StartBingoAnimation(isJackPot);

                if (GameController.tutorialIsActive && openedBingoCount < 2 && GameController.tutorialManager.GetGameProgress() < 5)
                    GameController.tutorialManager.UpdateGameProgress(4);

                bingoScreenActive = false;
            }
        }
    }

    private void StartBingoAnimation(bool isJackPot)
    {
        int[] currency = new int[2] { 0, 0 };

        if (!isJackPot)
            currency = new int[2] { TableCalculations.CalculateBingoCoins(bingoCount, jackpotLevel), TableCalculations.CalculateBingoMoney(bingoCount, jackpotLevel) };
        else
            currency = new int[2] { TableCalculations.GetJackpotBonus(jackpotLevel), 0 };

        tableController.CallBingoAnim(transform, openedBingoCount, currency, isJackPot);
    }

    #endregion

    #region Tutorial

    public IEnumerator CloseTutorialChip(bool isBooster, int chipNum)
    {
        openNumbersArray[chipNum] = 1;

        if (isBooster)
        {
            UIAnimationScreen.Instance.StartCometAnimation(playerGrid.chipArray[chipNum].transform.position);
            yield return new WaitForSeconds(settings.bingoAnumationTimescale);
            playerGrid.CloseChip(chipNum, false);
        }
        else
        {
            EventBus.onChipClosed?.Invoke(chipNum, true);
            playerGrid.CloseChip(chipNum, false);
        }
    }

    public void GenerateTutorialCard()
    {
        cardNumbers.ResetNumbersVisibility();
        cardNumbers.SetTutorialNumbers();
        cardNumbersArray = cardNumbers.GetNumbersArray();

        openNumbersArray = new int[25];
        openNumbersArray[12] = 1;
        cardNumbers.TurnOffTextNum(12);
        openedBingosArray = new int[13];
        playerGrid.ResetChips();
        playerGrid.CloseChip(12, false);
        cardIsOpen = true;
        openedBingoCount = 0;
    }
    #endregion
}
