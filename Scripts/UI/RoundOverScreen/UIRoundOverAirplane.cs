
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIRoundOverAirplane : MonoBehaviour
{
    public int tutorialWasShown { get; private set; }

    [SerializeField] GameObject mainObject;

    [Header("Head Part")]

    [SerializeField] RectTransform headPartTransform;
    [SerializeField] TextMeshProUGUI countHeadText;

    [Header("Middle Part")]

    [SerializeField] RectTransform middlePartTransform;
    [SerializeField] TextMeshProUGUI[] countMiddleTextArray;
    [SerializeField] GameObject[] buttonsArray;
    [SerializeField] Image useButtonImage;
    [SerializeField] Sprite[] buttonSprites;

    [Header("Timer Part")]

    [SerializeField] float timeForAirplane = 11f;
    [SerializeField] TextMeshProUGUI[] timerTextArray;

    [Header("Tutorial")]

    [SerializeField] GameObject tutorialMessage;
    [SerializeField] float tutorialXPos;

    private UIRoundOverScreen mainScreen;

    private bool timerIsActive;
    private float airplaneTimer;

    private string tutorKey = "airplaneTutor";

    public void Init(UIRoundOverScreen main)
    {
        mainScreen = main;

        ResetPart();
        tutorialWasShown = 0;

        mainObject.SetActive(false);

        if (PlayerPrefs.HasKey(tutorKey))
            tutorialWasShown = PlayerPrefs.GetInt(tutorKey);
        else
            PlayerPrefs.SetInt(tutorKey, tutorialWasShown);
    }

    private void Update()
    {
        if (mainObject.activeSelf)
        {
            UpdateTimer();
        }
    }

    public void ResetPart()
    {
        foreach (GameObject button in buttonsArray)
            button.SetActive(false);

        headPartTransform.DOMoveY(8f, 0f);
        middlePartTransform.DOScale(0f, 0f);
        timerIsActive = false;
        airplaneTimer = timeForAirplane;
        tutorialMessage.transform.DOScale(0f, 0f);
    }

    public void UpdateAirplaneCountText()
    {
        int airplaneCount = GameController.Instance.boosterManager.airplaneCount;
        countHeadText.text = "" + airplaneCount;

        foreach (TextMeshProUGUI text in countMiddleTextArray)
            text.text = airplaneCount + " planes";

    }

    public void SwitchUseButtonActive(bool isActive) => useButtonImage.sprite = isActive ? buttonSprites[1] : buttonSprites[0];

    private void CheckTutorial()
    {
        tutorialMessage.SetActive(false);
        buttonsArray[0].SetActive(false);
        buttonsArray[1].SetActive(false);
        buttonsArray[2].SetActive(false);

        if (tutorialWasShown == 0)
        {
            tutorialMessage.SetActive(true);
            buttonsArray[0].SetActive(true);
        }
        else
        {
            buttonsArray[1].SetActive(true);
            buttonsArray[2].SetActive(true);
        }
    }

    public void CloseTutorial()
    {
        if (tutorialMessage.activeSelf)
            tutorialMessage.transform.DOScale(0f, 0.5f).OnComplete(() => tutorialMessage.SetActive(false));
    }

    #region Timer

    public void SwitchTimer(bool isActive) => timerIsActive = isActive;

    private void StartTimer()
    {
        airplaneTimer = timeForAirplane;
        timerIsActive = true;
    }

    private void UpdateTimer()
    {
        if (timerIsActive)
        {
            if (airplaneTimer > 0)
            {
                airplaneTimer -= Time.deltaTime;
                UpdateTimerText();
            }
            else
            {
                timerIsActive = false;
                ClosePart();
            }
        }
    }

    private void UpdateTimerText()
    {
        int remaining = (int)airplaneTimer;
        string newText = remaining >= 10 ? "00:" + remaining : "00:0" + remaining;

        foreach (TextMeshProUGUI text in timerTextArray)
            text.text = newText;
    }

    #endregion

    #region Screen

    public void SwitchObjectActive(bool isActive)
    {
        mainObject.SetActive(isActive);
        UpdateAirplaneCountText();
        CheckTutorial();
    }

    public void OpenPart()
    {
        mainObject.SetActive(true);
        SwitchUseButtonActive(true);
        CheckTutorial();
        UpdateAirplaneCountText();
        StartOpenAnimation(1f);
    }

    public void ClosePart() => StartCloseAnimation();
    
    private void StartOpenAnimation(float delay)
    {
        headPartTransform.DOMoveY(4.8f, 0.5f).SetDelay(delay);
        middlePartTransform.DOScale(1f, 0.5f).SetDelay(delay).OnComplete(() =>
        {
            StartTimer();
        });

        if (tutorialWasShown == 0)
        {
            tutorialMessage.transform.DOScale(1f, 0.5f).SetDelay(delay);
            tutorialWasShown = 1;
        }
    }

    private void StartCloseAnimation()
    {
        headPartTransform.DOMoveY(8f, 0.5f);
        middlePartTransform.DOScale(0f, 0.5f).OnComplete(() =>
        {
            ResetPart();
            mainObject.SetActive(false);
            mainScreen.CallScreenByNum(1);
        });

        if (tutorialMessage.activeSelf)
            tutorialMessage.transform.DOScale(0f, 0.5f);
    }
    #endregion
}
