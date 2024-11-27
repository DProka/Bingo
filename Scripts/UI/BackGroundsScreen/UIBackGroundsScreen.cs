
using UnityEngine;
using UnityEngine.UI;

public class UIBackGroundsScreen : UIMenuGeneral
{
    [SerializeField] UIScreenButton button;
    [SerializeField] Image backGroundImage;
    [SerializeField] Transform prefabParent;

    private UISave save;
    private BackgroundSettings settings;
    private UIBackGroundPrefab[] prefabsArray;
    private int currentBackgroundNum;
    public int lastOpenedBackground;
    public bool newBackIsAvailable;

    public void Init(UISave _save, BackgroundSettings _settings)
    {
        save = _save;
        settings = _settings;
        lastOpenedBackground = 0;
        Load();

        PreparePrefabs();
    }

    public void ActivateBackGround(int num)
    {
        currentBackgroundNum = num;
        backGroundImage.sprite = settings.backgroundsArray[currentBackgroundNum];
        CheckPrefabFrames();
        SoundController.Instance.PlaySound(SoundController.Sound.ButtonClick2);

        Save();
    }

    private void CheckPrefabFrames()
    {
        for (int i = 0; i < prefabsArray.Length; i++)
        {
            prefabsArray[i].SwitchState(i * 20 <= GameController.Instance.playedRoundsCount ? UIBackGroundPrefab.State.Open : UIBackGroundPrefab.State.Locked);
        }

        prefabsArray[currentBackgroundNum].SwitchState(UIBackGroundPrefab.State.Active);
    }

    private void CheckButtonAttention()
    {
        if (GameController.Instance.playedRoundsCount >= 20)
        {
            button.SwitchStatus(newBackIsAvailable ? UIScreenButton.Status.Attention : UIScreenButton.Status.Open);
            button.SwitchAttentionActive(newBackIsAvailable);
        }
        else
            button.SwitchStatus(UIScreenButton.Status.Closed);
    }

    private void CheckNewBackIsAvailable()
    {
        int backNum = GameController.Instance.playedRoundsCount / 20;

        if (lastOpenedBackground != backNum)
        {
            lastOpenedBackground = backNum;
            newBackIsAvailable = true;

            //Debug.LogError("LastBackNum= " + lastOpenedBackground + " NewBackNum= " + backNum);

            Save();
        }
    }

    private void ShowNewBack()
    {
        if (newBackIsAvailable)
        {
            newBackIsAvailable = false;
            Save();
        }
    }

    private void PreparePrefabs()
    {
        prefabsArray = new UIBackGroundPrefab[prefabParent.childCount];

        for (int i = 0; i < prefabsArray.Length; i++)
        {
            prefabsArray[i] = prefabParent.GetChild(i).GetComponent<UIBackGroundPrefab>();
            prefabsArray[i].Init(this, i, settings.shortSpritesArray[i]);
        }

        prefabsArray[prefabParent.childCount - 1].SwitchNextArrow(false);
        backGroundImage.sprite = settings.backgroundsArray[currentBackgroundNum];
        //CheckAllPrefabFrames();
    }

    #region Screen

    public void OpenMenu()
    {
        CheckPrefabFrames();
        OpenMain();
        button.SwitchAttentionActive(false);
        GameController.tutorialManager.HideSteps();
        SoundController.Instance.PlaySound(SoundController.Sound.ButtonClick1);
        ShowNewBack();

        EventBus.onWindowOpened?.Invoke();
    }

    public void CloseMenu()
    {
        CloseMain();
        CheckNewBackIsAvailable();
        CheckButtonAttention();

        EventBus.onWindowClosed?.Invoke();
    }
    #endregion

    #region Save\Load

    private void Save() { save.SaveBackGround(currentBackgroundNum, lastOpenedBackground, newBackIsAvailable); }

    private void Load()
    {
        currentBackgroundNum = save.backGroundNum;
        lastOpenedBackground = save.lastOpenedBackground;
        newBackIsAvailable = save.backGroundWasShown;
    }

    #endregion
}
