using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIBallsCount : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ballsCountText;
    [SerializeField] GameObject ballsLeftObj;
    [SerializeField] GameObject alarmImageObj;

    
    [Header("Text Colors")]

    [SerializeField] Color whiteColor;
    [SerializeField] Color redColor;

    [Header("End Round Timer")]
    
    [SerializeField] float timeToEndRound;
    private float endRoundTimer;

    [SerializeField] float timeBeforeAlarm;
    private float beforeAlarmTimer;

    public void ResetCount()
    {
        endRoundTimer = timeToEndRound;
        beforeAlarmTimer = timeBeforeAlarm;

        ballsLeftObj.SetActive(true);
        alarmImageObj.SetActive(false);
        ballsCountText.color = whiteColor;
    }

    public void UpdateCountAnimation(int num)
    {
        if(num > 5)
            ballsCountText.color = whiteColor;
        else
            ballsCountText.color = redColor;

        UpdateCount(num);
        ballsCountText.transform.DOPunchScale(new Vector3(0.3f, 0.3f, 0.3f), 0.4f, 0);
    }

    public void UpdateCount(int num)
    {
        ballsCountText.text = $"{num}";
    }

    public void UpdateTimer()
    {
        if (beforeAlarmTimer > 0)
            beforeAlarmTimer -= Time.fixedDeltaTime;
        else
        {
            if (endRoundTimer > 0)
            {
                endRoundTimer -= Time.fixedDeltaTime;
                UpdateCount((int)endRoundTimer);
                ballsLeftObj.SetActive(false);
                alarmImageObj.SetActive(true);
                ballsCountText.color = redColor;
            }
            else
            {
                ballsLeftObj.SetActive(true);
                alarmImageObj.SetActive(false);
                ballsCountText.color = whiteColor;
            }
        }
    }

    public float GetTimer() { return endRoundTimer; }
}
