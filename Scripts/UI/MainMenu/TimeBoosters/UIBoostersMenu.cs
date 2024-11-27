
using UnityEngine;
using DG.Tweening;

public class UIBoostersMenu : MonoBehaviour
{
    [SerializeField] GameObject mainObject;
    [SerializeField] Transform prefabParent;

    [SerializeField] UIScreenButton button;

    private TimeBoosterMenuSettings settings;
    private UITimeBoosterPrefab[] prefabsArray;

    private bool isActive;

    public void Init(TimeBoosterMenuSettings _settings)
    {
        settings = _settings;
        isActive = false;

        button.SwitchStatus(UIScreenButton.Status.Open);

        InitializeMenu();
        CheckButtonStatus();
    }

    private void Update()
    {
        if (isActive)
        {
            for (int i = 0; i < prefabsArray.Length; i++)
            {
                if (prefabsArray[i]._isTimer)
                {
                    System.TimeSpan timer = GameController.Instance.boosterManager.GetTimerByType(prefabsArray[i].boosterType);
                    UpdateTimerText(timer, prefabsArray[i]);
                }
            }
        }
    }

    public void SwitchMenu()
    {
        if (isActive)
        {
            mainObject.transform.DOScale(0, settings.openAnimSpeed).OnComplete(() => SwitchMenuByBool(false));
            isActive = false;
        }
        else
        {
            SwitchMenuByBool(true);
            UpdateCounts();
            mainObject.transform.DOScale(0, 0);
            mainObject.transform.DOScale(1, settings.openAnimSpeed);
            isActive = true;
            button.SwitchStatus(UIScreenButton.Status.Open);
        }
    }

    public void SwitchMenuByBool(bool _isActive)
    {
        isActive = _isActive;
        mainObject.SetActive(isActive);
    }

    private void UpdateTimerText(System.TimeSpan time, UITimeBoosterPrefab boosterPref)
    {
        string timerText;

        if (time.Hours > 0)
            timerText = time.Hours + "h" + time.Minutes + "m";

        else
        {
            if (time.Minutes > 0)
                timerText = time.Minutes + "m";// + time.Seconds + "s";
            else
            {
                if (time.Seconds > 0)
                    timerText = time.Seconds + "s";
                else
                    timerText = "00:00";
            }
        }

        boosterPref.UpdateText(timerText);
    }

    private void InitializeMenu()
    {
        prefabsArray = new UITimeBoosterPrefab[prefabParent.childCount];

        for (int i = 0; i < prefabParent.childCount; i++)
        {
            prefabsArray[i] = prefabParent.GetChild(i).GetComponent<UITimeBoosterPrefab>();
            prefabsArray[i].Init(settings, i);//, 0);
        }
    }

    private void UpdateCounts()
    {
        for (int i = 0; i < prefabsArray.Length; i++)
        {
            if (prefabsArray[i]._isTimer)
            {
                System.TimeSpan timer = GameController.Instance.boosterManager.GetTimerByType(prefabsArray[i].boosterType);
                UpdateTimerText(timer, prefabsArray[i]);
            }
            else
            {
                prefabsArray[i].UpdateCount(GameController.Instance.boosterManager.GetCountByType(prefabsArray[i].boosterType));
            }
        }
    }

    #region Button

    public void ActivateButtonAttention()
    {
        button.SwitchStatus(UIScreenButton.Status.Attention);
        button.SwitchAttentionActive(true);
    }

    private void CheckButtonStatus()
    {
        if (GameController.Instance.currentXPLevel >= 2)
            button.SwitchStatus(UIScreenButton.Status.Open);
        else
            button.SwitchStatus(UIScreenButton.Status.Closed);
    }

    #endregion
}
