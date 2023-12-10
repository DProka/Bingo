using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIJoinGame : UIMenuGeneral
{
    [Header("Main")]

    [SerializeField] UIController uiController;
    [SerializeField] TextMeshProUGUI playerLevelText;
    [SerializeField] TextMeshProUGUI gamePriceText;

    private int playerLevel;
    private int cardCount = 2;
    private int cardsMultiplier = 1;
    private int gamePrice = 1000;

    [Header("JackPot Buttons")]

    [SerializeField] Image buttonMinusImage;
    [SerializeField] Image buttonPlusImage;

    [SerializeField] Sprite[] minusImageArray;
    [SerializeField] Sprite[] plusImageArray;

    [Header("JackPot Image")]

    [SerializeField] GameObject[] lockImagesArray;
    [SerializeField] GameObject[] betImagesArray;
    [SerializeField] Image[] chestsImagesArray;

    [SerializeField] Sprite[] chestsSpritesArray;

    private int betCount = 0;
    private int maxBetCount = 1;

    public void ChooseCardsButton(int cardsCount)
    {
        cardCount = cardsCount;
        cardsMultiplier = cardsCount / 2;
        SetGamePrice();
    }

    public void JoinGame()
    {
        uiController.CalculateCoins(false, gamePrice);
        uiController.StartNewGame(); 
        uiController.SetChestSprites(chestsSpritesArray[betCount - 1]);
    }

    public int GetCount() { return cardCount; }
    
    public void SetLevelText() { playerLevelText.text = $"Level {playerLevel}"; }

    public void SetMaxBetCount(int lvl)
    {
        playerLevel = lvl;

        if (lvl < 8)
            maxBetCount = 1;
        if (lvl >= 8)
            maxBetCount = 2;
        if(lvl >= 15)
            maxBetCount = 3;
        if (lvl >= 40)
            maxBetCount = 4;
        if (lvl >= 80)
            maxBetCount = 5;

        SetLockImages();
        SetLevelText();
    }

    private void SetGamePrice()
    {
        gamePrice = 1000 * betCount * cardsMultiplier;
        SetPriceText();
    }

    private void SetPriceText() { gamePriceText.text = $"{gamePrice}"; }

    #region Bet Menu

    public void SetBet(bool plus)
    {
        if (plus && betCount < maxBetCount)
        {
            betCount += 1;
        }
        else if (!plus && betCount > 1)
        {
            betCount -= 1;
        }

        SetGamePrice();
        UpdateMenuImages();
    }

    public int GetJackpotLvl() { return betCount; }

    private void UpdateMenuImages()
    {
        CheckBetButtonsImage();
        SwitchBetImage();
        SwitchChestSprite();
        SetGamePrice();
    }

    private void CheckBetButtonsImage()
    {
        if (betCount == 1)
            buttonMinusImage.sprite = minusImageArray[0];
        else if(betCount > 1)
            buttonMinusImage.sprite = minusImageArray[1];
        
        if (betCount == 5)
            buttonPlusImage.sprite = plusImageArray[0];
        else if(betCount < 5)
            buttonPlusImage.sprite = plusImageArray[1];
    }

    private void SetLockImages()
    {
        foreach (GameObject lockObj in lockImagesArray)
        {
            lockObj.SetActive(false);
        }

        for (int i = maxBetCount - 1; i < lockImagesArray.Length; i++)
        {
            lockImagesArray[i].SetActive(true);
        }
    }

    private void SwitchBetImage()
    {
        foreach(GameObject betObj in betImagesArray)
        {
            betObj.SetActive(false);
        }

        for (int i = 0; i < betCount - 1; i++)
        {
            betImagesArray[i].SetActive(true);
        }
    }

    private void SwitchChestSprite()
    {
        foreach(Image img in chestsImagesArray)
        {
            img.sprite = chestsSpritesArray[betCount - 1];
            img.transform.DOPunchScale(new Vector3(-0.5f, -0.5f, 0), 0.1f, 0);
        }
    }
    #endregion

    #region Main Window

    public override void OpenMain()
    {
        base.OpenMain();

        betCount = 1;
        UpdateMenuImages();
    }

    public override void CloseMain()
    {
        base.CloseMain();
    }
    #endregion
}
