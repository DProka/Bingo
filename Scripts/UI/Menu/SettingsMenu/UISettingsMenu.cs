
using UnityEngine;
using DG.Tweening;

public class UISettingsMenu : UIMenuGeneral
{
    [Header("Main Links")]

    [SerializeField] PlayerSettings playerSettings;
    [SerializeField] SoundController soundController;

    private bool isActive;

    [Header("Switches")]

    [SerializeField] UISwitchVoice switchVoice;
    [SerializeField] UISwitchMusic switchMusic;
    [SerializeField] UISwitchSound switchSound;

    [Header("Edit Language")]

    [SerializeField] UISwitchLocalization switchLanguage;

    public void Init()
    {
        LoadSettings();

        isActive = true;
        CloseMain();

        switchLanguage.Init();

        CloseAllWindows();
    }

    public void LoadSettings()
    {
        playerSettings.Load();

        if (playerSettings.voice == 1)
            switchVoice.SwitchOn();
        else
            switchVoice.SwitchOff();
        
        if (playerSettings.music == 1)
            switchMusic.SwitchOn();
        else
            switchMusic.SwitchOff();
        
        if (playerSettings.sound == 1)
            switchSound.SwitchOn();
        else
            switchSound.SwitchOff();

        soundController.SwitchVoice(switchVoice.GetStatus());

        switchLanguage.LoadLocalization(playerSettings.gameLanguage);
    }

    public void CloseAllWindows()
    {
        CloseLanguageWindow();
    }

    #region Main Window

    public override void OpenMain()
    {
        base.OpenMain();

        isActive = true;
    }

    public override void CloseMain()
    {
        if (isActive)
        {
            playerSettings.SetVoice(switchVoice.GetStatus());
            playerSettings.SetMusic(switchMusic.GetStatus());
            playerSettings.SetSound(switchSound.GetStatus());

            base.CloseMain();
            isActive = false;
        }
    }
    #endregion

    #region Switches

    public void SwitchVoice()
    {
        if(switchVoice.GetStatus())
            switchVoice.SwitchOff();
        else
            switchVoice.SwitchOn();

        soundController.SwitchVoice(switchVoice.GetStatus());
    }
    
    public void SwitchMusic()
    {
        if(switchMusic.GetStatus())
            switchMusic.SwitchOff();
        else
            switchMusic.SwitchOn();
    }

    public void SwitchSound()
    {
        if(switchSound.GetStatus())
            switchSound.SwitchOff();
        else
            switchSound.SwitchOn();
    }

    #endregion

    #region Edit Language

    public void OpenLanguageWindow() 
    {
        isActive = false;
        switchLanguage.OpenMain();
        SwitchShade(false);
    }

    public void CloseLanguageWindow() 
    {
        isActive = true;
        switchLanguage.CloseMain();
        SwitchShade(true);
    }

    public void SetLanguage() 
    {
        playerSettings.SetLanguage(switchLanguage.GetNumber());
        CloseLanguageWindow();
    }
    #endregion
}
