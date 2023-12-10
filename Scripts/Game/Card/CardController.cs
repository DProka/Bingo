
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class CardController : MonoBehaviour
{
    [Header("Main")]

    [SerializeField] CardNumbers cardNumbers;
    [SerializeField] CardPlayerGrid playerGrid;

    private TableController tableController;
    private BoosterController boosterController;
    private bool cardIsOpen;

    [Header("Bingo Screen")]

    [SerializeField] GameObject bingoScreen;
    [SerializeField] Image bingoImage;
    [SerializeField] float bingoAnimationTime;
    [SerializeField] float bingoScreenTime = 1.5f;

    [SerializeField] Sprite[] bingoSpriteArray;

    private int openedBingoCount;
    private int[] openedBingosArray;
    private int openedJackpot;

    private int[] cardNumbersArray;
    private int[] openNumbersArray;
    private int[] chestArray;

    public void Init(TableController table, BoosterController booster)
    {
        tableController = table;
        boosterController = booster;

        cardNumbersArray = new int[25];
        openNumbersArray = new int[25];
        chestArray = new int[25];

        openedBingosArray = new int[13];
        openedJackpot = 0;

        cardNumbers.Init();
        playerGrid.Init(this, booster);
    }

    public void GenerateCard()
    {
        cardNumbers.ResetNumbersVisibility();
        cardNumbers.FillEmptyNumbers(cardNumbersArray);

        openNumbersArray = new int[25];
        openedBingosArray = new int[13];
        playerGrid.ResetChips();

        if (tableController.GetPlayerLvl() < 4)
        {
            openNumbersArray[12] = 1;
            cardNumbers.TurnOffTextNum(12);
            playerGrid.CloseChip(12);
        }
        
        SetRandomStartChests();
        cardIsOpen = true;
        openedBingoCount = 0;
        bingoScreen.SetActive(false);
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
                    if (tableController.GetPlayerLvl() >= 4)
                        playerGrid.SetChipClick(i);
                    else
                        StartCoroutine(playerGrid.OpenChip(i));
                    
                    GameController.tutorialManager.ActivateStepWithHand(i, playerGrid.GetChipPosition(i));
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

    #region Chip Code

    public IEnumerator OpenChip(int num)
    {
        cardNumbers.TurnOffTextNum(num);

        yield return new WaitForSeconds(0.2f);

        CheckBingoCondition();
        CheckJackpotCondition();
    }

    public void CloseRandomChip()
    {
        if (cardIsOpen)
        {
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
                    openNumbersArray[i] = 1;
                    playerGrid.CloseChip(i);
                    boosterController.StartCometAnimation(playerGrid.GetChipPosition(i));
                    break;
                }
            }
        }
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
            cardNumbers.SetTextBorder(openedNumbers[num]);
            openedNumbers.Remove(openedNumbers[num]);
        }
    }

    public void GetChestBonus(Vector3 chestPos) { tableController.GetChestBonus(chestPos); }

    public void AddRandomBonusChest()
    {
        if (cardIsOpen)
        {
            List<int> openedNumbers = new List<int>();

            for (int i = 0; i < openNumbersArray.Length; i++)
            {
                if (openNumbersArray[i] == 0 && chestArray[i] == 0)
                {
                    //openedNumbers.Add(cardNumbersArray[i]);
                    openedNumbers.Add(i);
                }
            }

            int num = Random.Range(0, openedNumbers.Count);

            chestArray[openedNumbers[num]] = 1;
            playerGrid.SetChestChip(openedNumbers[num]);
            boosterController.StartCometAnimation(playerGrid.GetChipPosition(openedNumbers[num]));
            cardNumbers.SetTextBorder(openedNumbers[num]);
            openedNumbers.Remove(openedNumbers[num]);
        }
    }

    public void SetChestSprite(Sprite usedChest) { playerGrid.SetChestSprite(usedChest); }

    #endregion

    #region Victory Conditions

    private void CheckBingoCondition()
    {
        int[] conditions = new int[13];

        conditions[0] = openNumbersArray[0] + openNumbersArray[1] + openNumbersArray[2] + openNumbersArray[3] + openNumbersArray[4];
        conditions[1] = openNumbersArray[5] + openNumbersArray[6] + openNumbersArray[7] + openNumbersArray[8] + openNumbersArray[9];
        conditions[2] = openNumbersArray[10] + openNumbersArray[11] + openNumbersArray[12] + openNumbersArray[13] + openNumbersArray[14];
        conditions[3] = openNumbersArray[15] + openNumbersArray[16] + openNumbersArray[17] + openNumbersArray[18] + openNumbersArray[19];
        conditions[4] = openNumbersArray[20] + openNumbersArray[21] + openNumbersArray[22] + openNumbersArray[23] + openNumbersArray[24];
        conditions[5] = openNumbersArray[0] + openNumbersArray[5] + openNumbersArray[10] + openNumbersArray[15] + openNumbersArray[20];
        conditions[6] = openNumbersArray[1] + openNumbersArray[6] + openNumbersArray[11] + openNumbersArray[16] + openNumbersArray[21];
        conditions[7] = openNumbersArray[2] + openNumbersArray[7] + openNumbersArray[12] + openNumbersArray[17] + openNumbersArray[22];
        conditions[8] = openNumbersArray[3] + openNumbersArray[8] + openNumbersArray[13] + openNumbersArray[18] + openNumbersArray[23];
        conditions[9] = openNumbersArray[4] + openNumbersArray[9] + openNumbersArray[14] + openNumbersArray[19] + openNumbersArray[24];
        conditions[10] = openNumbersArray[0] + openNumbersArray[6] + openNumbersArray[12] + openNumbersArray[18] + openNumbersArray[24];
        conditions[11] = openNumbersArray[4] + openNumbersArray[8] + openNumbersArray[12] + openNumbersArray[16] + openNumbersArray[20];
        conditions[12] = openNumbersArray[0] + openNumbersArray[4] + openNumbersArray[12] + openNumbersArray[20] + openNumbersArray[24];

        for (int i = 0; i < conditions.Length; i++)
        {
            if (conditions[i] == 5 && openedBingosArray[i] == 0)
            {
                openedBingosArray[i] = 1;
                playerGrid.CheckBingoChips(i);
                openedBingoCount++;
                tableController.UpdateBingosCounter(1);
                if(openedBingoCount <= 2)
                    bingoImage.sprite = bingoSpriteArray[openedBingoCount - 1];
                else
                    bingoImage.sprite = bingoSpriteArray[bingoSpriteArray.Length - 1];
                StartCoroutine(ShowBingoScreen());
                Debug.Log("BINGO");
            }
        }
    }

    public void CheckJackpotCondition()
    {
        int[] jackpotChips = new int[] { 0, 1, 2, 3, 4, 6, 12, 16, 20, 21, 22, 23, 24 };
        int conditions = openNumbersArray[0] + openNumbersArray[1] + openNumbersArray[2] + openNumbersArray[3] + openNumbersArray[4] +
            openNumbersArray[6] + openNumbersArray[12] + openNumbersArray[16] +
            openNumbersArray[20] + openNumbersArray[21] + openNumbersArray[22] + openNumbersArray[23] + openNumbersArray[24];

        if (conditions == 13 && openedJackpot == 0)
        {
            openedJackpot = 1;

            playerGrid.CheckJackpotChips();
            tableController.UpdateJackpotRewardBonus();

            StartCoroutine(ShowBingoScreen());
            Debug.Log("JackPot");
        }
    }

    private IEnumerator ShowBingoScreen()
    {
        tableController.SwitchGameIsActive(false);
        bingoScreen.SetActive(true);
        bingoScreen.transform.localScale = new Vector3(0, 0, 0);
        bingoScreen.transform.DOScale(1, 0.4f).SetDelay(1f);
        bingoScreen.transform.DOScale(0, 0.4f).SetDelay(bingoScreenTime * 2 + 0.2f);

        if (GameController.tutorialIsActive && openedBingoCount < 2)
            tableController.SwitchKeepGoingScreen();

        yield return new WaitForSeconds(bingoScreenTime * 2 + 0.4f);
         
        tableController.SwitchGameIsActive(true);
        bingoScreen.SetActive(false);
    }
    #endregion

    #region Tutorial

    public void CloseTutorialChip(bool isBooster, int chipNum)
    {
        openNumbersArray[chipNum] = 1;
        playerGrid.CloseChip(chipNum);
        
        if(isBooster)
            boosterController.StartCometAnimation(playerGrid.GetChipPosition(chipNum));
        else
            boosterController.UpdateProgress();
    }

    public void GenerateTutorialCard()
    {
        cardNumbers.SetTutorialNumbers();
        cardNumbersArray = cardNumbers.GetNumbersArray();

        openNumbersArray = new int[25];
        openNumbersArray[12] = 1;
        cardNumbers.TurnOffTextNum(12);
        openedBingosArray = new int[13];
        playerGrid.ResetChips();
        playerGrid.CloseChip(12);
        cardIsOpen = true;
        openedBingoCount = 0;
        bingoScreen.SetActive(false);
    }
    #endregion
}
