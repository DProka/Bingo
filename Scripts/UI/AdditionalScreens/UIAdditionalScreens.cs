
using UnityEngine;

public class UIAdditionalScreens : MonoBehaviour
{
    [SerializeField] UIJoinGame joinGameMenu;
    [SerializeField] UIBoostersMenu timeBoosterMenu;
    [SerializeField] NoMoneyScreen noMoneyScreen;
    [SerializeField] UINewLevelScreen newLevelScreen;
    [SerializeField] RateUsScreen rateUsScreen;
    [SerializeField] UISettingsMenu settingsMenu;
    [SerializeField] UIBackGroundsScreen backGroundsScreen;
    [SerializeField] FirstLoadingScreen loadScreen;
    [SerializeField] ExitScreen exitScreen;

    private CoreUISettings settings;
    private PlayerSettings settingsSave;
    private UISave uiSave;

    public void Init(PlayerSettings _settingsSave, UISave _uiSave)
    {
        settings = UIController.Instance.settings;
        settingsSave = _settingsSave;
        uiSave = _uiSave;

        joinGameMenu.Init(settingsSave);
        timeBoosterMenu.Init(settings.boosterMenuSettings);
        settingsMenu.Init(settingsSave);
        backGroundsScreen.Init(uiSave, settings.backGroundSettings);
        rateUsScreen.Init();
        exitScreen.Init();
        loadScreen.Init(settings.loadindAnimationTime);
        newLevelScreen.Init();
    }

    public void CallScreen(UIController.Menu newScreen)
    {
        HideAllMenus();

        switch (newScreen)
        {
            case UIController.Menu.JoinGameMenu:
                joinGameMenu.OpenMain();
                break;

            case UIController.Menu.BackGroundsMenu:
                backGroundsScreen.OpenMenu();
                break;

            case UIController.Menu.SettingsMenu:
                settingsMenu.OpenMain();
                break;

            case UIController.Menu.NoMoneyScreen:
                noMoneyScreen.OpenMain();
                break;

            case UIController.Menu.RateUsScreen:
                rateUsScreen.CallRateUs();
                break;

            case UIController.Menu.LoadScreen:
                loadScreen.OpenWindow();
                break;

            case UIController.Menu.ExitScreen:
                exitScreen.OpenScreen(UIController.Instance.mainUiIsActive);
                break;
        
            case UIController.Menu.BoosterScreen:
                timeBoosterMenu.SwitchMenu();
                break;
        }
    }

    public void StartNewLevelAnimation(int[] stats, BoosterManager.Type[] typesArray, int[] counts) 
    {
        newLevelScreen.SetLevelReward(typesArray, counts);
        newLevelScreen.StartNewLevelAnimation(stats);
        timeBoosterMenu.ActivateButtonAttention();
    }

    public void HideAllMenus()
    {
        joinGameMenu.CloseMain();
        newLevelScreen.CloseMain();
        settingsMenu.CloseMain();
        backGroundsScreen.CloseMenu();
        loadScreen.CloseWindow();
        noMoneyScreen.CloseMain();
        exitScreen.CloseMain();
        timeBoosterMenu.SwitchMenuByBool(false);
    }
}
