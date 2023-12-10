using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [Header("Main Elements")]

    [SerializeField] GameController gameController;
    [SerializeField] PlayerProfile playerData;

    [Header("Main Menu")]

    [SerializeField] UIMainMenu mainMenu;
    [SerializeField] GameObject newGameButton;
    [SerializeField] TextMeshProUGUI newGamelvlText;

    [Header("Room Menu")]

    [SerializeField] UIJoinGame joinGameMenu;
    [SerializeField] TextMeshProUGUI playerlvlText;

    [Header("Room Design")]

    [SerializeField] DesignManager designManager;
    [SerializeField] NoMoneyScreen noMoneyScreen;

    [Header("In Game")]

    [SerializeField] UITable tableMenu;
    [SerializeField] UIEndScreen endScreen;

    [Header("Load Screen")]

    [SerializeField] FirstLoadingScreen loadScreen;

    //[Header("Profile Statistics")]

    //[SerializeField] UIPlayerProfile playerProfile;

    //[Header("Settings Menu")]

    //[SerializeField] UISettingsMenu settings;

    //[Header("New Level Screen")]

    //[SerializeField] NewLevelController newLevelScreen;

    [Header("Debug Menu")]

    [SerializeField] DebugMenu debugMenu;

    public void Init()
    {
        mainMenu.Init();

        designManager.Init(this);
        designManager.SwitchManagerActive(true);

        //settings.Init();

        endScreen.Init();

        debugMenu.Init();

        HideAllMenus();
        OpenLoadingScreen();
    }

    private void HideAllMenus()
    {
        mainMenu.CloseMenu();
        designManager.CloseMain();
        joinGameMenu.CloseMain();
        tableMenu.CloseMenu();
        endScreen.CloseWindow();
        loadScreen.CloseWindow();
        //playerProfile.CloseMain();
        noMoneyScreen.CloseMain();
    }

    #region Main Menu

    public void OpenMainMenu()
    {
        HideAllMenus();
        mainMenu.OpenMenu();
        UpdateMainUI();
        newGameButton.SetActive(true);
        designManager.OpenMain();
        SwitchDesignManagerActive(true);
    }

    public void UpdateMainUI() { mainMenu.UpdateUI(); }

    public void HideMainMenuInterface(bool hide) { mainMenu.HideInterface(hide); }
    #endregion

    #region Design

    public void CalculateCoins(bool isPlus, int count)
    {
        gameController.CalculateCoins(isPlus, count);
    }

    public void OpenNoMoney() 
    {
        SwitchDesignManagerActive(false);   
        noMoneyScreen.OpenMain(); 
    }

    public void CloseNoMoney()
    {
        SwitchDesignManagerActive(true);
        noMoneyScreen.CloseMain();
    }

    public void SwitchDesignManagerVisible(bool isVisible) { designManager.gameObject.SetActive(isVisible); }

    private void SwitchDesignManagerActive(bool isActive) { designManager.SwitchManagerActive(isActive); }

    #endregion

    #region Join Game Menu

    public void OpenNewGameMenu()
    {
        joinGameMenu.OpenMain();
        mainMenu.HideInterface(true);
        //newGameButton.SetActive(false);
        SwitchDesignManagerActive(false);
    }

    public void CloseNewGameMenu()
    {
        joinGameMenu.CloseMain();
        mainMenu.HideInterface(false);
        //newGameButton.SetActive(true);
        SwitchDesignManagerActive(true);
    }

    public int GetPlayerLevel() { return gameController.GetPlayerLevel(); }

    public void SetJackpotLevel(int lvl) { gameController.SetJackpotLevel(lvl); }

    public void SetChestSprites(Sprite usedChest) { gameController.SetChestSprites(usedChest); }

    #endregion

    #region Game Part

    public void StartNewGame()
    {
        HideAllMenus();
        tableMenu.OpenTable(joinGameMenu.GetCount());
        tableMenu.OpenMenu();
        mainMenu.HideInterface(false);
        UpdateMainUI();
        gameController.SetJackpotLevel(joinGameMenu.GetJackpotLvl());
        StartCoroutine(gameController.StartGameLogic(joinGameMenu.GetCount()));
    }

    public void OpenRoundOverScreen() { endScreen.OpenWindow(); }

    public void CloseRoundOverScreen()
    {
        OpenMainMenu();
        endScreen.StartRewardAnimation();
        gameController.CloseEndRoundScreen();
    }

    #endregion

    #region Tutorial

    public void SetTutorialMenu()
    {
        SwitchDesignManagerVisible(false);
        newGameButton.SetActive(false);
    }

    public void StartTutorialGame()
    {
        HideAllMenus();
        tableMenu.OpenTable(1);
        tableMenu.OpenMenu();
        UpdateMainUI();
    }

    public void BuyTutorialObject(RoomObjectScript obj, int price)
    {
        designManager.BuyObject(obj, price);
    }

    #endregion

    #region Loading Screen

    public void OpenLoadingScreen() { loadScreen.OpenWindow(); }
    public void CloseLoadingScreen() { loadScreen.CloseWindow(); }
    public void UpdateLoadinrScreenBar() { loadScreen.UpdateText(); }
    #endregion

    #region Chat

    public void OpenChatWindow() { }
    #endregion

}
