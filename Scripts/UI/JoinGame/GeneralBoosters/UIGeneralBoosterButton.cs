
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIGeneralBoosterButton : MonoBehaviour
{
    public int id { get; private set; }
    public Status currentStatus { get; private set; }

    [SerializeField] Image backImage;
    [SerializeField] Image frontImage;
    [SerializeField] Image statusImage;
    [SerializeField] Image countImage;
    [SerializeField] TextMeshProUGUI countText;
    [SerializeField] UIMessageToUnlock lockMessage;

    private UIJoinGameBoosters menu;
    private JoinGameMenuSettings settings;

    private int count;

    public void Init(UIJoinGameBoosters _menu, JoinGameMenuSettings _settings, int _id, int _count)
    {
        menu = _menu;
        settings = _settings;
        id = _id;
        count = _count;
        frontImage.sprite = settings.boosterFrontImagesArray[id];

        lockMessage.SetText(settings.bonusUnlockLvlArray[id]);
        
        UpdateCount(_count);
        UpdateStatus(Status.Closed);
    }

    public void ActivateBonus()
    {
        switch (currentStatus)
        {
            case Status.Closed:
                ShowLockMessage();
                break;
        
            case Status.Open:
                if(count > 0)
                {
                    menu.ActivateBooster(id, true);
                    UpdateStatus(Status.Active);
                }
                break;
        
            case Status.Active:
                    menu.ActivateBooster(id, false);
                    UpdateStatus(Status.Open);
                break;
        }
    }

    public void UpdateCount(int _count)
    {
        count = _count;
        countText.text = "" + _count;
    }

    public void UpdateStatus(Status newStatus)
    {
        currentStatus = newStatus;
        backImage.sprite = settings.boosterBackImagesArray[0];
        frontImage.DOFade(0.5f, 0f);
        statusImage.sprite = settings.BoosterStatusImagesArray[0];
        countImage.enabled = false;
        countText.enabled = false;

        switch (newStatus)
        {
            case Status.Closed:
                break;

            case Status.Open:
                backImage.sprite = settings.boosterBackImagesArray[1];
                frontImage.DOFade(1f, 0f);
                statusImage.sprite = settings.BoosterStatusImagesArray[1];
                countImage.enabled = true;
                countText.enabled = true;
                break;

            case Status.Active:
                backImage.sprite = settings.boosterBackImagesArray[2];
                frontImage.DOFade(1f, 0f);
                statusImage.sprite = settings.BoosterStatusImagesArray[2];
                break;
        }
    }

    public enum Status
    {
        Closed,
        Open,
        Active
    }

    public void ShowLockMessage() => lockMessage.StartMessageAnimation();
    
    public void HideMessage() => lockMessage.HideMessage();
}
