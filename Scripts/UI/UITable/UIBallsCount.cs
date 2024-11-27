
using UnityEngine;
using TMPro;
using DG.Tweening;
using Spine.Unity;
using Spine;

public class UIBallsCount : MonoBehaviour
{
    public static UIBallsCount Instance;
    [SerializeField] TextMeshProUGUI ballsCountText;
    [SerializeField] GameObject ballsLeftObj;

    [Header("Text Colors")]

    [SerializeField] Color whiteColor;
    [SerializeField] Color redColor;

    [SerializeField] TMP_FontAsset[] fontColorsArray;

    [Header("End Round Timer")]

    [SerializeField] TextMeshProUGUI[] alarmTextArray;

    [SerializeField] SkeletonGraphic alarmSkel;

    [SerializeField] float timeToEndRound;
    private float endRoundTimer;

    [SerializeField] float timeBeforeAlarm;

    public void Init()
    {
        Instance = this;
        EventBus.onRoundStarted += ResetCount;
    }

    public void ResetCount(int cardsCount = 0)
    {
        endRoundTimer = timeToEndRound;

        ballsLeftObj.SetActive(true);
        ballsCountText.color = whiteColor;

        if (!ballsCountText.gameObject.activeSelf)
            SwitchAlarmText(false);

        alarmSkel.gameObject.SetActive(false);
        alarmSkel.timeScale = 0f;

        if(cardsCount <= 2)
            transform.position = new Vector3(-5.6f, transform.position.y, 0);
        else
            transform.position = new Vector3(-0.6f, transform.position.y, 0);
    }

    public void UpdateCountAnimation(int num)
    {
        if(num > 5)
            ballsCountText.color = whiteColor;

        else if(num > 0)
        {
            if (!alarmTextArray[0].gameObject.activeSelf)
            {
                SwitchAlarmText(true);
                ballsLeftObj.SetActive(true);
                alarmTextArray[0].font = fontColorsArray[1];
            }

            SoundController.Instance.PlayBeforeTimer(num);
        }
        else if (num == 0)
        {
            if (!alarmTextArray[0].gameObject.activeSelf)
            {
                SwitchAlarmText(true);
                ballsLeftObj.SetActive(false);
            }

            StartCoroutine(SoundController.Instance.StartTimerAlarm());
            StartAlarmAnimation();
        }

        UpdateCount(num);
        ballsCountText.transform.DOPunchScale(new Vector3(0.3f, 0.3f, 0.3f), 0.4f, 0);
    }

    public void UpdateCount(int num)
    {
        if(num > 0 && num < 6)
            UpdateAlarm(num);
        else
            ballsCountText.text = $"{num}";
        
    }

    private void SwitchAlarmText(bool isActive)
    {
        ballsCountText.gameObject.SetActive(isActive ? false : true);

        foreach (TextMeshProUGUI text in alarmTextArray)
        {
            text.gameObject.SetActive(isActive);
        }
    }

    private void UpdateAlarmText(int num)
    {
        foreach (TextMeshProUGUI text in alarmTextArray)
        {
            text.text = "" + num;
        }
    }

    #region Alarm

    public void UpdateAlarm(float timer)
    {
        //UpdateCount((int)timer);
        UpdateAlarmText((int)timer);
        //ballsLeftObj.SetActive(false);
        //ballsCountText.color = redColor;
    }

    public void StopAlarmAnimation() { alarmSkel.timeScale = 0f; }

    //private void ActivateAlarm() => alarmIsActive = true;

    private void StartAlarmAnimation()
    {
        ballsLeftObj.SetActive(false);
        alarmTextArray[0].font = fontColorsArray[0];
        alarmSkel.gameObject.SetActive(true);
        alarmSkel.timeScale = 0.6f;
        alarmSkel.AnimationState.Complete += OnFirstAnimationComplete;
        alarmSkel.AnimationState.SetAnimation(0, "Create", false);
    }

    private void OnFirstAnimationComplete(TrackEntry trackEntry)
    {
        alarmSkel.timeScale = 1f;
        alarmSkel.AnimationState.Complete -= OnFirstAnimationComplete;
        alarmSkel.AnimationState.SetAnimation(0, "Idle", true);
    }
    #endregion

    public float GetTimer() { return endRoundTimer; }

    private void OnDestroy()
    {
        EventBus.onRoundStarted -= ResetCount;
    }
}
