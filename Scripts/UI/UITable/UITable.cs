
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UITable : MonoBehaviour
{
    [SerializeField] Image backgroundImage;

    [Header("Head Part")]

    [SerializeField] GameObject headObj;
    [SerializeField] UIBoosterTable boosterProgress;
    [SerializeField] UIBallsCount ballsCount;

    [Header("Left Part")]

    [SerializeField] GameObject leftObj;
    [SerializeField] Transform leftPart;
    [SerializeField] Transform puzzlePart;
    [SerializeField] TextMeshProUGUI puzzleCount;

    [Header("Right Part")]

    [SerializeField] GameObject rightObj;
    [SerializeField] JackpotDisplay jackpotDisplay;

    private Canvas mainCanvas;
    private TableSettings settings;

    private bool alarmIsActive;
    private float alarmTimer;

    public void Init(TableSettings _settings)
    {
        mainCanvas = GetComponent<Canvas>();

        settings = _settings;

        EventBus.onPuzzleOpened += OpenPuzzlePart;
        EventBus.onRoundStarted += ResetTable;
        EventBus.onBallsIsEmpty += ActivateAlarm;

        ballsCount.Init();
        jackpotDisplay.Init();
        boosterProgress.Init(settings);
    }

    private void Update()
    {
        if (alarmIsActive)
            UpdateAlarm();
    }

    public void UpdateBallCount(int count) => ballsCount.UpdateCount(count);

    public void SetRoundBoosters(bool[] statuses) => boosterProgress.SetRoundBoosters(statuses);
    
    public void UpdateBooster(float progress, BoosterManager.Booster boosterType, bool isActive)
    {
        boosterProgress.UpdateProgress(progress, boosterType);
        if (!isActive)
            boosterProgress.SetBoosterActive(false, 0);
    }

    public void SwitchTableCanvas(bool isActive) => mainCanvas.enabled = isActive;

    public void SwitchTableUiActive(bool isActive)
    {
        headObj.SetActive(isActive);
        leftObj.SetActive(isActive);
        rightObj.SetActive(isActive);
    }

    public void CheckJackpotDisplaySize(bool isBig) => jackpotDisplay.SetSize(isBig);
    
    private void ActivateAlarm()
    {
        alarmTimer = settings.alarmTimeBeforeEnd;
        alarmIsActive = true;
    }

    private void UpdateAlarm()
    {
        if (alarmTimer > 0)
        {
            alarmTimer -= Time.fixedDeltaTime;
            ballsCount.UpdateAlarm(alarmTimer);
        }
        else
        {
            ballsCount.StopAlarmAnimation();
            GameController.Instance.EndRound();
            alarmIsActive = false;
        }
    }

    private void ResetTable(int cardsCount)
    {
        leftPart.gameObject.SetActive(false);
        leftPart.DOScaleY(0f, 0f);
        puzzlePart.DOScale(0f, 0f);
        backgroundImage.enabled = true;
    }

    private void OpenPuzzlePart()
    {
        CheckLeftPartActive();
        puzzlePart.DOScale(1f, 0.5f).SetDelay(0.5f);
        puzzleCount.text = "1";
    }

    private void CheckLeftPartActive()
    {
        if (!leftPart.gameObject.activeSelf)
        {
            leftPart.gameObject.SetActive(true);
            leftPart.DOScaleY(1f, 0.5f);
        }
    }

    private void OnDestroy()
    {
        EventBus.onPuzzleOpened -= OpenPuzzlePart;
        EventBus.onRoundStarted -= ResetTable;
        EventBus.onBallsIsEmpty -= ActivateAlarm;
    }
}
