
using UnityEngine;

public class UISettingsMenu : UIMenuGeneral
{
    [Header("Switches")]

    [SerializeField] UISwitchButton switchMusic;
    [SerializeField] UISwitchButton switchEffects;

    [SerializeField] Sprite[] switchSpritesArray;

    [Header("Edit Language")]

    [SerializeField] UISwitchLocalization switchLanguage;

    private PlayerSettings settingsSave;

    public bool musicOn;
    public bool effectsOn;

    public void Init(PlayerSettings save)
    {
        musicOn = true;
        effectsOn = true;

        settingsSave = save;
        LoadSettings();
    }

    private void LoadSettings()
    {
        int[] loadedSettings = settingsSave.GetSettings();

        musicOn = loadedSettings[0] == 1 ? true : false;
        SoundController.Instance.SwitchMusic(musicOn);
        switchMusic.SwitchImage(musicOn ? switchSpritesArray[0] : switchSpritesArray[1]);

        effectsOn = loadedSettings[1] == 1 ? true : false;
        SoundController.Instance.SwitchEffects(effectsOn);
        switchEffects.SwitchImage(effectsOn ? switchSpritesArray[0] : switchSpritesArray[1]);
    }

    #region Switches

    public void SwitchMusic()
    {
        if (!musicOn)
        {
            SoundController.Instance.SwitchMusic(true);
            switchMusic.SwitchImage(switchSpritesArray[0]);
            musicOn = true;
        }
        else
        {
            SoundController.Instance.SwitchMusic(false);
            switchMusic.SwitchImage(switchSpritesArray[1]);
            musicOn = false;
        }
    }

    public void SwitchEffects()
    {
        if (!effectsOn)
        {
            SoundController.Instance.SwitchEffects(true);
            switchEffects.SwitchImage(switchSpritesArray[0]);
            effectsOn = true;
        }
        else
        {
            SoundController.Instance.SwitchEffects(false);
            switchEffects.SwitchImage(switchSpritesArray[1]);
            effectsOn = false;
        }
    }

    #endregion

    public void SetNewBackground(int num) => settingsSave.SaveBackGround(num);

    public int GetBackGroundNum() { return settingsSave.currentBackgroundNum; }

    #region Edit Language

    public void OpenLanguageWindow() 
    {
        switchLanguage.OpenMain();
        SwitchShade(false);
    }

    public void CloseLanguageWindow() 
    {
        switchLanguage.CloseMain();
        SwitchShade(true);
    }

    public void SetLanguage() 
    {
        //playerSettings.SetLanguage(switchLanguage.GetNumber());
        CloseLanguageWindow();
    }
    #endregion

    #region Main Window

    public override void OpenMain()
    {
        base.OpenMain();

        EventBus.onWindowOpened?.Invoke();
    }

    public override void CloseMain()
    {
        base.CloseMain();
        settingsSave.SaveSettings(musicOn, effectsOn);
        EventBus.onWindowClosed?.Invoke();
    }
    #endregion
}
