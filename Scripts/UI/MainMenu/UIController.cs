
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    public bool mainUiIsActive { get; private set; }

    [SerializeField] UIMainMenu mainMenu;
    [SerializeField] DesignManager designManager;
    [SerializeField] UIPuzzleMenuScript puzzleMenu;
    [SerializeField] UIGachaMenuScript gachaMenu;
    [SerializeField] TableController tableController;
    [SerializeField] UITable uiTable;
    [SerializeField] UIRoundOverScreen endScreen;
    [SerializeField] UIAnimationScreen animationScreen;
    [SerializeField] UIAdditionalScreens additionalScreens;

    [SerializeField] DebugMenu debugMenu;

    public CoreUISettings settings { get; private set; }
    private PlayerSettings settingsSave;
    private UISave uiSave;

    public void Init(CoreUISettings _settings, TableSettings tableSettings)
    {
        Instance = this;
        settings = _settings;

        uiSave = new UISave();
        uiSave.Load();

        settingsSave = new PlayerSettings();
        settingsSave.Load();

        mainMenu.Init();
        EventBus.onWindowOpened += ClosehMainMenu;
        EventBus.onWindowClosed += OpenMainMenu;
        EventBus.onRoundStarted += StartNewGame;

        designManager.Init(this);
        designManager.SwitchManagerActive(false);

        uiTable.Init(tableSettings);
        animationScreen.Init(settings.animationSettings);
        puzzleMenu.Init();
        gachaMenu.Init(settings.gachaSettings, uiSave);
        endScreen.Init();

        additionalScreens.Init(settingsSave, uiSave);

        debugMenu.Init();

        HideAllMenus();
        CallScreen(Menu.LoadScreen);
    }

    public void CallScreen(Menu newScreen)
    {
        HideAllMenus();

        switch (newScreen)
        {
            case Menu.MainMenu:
                UpdateMainUI();
                mainMenu.OpenMenu();
                mainMenu.HideInterface(false);
                SwitchTable(false, 2);
                mainUiIsActive = true;
                SwitchDesignManagerActive(true);
                GameController.Instance.CheckTutorialSteps();
                SoundController.Instance.SwitchMainMenuMusic(true);
                break;

            case Menu.PuzzleMenu1:
                puzzleMenu.CallWindow1();
                break;

            case Menu.PuzzleMenu2:
                puzzleMenu.CallWindow2();
                break;

            case Menu.GachaMenu:
                gachaMenu.OpenMain();
                break;

            case Menu.EndRoundScreen:
                endScreen.OpenWindow();
                break;

            case Menu.DesignManager:
                break;

            case Menu.JoinGameMenu:
                additionalScreens.CallScreen(Menu.JoinGameMenu);
                break;

            case Menu.BackGroundsMenu:
                additionalScreens.CallScreen(Menu.BackGroundsMenu);
                break;

            case Menu.SettingsMenu:
                additionalScreens.CallScreen(Menu.SettingsMenu);
                break;

            case Menu.NoMoneyScreen:
                additionalScreens.CallScreen(Menu.NoMoneyScreen);
                break;

            case Menu.RateUsScreen:
                additionalScreens.CallScreen(Menu.RateUsScreen);
                break;

            case Menu.LoadScreen:
                UpdateMainUI();
                additionalScreens.CallScreen(Menu.LoadScreen);
                break;

            case Menu.ExitScreen:
                additionalScreens.CallScreen(Menu.ExitScreen);
                break;

            case Menu.BoosterScreen:
                additionalScreens.CallScreen(Menu.BoosterScreen);
                break;
        }
    }

    public enum Menu
    {
        MainMenu,
        JoinGameMenu,
        BackGroundsMenu,
        PuzzleMenu1,
        PuzzleMenu2,
        GachaMenu,
        SettingsMenu,

        DesignManager,

        NoMoneyScreen,
        RateUsScreen,
        EndRoundScreen,
        LoadScreen,
        ExitScreen,

        NewLevelScreen,
        BoosterScreen,
    }

    public void UpdateMainUI()
    {
        mainMenu.UpdateUI(GameController.Instance.playerCoins,
            GameController.Instance.playerMoney,
            GameController.Instance.playerCrystals,
            GameController.Instance.playedRoundsCount,
            GameController.Instance.currentXPLevel,
            GameController.Instance.currentXPPoints,
            GameController.Instance.xpPointsToNextLevel);
    }

    public void UpdateEndRoundScreen(int coins, int money) { endScreen.UpdateCardRewardText(coins, money); }

    public void SetChestSprites(Sprite usedChest) => tableController.SetChestSprite(usedChest);

    public void StartNewLevelAnimation(int[] stats, BoosterManager.Type[] typesArray, int[] counts) => additionalScreens.StartNewLevelAnimation(stats, typesArray, counts);

    public void StartXPProgressAnimation()
    {
        mainMenu.UpdatePlayerXPPoints(GameController.Instance.currentXPLevel,
              GameController.Instance.currentXPPoints,
              GameController.Instance.xpPointsToNextLevel, 1f);
    }

    public void SwitchTable(bool isActive, int cardsCount)
    {
        if (isActive)
            designManager.CloseMain();
        else
            designManager.OpenMain();

        uiTable.SwitchTableCanvas(isActive);
        uiTable.CheckJackpotDisplaySize(cardsCount > 2);
        SwitchTableUI(true);
    }

    public void SwitchTableUI(bool isActive) => uiTable.SwitchTableUiActive(isActive);

    public void SetRoundTableBoosters(bool[] statuses) => uiTable.SetRoundBoosters(statuses);

    public void CallExitScreen()
    {
        if (!mainMenu.CheckCanvasIsActive() && mainUiIsActive)
        {
            CallScreen(Menu.MainMenu);
            return;
        }

        CallScreen(Menu.ExitScreen);
    }

    private void HideAllMenus()
    {
        mainMenu.CloseMenu();
        endScreen.CloseWindow();
        puzzleMenu.CloseWindow();
        gachaMenu.CloseMain();
        additionalScreens.HideAllMenus();
    }

    private void OpenMainMenu()
    {
        //CallScreen(Menu.MainMenu);
        mainMenu.HideInterface(false);
        SwitchDesignManagerActive(true);
    }

    private void ClosehMainMenu()
    {
        mainMenu.HideInterface(true);
        SwitchDesignManagerActive(false);
    }

    #region Design

    public void SwitchDesignManagerVisible(bool isVisible)
    {
        if (isVisible)
            designManager.OpenMain();
        else
            designManager.CloseMain();
    }

    public void LoadNextRoom() => designManager.SwitchCurrentRoom();

    public void CheckRoomIsPassed() => designManager.CheckRoomPassed();

    public RoomPrefabScript GetCurrentRoom() { return designManager.GetCurrentRoom(); }

    public void SwitchDesignManagerActive(bool isActive) => designManager.SwitchManagerActive(isActive);

    #endregion

    #region Game Part

    public void StartNewGame(int cardsCount = 0)
    {
        SoundController.Instance.SwitchMainMenuMusic(false);
        HideAllMenus();
        SwitchTable(true, cardsCount);
        mainUiIsActive = false;
    }

    public void OpenRoundOverScreen() => endScreen.OpenWindow();

    public void CloseRoundOverScreen()
    {
        CallScreen(Menu.MainMenu);
        EventBus.onRoundEnded?.Invoke();
    }

    public void UpdateTableBooster(float progress, BoosterManager.Booster boosterType, bool isActive) => uiTable.UpdateBooster(progress, boosterType, isActive);

    #endregion

    #region Tutorial

    public void SetTutorialMenu()
    {
        SwitchDesignManagerVisible(false);
        mainMenu.SwitchNewGameButton(false);
    }

    public void StartTutorialGame()
    {
        SoundController.Instance.SwitchMainMenuMusic(false);

        HideAllMenus();
        UpdateMainUI();
    }
    #endregion

    public void SetFirebaseSettings()
    {
        designManager.SetFirebaseSettings();
    }

    public void ResetSave()
    {
        designManager.ResetSave();
        puzzleMenu.ResetSave();
        uiSave.ResetSave();
        settingsSave.ResetSave();
    }

    private void OnDestroy()
    {
        EventBus.onWindowOpened -= ClosehMainMenu;
        EventBus.onWindowClosed -= OpenMainMenu;
        EventBus.onRoundStarted -= StartNewGame;
    }
}
