
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using Spine;
using DG.Tweening;
using TMPro;

public class UIGachaMenuScript : UIMenuGeneral
{
    [SerializeField] GameObject[] windowsArray;

    [Header("Buttons")]

    [SerializeField] SkeletonGraphic mainButtonSkelet;
    [SerializeField] Image mainButtonImage;

    [SerializeField] Image claimButtonImage;
    [SerializeField] TextMeshProUGUI[] claimButtonText;

    [Header("Animations")]

    [SerializeField] SkeletonGraphic gachaSkelet;
    [SerializeField] GameObject gachaStaticObject;
    [SerializeField] RectTransform energyMaskRect;
    [SerializeField] RectTransform energyRect;

    [Header("Reward Screen")]

    [SerializeField] UIGachaRewardScreen rewardScreen;

    [Header("Timer")]

    [SerializeField] TextMeshProUGUI[] timerTextArray;

    [Header("Speed Up")]

    [SerializeField] GameObject speedUpObject;
    [SerializeField] Transform speedUpTransform;
    [SerializeField] TextMeshProUGUI[] speedUpTextArray;

    [Header("Tutorial")]

    [SerializeField] Transform tutorMessage;
    [SerializeField] TutorialArrow tutorArrow;

    private Canvas mainCanvas;
    private GachaScreenSettings settings;
    private UISave save;

    private int openCapsulesCount = 0;

    private System.DateTime lastCapsuleTime;
    private System.DateTime nextCapsuleTime;
    private System.TimeSpan timeRemaining;
    private System.TimeSpan dividedTime;
    private double dividedTimeInSeconds;

    private int capsuleReward;
    private string timerText;
    private bool capsuleIsActive;
    private bool animationIsActive;
    private float animTimer = 0;
    private bool animTimerIsActive;


    private bool tutorWasShown = false;
    private bool tutorIsActive = false;

    public void Init(GachaScreenSettings _settings, UISave _save)
    {
        mainCanvas = GetComponent<Canvas>();
        settings = _settings;
        save = _save;
        LoadParams();

        EventBus.onRewardedADClosed += GetADReward;

        rewardScreen.Init(this);

        animationIsActive = false;
        animTimerIsActive = false;

        rewardScreen.SwitchCapsuleVisibility(false);
        HideTutorial();
    }

    private void Update()
    {
        if (!capsuleIsActive)
            UpdateTimer();

        if (tutorIsActive && Input.GetMouseButton(0))
            HideTutorial();

        if (animationIsActive && animTimerIsActive)
        {
            if (animTimer > 0)
                animTimer -= Time.deltaTime;
            else
                ActivateFinishGachaAnimation();
        }
        
    }

    private void GetADReward(string location)
    {
        switch (location)
        {
            case "GachaSpeedUp":
                IncreaceSpeedUp();
                break;

            case "GachaImproveReward":
                ImproveReward();
                break;
        }
    }

    #region Timer

    private void UpdateTimer()
    {
        timeRemaining = nextCapsuleTime - System.DateTime.Now;

        if (GameController.Instance.boosterManager.batteryActive)
            timeRemaining = System.TimeSpan.FromSeconds(timeRemaining.TotalSeconds / 2);

        dividedTimeInSeconds = timeRemaining.TotalSeconds / 3;

        if (timeRemaining.TotalSeconds > 0)
        {
            dividedTime = System.TimeSpan.FromSeconds(dividedTimeInSeconds);
            UpdateBattery(timeRemaining.TotalSeconds);
            UpdateTimerText(timeRemaining, timerTextArray);
            UpdateTimerText(dividedTime, speedUpTextArray);

            SwitchCapsuleActive(false); //?
        }
        else
            SwitchCapsuleActive(true);
    }

    private void ResetTimer()
    {
        nextCapsuleTime = lastCapsuleTime.AddMinutes(settings.GetMinutesByCount(openCapsulesCount));
        SwitchCapsuleActive(false);
    }

    private void UpdateTimerText(System.TimeSpan time, TextMeshProUGUI[] textArray)
    {
        if (time.Hours > 0)
            timerText = time.Hours + "h " + time.Minutes + "m";

        else
        {
            if (time.Minutes > 0)
                timerText = time.Minutes + "m " + time.Seconds + "s";
            else
                timerText = time.Seconds + "s";
        }

        foreach (TextMeshProUGUI text in textArray)
            text.text = timerText;
    }

    private void SwitchCapsuleActive(bool isActive)
    {
        capsuleIsActive = isActive;

        gachaSkelet.enabled = isActive;
        gachaSkelet.AnimationState.TimeScale = isActive ? 1 : 0;

        gachaStaticObject.SetActive(!isActive);

        if (openCapsulesCount >= 1)
            SwitchSpeedUpScreen(!isActive);
        else
            SwitchSpeedUpScreen(false);

        CheckMainButtonAnimation();
    }

    #endregion

    #region Buttons

    public void ClaimCapsuleButton()
    {
        if (capsuleIsActive)
        {
            ActivateGachaAnimation();
            SetNewReward();
        }
    }

    public void CollectReward()
    {
        GameController.Instance.CalculateCoins(capsuleReward);
        IncreaseOpenCapsulesCount();
        CloseMain();
        StartCoroutine(StartCoinRewardAnimation(0.1f));
    }

    public void IncreaseRewardByAD()
    {
        if (!UIAnimationScreen.Instance.animationIsActive)
        {
            MaxSdkManager.Instance.ShowRewarded("GachaImproveReward");

            //ImproveReward();
        }
    }

    public void IncreaceSpeedUpByAd()
    {
        MaxSdkManager.Instance.ShowRewarded("GachaSpeedUp");

        //IncreaceSpeedUp();
    }

    public void CloseWindow()
    {
        if (!animationIsActive)
            CloseMain();
    }

    private IEnumerator StartRewardTextAnimation(float delay, int count)
    {
        yield return new WaitForSeconds(delay);
        rewardScreen.UpdateRewardText(count);
    }

    private void CheckMainButtonAnimation()
    {
        if (capsuleIsActive)
        {
            mainButtonImage.enabled = false;
            foreach (TextMeshProUGUI text in timerTextArray)
                text.enabled = false;

            mainButtonSkelet.enabled = true;
            mainButtonSkelet.AnimationState.SetAnimation(0, "IDLE", true);
            mainButtonSkelet.AnimationState.TimeScale = 1f;

            claimButtonImage.sprite = settings.claimButtonSpritesArray[0];
            foreach (TextMeshProUGUI text in claimButtonText)
                text.enabled = false;
        }
        else
        {
            mainButtonImage.enabled = true;
            foreach (TextMeshProUGUI text in timerTextArray)
                text.enabled = true;

            mainButtonSkelet.enabled = false;
            mainButtonSkelet.AnimationState.TimeScale = 0f;

            claimButtonImage.sprite = settings.claimButtonSpritesArray[1];
            foreach (TextMeshProUGUI text in claimButtonText)
                text.enabled = true;
        }
    }

    private IEnumerator StartCoinRewardAnimation(float delay)
    {
        yield return new WaitForSeconds(1f);

        UIAnimationScreen.Instance.StartBecomeCoinsAnimation();
        SoundController.Instance.PlaySound(SoundController.Sound.PileOfMoney);
    }

    #endregion

    #region Gacha

    private void ActivateGachaAnimation()
    {
        gachaSkelet.AnimationState.Complete += ActivateContinueGachaAnimation;
        
        animationIsActive = true;
        gachaSkelet.AnimationState.SetAnimation(0, "Emersion_Start", false);
        SoundController.Instance.PlaySound(SoundController.Sound.GachaStart);
    }

    private void ActivateContinueGachaAnimation(TrackEntry trackEntry)
    {
        gachaSkelet.AnimationState.Complete -= ActivateContinueGachaAnimation;

        StartAnimTimer();
        gachaSkelet.AnimationState.SetAnimation(0, "Emersion_IDLE", true);
        SoundController.Instance.PlaySound(SoundController.Sound.GachaMix);
    }
    
    private void StartAnimTimer()
    {
        animTimer = settings.gachaMixDuration;
        animTimerIsActive = true;
    }

    private void ActivateFinishGachaAnimation()
    {
        gachaSkelet.AnimationState.Complete += StartCapsuleAnimation;
        animTimerIsActive = false;
        gachaSkelet.AnimationState.SetAnimation(0, "Emersion_Finish", false);
        SoundController.Instance.PlaySound(SoundController.Sound.CapsuleAppearing);
    }

    private void StartCapsuleAnimation(TrackEntry trackEntry)
    {
        gachaSkelet.AnimationState.Complete -= StartCapsuleAnimation;

        rewardScreen.StartCapsuleAnimation();
        animationIsActive = false;
    }

    private void CheckGachaActive()
    {
        if (mainCanvas.enabled)
        {
            gachaSkelet.AnimationState.TimeScale = 1f;
            gachaSkelet.AnimationState.SetAnimation(0, "IDLE", true);
        }
        else
            gachaSkelet.AnimationState.TimeScale = 0f;
    }

    private void SetNewReward()
    {
        int maxChance = Random.Range(0, 5);

        if (maxChance == 4)
            capsuleReward = 39999;
        else
            capsuleReward = Random.Range(5000, 10001);

        rewardScreen.UpdateRewardText(capsuleReward);
    }

    #endregion

    #region Capsule

    private void IncreaseOpenCapsulesCount()
    {
        openCapsulesCount++;
        lastCapsuleTime = System.DateTime.Now;
        ResetTimer();

        SaveParams();
    }

    private void ImproveReward()
    {
        float step = capsuleReward / settings.adBonusAnimationStepCount;
        int oldReward = capsuleReward;

        float delay = 0;
        float delayStep = settings.adBonusAnimationTime / settings.adBonusAnimationStepCount;

        rewardScreen.StartCoinAnimation();

        for (int i = 0; i < settings.adBonusAnimationStepCount; i++)
        {
            oldReward += (int)step;
            StartCoroutine(StartRewardTextAnimation(delay, oldReward));
            delay += delayStep;
        }

        capsuleReward += capsuleReward;

        rewardScreen.SwitchButtons(false);
        Invoke("CollectReward", 1.3f);
        //StartCoroutine(StartCoinRewardAnimation(1f));
        //buttonsObjTransform.DOSizeDelta(new Vector2(430, buttonsObjTransform.sizeDelta.y), 1f);
        //x2Transform.DOScale(0, 0.5f);
    }

    #endregion

    #region SpeedUp

    private void SwitchSpeedUpScreen(bool isActive)
    {
        if (isActive)
        {
            speedUpObject.SetActive(true);
            speedUpObject.transform.DOScale(1, 0.3f);
        }
        else
            speedUpObject.transform.DOScale(0, 0.3f).OnComplete(() => speedUpObject.SetActive(false));
    }

    private void UpdateBattery(double remainSec)
    {
        double totalSec = settings.GetMinutesByCount(openCapsulesCount) * 60;
        double percentagePassed = (totalSec - remainSec) / totalSec;
        energyMaskRect.sizeDelta = new Vector2(energyRect.sizeDelta.x, ((energyRect.sizeDelta.y) / 100) * ((float)percentagePassed * 100));
    }

    private void IncreaceSpeedUp()
    {
        System.DateTime newTime = nextCapsuleTime.Subtract(dividedTime * 2);
        nextCapsuleTime = newTime;

        speedUpTransform.DOPunchScale(new Vector3(0.5f, 0.5f, 0.5f), 0.5f, 6).SetDelay(0.15f);

        SaveParams();
    }

    #endregion

    #region Tutorial

    private void CallTutorial()
    {
        tutorIsActive = true;
        tutorMessage.gameObject.SetActive(true);
        tutorMessage.DOScale(1, 0.3f).OnComplete(() =>
        {
            tutorArrow.gameObject.SetActive(true);
            tutorArrow.SetActive(true);
        });
    }

    private void HideTutorial()
    {
        tutorIsActive = false;
        tutorMessage.DOScale(0, 0.3f).OnComplete(() =>
        {
            tutorMessage.gameObject.SetActive(false);
            tutorArrow.SetActive(false);
            tutorArrow.gameObject.SetActive(false);
        });
    }

    #endregion

    #region Window

    public override void OpenMain()
    {
        base.OpenMain();
        mainCanvas.enabled = true;
        CheckGachaActive();
        SwitchWindow(0);

        EventBus.onWindowOpened?.Invoke();
    }

    public override void CloseMain()
    {
        base.CloseMain();
        mainCanvas.enabled = false;
        EventBus.onWindowClosed?.Invoke();
    }

    public void SwitchWindow(int num)
    {
        foreach (GameObject obj in windowsArray)
            obj.SetActive(false);

        windowsArray[num].SetActive(true);

        SwitchShade(num == 0);
        switch (num)
        {
            case 0:
                SwitchShade(true);
                gachaSkelet.AnimationState.SetAnimation(0, "IDLE", true);
                if (!tutorWasShown && openCapsulesCount >= 1 && !capsuleIsActive)
                {
                    Invoke("CallTutorial", 0.3f);
                    tutorWasShown = true;
                    SaveParams();
                }
                break;

            case 1:
                SwitchShade(false);
                rewardScreen.SwitchCapsuleVisibility(true);
                rewardScreen.SwitchButtons(true);
                break;
        }
    }

    #endregion

    #region Save\Load

    private void SaveParams()
    {
        save.gachaTutorWasShown = tutorWasShown;
        save.SaveGacha(openCapsulesCount, lastCapsuleTime.ToString(), nextCapsuleTime.ToString());
    }

    private void LoadParams()
    {
        openCapsulesCount = save.capsulesCount;
        lastCapsuleTime = System.DateTime.Parse(save.lastCapsuleTime);
        nextCapsuleTime = System.DateTime.Parse(save.nextCapsuleTime);
        tutorWasShown = save.gachaTutorWasShown;
    }

    #endregion

    private void OnDestroy()
    {
        EventBus.onRewardedADClosed -= GetADReward;
    }
}
