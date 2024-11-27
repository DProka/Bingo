
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIJoinGame : UIMenuGeneral
{
    public int cardsTutorial => settingsSave.cardsTutorial;

    [Header("Main")]

    [SerializeField] JoinGameMenuSettings settings;

    [SerializeField] UIController uiController;
    [SerializeField] TextMeshProUGUI playerLevelText;

    [Header("Play Button")]

    [SerializeField] TextMeshProUGUI gamePriceText;
    [SerializeField] Image playButtonMainImage;
    [SerializeField] Image[] playButtonImagesArray;
    [SerializeField] TextMeshProUGUI[] playButtonTextArray;
    [SerializeField] UIMessageToUnlock lockMessage;

    [Header("Cards")]

    [SerializeField] UIJoinMenuCardPart cardsPart;

    [Header("JackPot")]

    [SerializeField] UIJoinGameBetPart betPart;

    [Header("Boosters")]

    [SerializeField] UIJoinGameBoosters boosters;

    [Header("Bet Tutorial")]

    [SerializeField] UIJoinGameBetTutorial betTutorial;

    private PlayerSettings settingsSave;

    private int cardsMultiplier = 1;
    private int gamePrice = 1000;

    private int cardCount = 2;
    private bool cards4IsOpen = false;

    private bool playButtonIsAvailable;

    private int tutorWaShown = 0;

    public void Init(PlayerSettings save)
    {
        settingsSave = save;

        betPart.Init(settings);
        boosters.Init(this, settings);
        cardsPart.Init(settings);
        betTutorial.Init(this);

        lockMessage.HideMessage();

        tutorWaShown = 0;
    }

    public void JoinGame()
    {
        if (playButtonIsAvailable)
        {
            GameController.Instance.CalculateCoins(-gamePrice);
            GameController.Instance.SetJackpotLevel(betPart.betCount);
            GameController.Instance.StartGameLogic(cardCount);
            uiController.SetChestSprites(settings.chestsSpritesArray[betPart.betCount - 1]);
        }
    }

    public int GetCount() { return cardCount; }

    public void SetMaxBetCount()
    {
        betPart.SetMaxBetCount();
        playerLevelText.text = $"Round {GameController.Instance.playedRoundsCount}";
    }

    public void UpdateSave() => settingsSave.SaveLastBid(betPart.betCount, cardsTutorial);

    private void SetGamePrice()
    {
        int betPrice = 1000;

        if (GameController.Instance.currentXPLevel >= 6)
        {
            if (GameController.Instance.currentXPLevel <= settings.betPricesArray[0])
                betPrice = 2000;

            else
                betPrice = settings.betPricesArray[betPart.betCount - 1];
        }

        gamePrice = betPrice * betPart.betCount * cardsMultiplier;
        gamePriceText.text = $"{gamePrice}";
    }

    private void UpdateMenuImages()
    {
        betPart.CheckBetButtonsImage();
        betPart.SwitchBetImage();
        SwitchChestSprite();
        SetGamePrice();
        SwitchChoosedCardsImage();
        Update4CardsLock();
        CheckPlayButtonAvailable();
    }

    private void CheckPlayButtonAvailable()
    {
        gamePriceText.enabled = false;

        foreach (TextMeshProUGUI text in playButtonTextArray)
            text.enabled = false;
        
        foreach (Image img in playButtonImagesArray)
            img.enabled = false;

        if (betPart.betCount > betPart.maxBetCount)
        {
            playButtonIsAvailable = false;
            playButtonMainImage.sprite = settings.playButtonSprites[0];

            foreach(TextMeshProUGUI text in playButtonTextArray)
            {
                text.text = $"Unlock at XP Level {settings.xpLevelsArray[betPart.betCount - 2]}";
                text.enabled = true;
            }

            lockMessage.SwitchMessageActive(true);
        }
        else
        {
            playButtonIsAvailable = true;
            playButtonMainImage.sprite = settings.playButtonSprites[1];
            gamePriceText.enabled = true;

            foreach (Image img in playButtonImagesArray)
                img.enabled = true;

            lockMessage.SwitchMessageActive(false);
        }

        Debug.Log("MaxBetCount = " + betPart.maxBetCount);
    }

    #region Cards Count

    public void ChooseCardsButton(int cardsCount)
    {
        if (cards4IsOpen)
            cardCount = cardsCount;

        else
        {
            cardCount = 2;

            if (cardsCount == 4)
                cardsPart.StartLockMessageAnimation();
        }

        cardsMultiplier = cardCount / 2;
        SetGamePrice();
        cardsPart.SwitchChoosedCardsCount(cardCount);

        SoundController.Instance.PlaySound(SoundController.Sound.ButtonClick1);
    }

    private void SwitchChoosedCardsImage() => cardsPart.SwitchChoosedCardsCount(cardCount);

    private void Update4CardsLock() => cardsPart.Set4CardsLock(cards4IsOpen);

    private void SwitchChestSprite()
    {
        if (betPart.betCount < 1)
            cardsPart.SwitchChestSprite(0);
        else
            cardsPart.SwitchChestSprite(betPart.betCount - 1);
    }

    #endregion

    #region Bet Menu

    public void SetBet(bool plus)
    {
        int lastBet = betPart.betCount;

        betPart.SetBet(plus);

        if (lastBet != betPart.betCount)
        {
            UpdateMenuImages();
            UpdateSave();
        }
    }

    public int GetJackpotLvl() { return betPart.betCount; }

    public void SetTutorBet(int num)
    {
        betPart.SetTutorBet(num);
        UpdateMenuImages();
        UpdateSave();
    }

    #endregion

    #region Boosters

    public void SwitchBoosterByNum(int num, bool isActive)
    {
        switch (num)
        {
            case 0:
                GameController.Instance.boosterManager.SwitchMenuBooster(BoosterManager.Type.Plus5Balls, isActive);
                break;

            case 1:
                GameController.Instance.boosterManager.SwitchMenuBooster(BoosterManager.Type.DoubleProgress, isActive);
                break;

            case 2:
                GameController.Instance.boosterManager.SwitchMenuBooster(BoosterManager.Type.AutoBingo, isActive);
                break;
        }

        Debug.Log("Booster Num " + num + " is active " + isActive + ". Autobingo is " + GameController.Instance.boosterManager.autobingoIsActive);
    }

    public enum Booster
    {
        BallsX5,
        Enhancer,
        Autobingo
    }

    #endregion

    #region Tutorial

    public void UpdateTutor()
    {
        tutorWaShown = 1;
    }

    private void CheckBetTutorial()
    {
        if(tutorWaShown == 0)
            betTutorial.StartTutor();
    }

    #endregion

    #region Main Window

    public override void OpenMain()
    {
        base.OpenMain();

        SetMaxBetCount();
        boosters.CheckBoosters();
        boosters.HideAllMessages();
        cards4IsOpen = false;

        if (GameController.Instance.currentXPLevel >= 3)
            cards4IsOpen = true;

        betPart.SetBetCount(settingsSave.lastBid == 0 ? 1 : settingsSave.lastBid);
        UpdateMenuImages();
        EventBus.onWindowOpened?.Invoke();

        CheckBetTutorial();
    }

    public override void CloseMain()
    {
        base.CloseMain();

        betTutorial.CloseTutorial();
        EventBus.onWindowClosed?.Invoke();
    }
    #endregion
}
