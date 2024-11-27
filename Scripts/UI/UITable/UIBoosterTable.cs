
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIBoosterTable : MonoBehaviour
{
    [Header("main")]

    [SerializeField] Transform boosterObj;
    [SerializeField] Image boosterImage;
    [SerializeField] Image progressImage;
    [SerializeField] Image boosterAutoImage;
    [SerializeField] float activeBoosterAnimTime = 0.7f;

    [Header("Round boosters")]

    [SerializeField] GameObject roundBoostersObj;
    [SerializeField] GameObject imagesParent;
    [SerializeField] TextMeshProUGUI rounBonusesText;
    [SerializeField] Image[] roundImagesArray;

    private TableSettings settings;
    private float activeAnimTime = 0.3f;
    private bool isActive;
    private bool autoBoosterActive;
    private bool animIsPlaying;
    private float animTimer;

    private RectTransform imagerParentTransform;

    public void Init(TableSettings _settings)
    {
        settings = _settings;
        autoBoosterActive = false;

        imagerParentTransform = imagesParent.GetComponent<RectTransform>();

        ResetProgress();
    }

    private void Update()
    {
        if (isActive && !animIsPlaying)
        {
            if (animTimer > 0)
                animTimer -= Time.deltaTime;
            else
            {
                animTimer = activeBoosterAnimTime;
                StartBoosterAnim();
            }
        }
    }

    public void SetRoundBoosters(bool[] statuses)
    {
        int activeCount = 0;

        foreach (bool status in statuses)
        {
            if (status)
                activeCount += 1;
        }

        foreach (Image img in roundImagesArray)
            img.enabled = false;

        roundBoostersObj.SetActive(activeCount > 0);

        if (activeCount > 1)
        {
            imagerParentTransform.sizeDelta = new Vector2(140, imagerParentTransform.sizeDelta.y);
            rounBonusesText.text = "two boosts";
            roundImagesArray[0].enabled = true;

            if (statuses[1] && !statuses[2])
                roundImagesArray[1].enabled = true;

            if (statuses[2])
                roundImagesArray[2].enabled = true;
        }
        else
        {
            imagerParentTransform.sizeDelta = new Vector2(70, imagerParentTransform.sizeDelta.y);
            if (statuses[0])
            {
                roundImagesArray[0].enabled = true;
                rounBonusesText.text = "Wild doub";
            }

            if (statuses[1] && !statuses[2])
            {
                roundImagesArray[1].enabled = true;
                rounBonusesText.text = "Hint doub";
            }

            if (statuses[2])
            {
                roundImagesArray[2].enabled = true;
                rounBonusesText.text = "Auto doub";
            }
        }
    }

    public void UpdateProgress(float progress, BoosterManager.Booster boosterType)
    {
        CheckAutoSwitch();

        progressImage.DOFillAmount(progress, settings.boosterSettings.progressAnimDuration).OnComplete(() =>
        {
            if (progress > 0.9f)
            {
                progressImage.enabled = false;
                SetBoosterActive(true, boosterType);
            }
            else
            {
                
            }
        });

        animIsPlaying = true;
        boosterImage.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0f), settings.boosterSettings.progressAnimDuration, 0).OnComplete(() =>
        {
            animIsPlaying = false;
        });
    }

    public void SetBoosterActive(bool _isActive, BoosterManager.Booster boosterType)
    {
        switch (boosterType)
        {
            case BoosterManager.Booster.GetX1:
                boosterImage.sprite = settings.boosterSettings.boostersSpriteArray[0];
                break;
        
            case BoosterManager.Booster.GetX2:
                boosterImage.sprite = settings.boosterSettings.boostersSpriteArray[1];
                break;
        
            case BoosterManager.Booster.Coins:
                boosterImage.sprite = settings.boosterSettings.boostersSpriteArray[2];
                break;
        
            case BoosterManager.Booster.Chest:
                boosterImage.sprite = settings.boosterSettings.boostersSpriteArray[3];
                break;
        
            case BoosterManager.Booster.Puzzle:
                boosterImage.sprite = settings.boosterSettings.boostersSpriteArray[4];
                break;
        
            case BoosterManager.Booster.DoubleXP:
                boosterImage.sprite = settings.boosterSettings.boostersSpriteArray[5];
                break;
        
            case BoosterManager.Booster.DoubleCoins:
                boosterImage.sprite = settings.boosterSettings.boostersSpriteArray[6];
                break;
        }

        isActive = _isActive;

        if (isActive)
            StartBoosterAnim();
        else
            ResetProgress();
    }

    public void StartBoosterAnim()
    {
        animIsPlaying = true;
        boosterImage.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0), activeAnimTime, 0).OnComplete(() =>
            {
                boosterImage.transform.DOScale(1, 0);
                animIsPlaying = false;
            });
    }

    private void ResetProgress()
    {
        boosterImage.transform.DOScale(new Vector3(1, 1, 1), 0);
        progressImage.fillAmount = 0;
        progressImage.enabled = true;
        boosterImage.sprite = settings.boosterSettings.progressSpriteArray[0];
    }

    private void CheckAutoSwitch()
    {
        if (autoBoosterActive != GameController.Instance.boosterManager.autoBonusActive)
        {
            autoBoosterActive = GameController.Instance.boosterManager.autoBonusActive;
            boosterAutoImage.sprite = settings.boosterSettings.autoBoosterSpritesArray[autoBoosterActive ? 1 : 0];
        }
    }
}
