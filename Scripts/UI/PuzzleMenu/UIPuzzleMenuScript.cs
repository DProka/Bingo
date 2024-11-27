
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;
using Spine;

public class UIPuzzleMenuScript : UIMenuGeneral
{
    [SerializeField] GameObject[] windowObjectsArray;

    [Header("Window 1")]

    [SerializeField] PuzzleMenuSettings menuSettings;
    [SerializeField] UIPuzzleMenuPuzzlesPart puzzlePart;
    [SerializeField] UIPuzzleMenuPartsPart partPart;
    [SerializeField] Image partImagePrefab;
    [SerializeField] UIScreenButton button;
    [SerializeField] Transform getAnotherButton;

    [Header("Window 2")]

    [SerializeField] Transform groupParent;

    [Header("Tutorial")]

    [SerializeField] SkeletonGraphic handTutorSkelet;

    private Canvas mainCanvas;
    private PuzzleMenuSave menuSave;

    private PuzzlePartPrefabScript clickedPart;

    private Image partImage;
    private int nextNum = 0;
    public int newPuzzleCount;
    public bool adPuzzleCount;
    private float buttonAnimTimer;
    private bool getAnotherIsAnimated;

    private int[] puzzlesOrderArray;
    private bool partIsDragging;

    private PuzzleGroupPrefab[] groupsArray;

    private Vector3 startHandPosition;
    private bool tutorAnimIsActive;

    public void Init()
    {
        mainCanvas = GetComponent<Canvas>();

        puzzlePart.Init(menuSettings);
        partPart.Init(menuSettings);

        LoadSettings();

        EventBus.onPuzzleOpened += AddNewPuzzle;
        EventBus.onRewardRecieved += CheckAvailableParts;
        EventBus.onWindowClosed += CheckButtonVisibility;
        EventBus.onRewardedADClosed += GetADRewardPart;

        SetPuzzleArray();
        SetPartArray();
        GetGroupArray();
        getAnotherIsAnimated = false;

        adPuzzleCount = false;

        handTutorSkelet.DOFade(0f, 0f);
        startHandPosition = handTutorSkelet.transform.position;

        CheckButtonVisibility();
    }

    private void Update()
    {
        if (mainCanvas.enabled)
        {
            DragNDropCheck();

            puzzlePart.UpdateScript();
            partPart.UpdateScript();

            if (getAnotherIsAnimated)
                AnimateGetAnotherButton();

            UpdateTutorialAnimation();
        }
    }

    public void OpenPartByAD()
    {
        adPuzzleCount = true;
        SendReport(Report.ADButtonClicked);
        MaxSdkManager.Instance.ShowRewarded("PuzzlePart");
    }

    public void GetADRewardPart(string adLocation)
    {
        if (mainCanvas.enabled && adLocation == "PuzzlePart" && adPuzzleCount)
        {
            OpenNewPartByNum();
            adPuzzleCount = false;
        }
    }

    public void OpenNewPartByNum()
    {
        partPart.OpenNewPartByNum(nextNum);
        MovePartsParentInNewPos(nextNum);
        MovePuzzleParentInNewPos(nextNum);
        CheckGetAnotherAnimation();
        nextNum += 1;

        if(newPuzzleCount > 0)
            newPuzzleCount--;

        SavePart();
    }

    public void CheckAvailableParts()
    {
        if (newPuzzleCount > 0)
        {
            UIAnimationScreen.Instance.StartPuzzleRewardAnimation(menuSave.tutorComplete == 0);
            CheckButtonVisibility();
        }
    }

    private void SwitchScrollArray(bool isActive)
    {
        puzzlePart.SwitchScroll(isActive);
        partPart.SwitchScroll(isActive);
    }

    private void CheckPuzzleCount()
    {
        if (newPuzzleCount > 0)
        {
            int count = newPuzzleCount;
            for (int i = 0; i < count; i++)
                OpenNewPartByNum();
        }
        else
        {
            int lastNum = 0;
            int targetNum;

            if (partPart.partsStatusArray.Contains(1))
                targetNum = 1;
            else
                targetNum = 2;

            for (int i = 0; i < partPart.partsStatusArray.Length; i++)
            {
                if (partPart.partsStatusArray[i] == targetNum)
                    lastNum = i;
            }

            MovePartsParentInNewPos(lastNum);
            MovePuzzleParentInNewPos(lastNum);
            CheckGetAnotherAnimation();
        }
    }

    private void AddNewPuzzle()
    {
        newPuzzleCount += 1;
        menuSave.UpdateButtonStatus();
        SavePart();
    }

    private void CheckButtonVisibility()
    {
        if (partPart.partsStatusArray[0] > 0)
            CheckButtonAttention();
        else
        {
            if (GameController.Instance.playedRoundsCount > 1)
            {
                if (menuSave.buttonIsActive == 1)
                    CheckButtonAttention();
                else
                    button.SwitchStatus(UIScreenButton.Status.Closed);
            }
            else
                button.SwitchStatus(UIScreenButton.Status.Closed);
        }
    }

    #region Puzzles

    public void SwitchPuzzleByButton(int num)
    {
        if (windowObjectsArray[0].activeSelf)
        {
            puzzlePart.SwitchPuzzleByButton(num);
        }
    }

    private void SetPuzzleArray() => puzzlePart.SetPuzzleArray();
    
    private void MovePuzzleParentInNewPos(int num)
    {
        int puzzleNum = partPart.partPrefabArray[num].puzzleNum;
        
        puzzlePart.MovePuzzleParentInNewPos(puzzleNum);
    }

    #endregion

    #region Parts

    private void SetPartArray()
    {
        partPart.SetPartArray();
        SaveArrays();
    }

    private void DragNDropCheck()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickedPart = partPart.GetPartByPosition(Input.mousePosition);

            if (clickedPart != null)
            {
                partIsDragging = true;
                partImage = Instantiate(partImagePrefab, transform);
                partImage.sprite = menuSettings.puzzlesArray[clickedPart.puzzleNum].partSpritesArray[clickedPart.partNum];
                SwitchScrollArray(false);
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (partIsDragging)
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                partImage.transform.position = mousePos;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (clickedPart != null)
            {
                CheckPuzzlePartPosition(Input.mousePosition);
                clickedPart = null;
                Destroy(partImage.gameObject);
                partIsDragging = false;
                SwitchScrollArray(true);
            }

        }
    }

    private void CheckPuzzlePartPosition(Vector2 pos)
    {
        if (clickedPart != null)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(pos);
            Vector2 partPos = puzzlePart.GetPartPosByNum(clickedPart.puzzleNum, clickedPart.partNum);

            if (mousePos.x > partPos.x - 0.7 && mousePos.x < partPos.x + 0.7
                && mousePos.y > partPos.y - 0.7 && mousePos.y < partPos.y + 0.7)
            {
                puzzlePart.OpenPartByNum(clickedPart.puzzleNum, clickedPart.partNum);
                clickedPart.SetStatus(PuzzlePartPrefabScript.Status.Recieved);

                for (int i = 0; i < partPart.partPrefabArray.Length; i++)
                {
                    if (partPart.partPrefabArray[i].partID == clickedPart.partID)
                        partPart.SetPartStatus(i, 2);
                }

                if (puzzlePart.CheckPuzzleCompleteByNum(clickedPart.puzzleNum))
                {
                    SoundController.Instance.PlaySound(SoundController.Sound.PuzzleFull);
                    SendReport(Report.PuzzleCollected);
                }
                else
                {
                    partPart.GetClosedPartReward(partPos);
                    SoundController.Instance.PlaySound(SoundController.Sound.PuzzlePart);
                    SendReport(Report.PartIsSet, $"Part (num: {clickedPart.partNum}) of puzzle (num: {clickedPart.puzzleNum}) is set");
                }

                CheckGetAnotherAnimation();
                SavePart();
            }
            else
                clickedPart.SwitchPartImage(true);
        }
    }

    private void MovePartsParentInNewPos(int partNum) => partPart.MovePartsParentInNewPos(partNum);
    
    #endregion

    #region Groups

    private void GetGroupArray()
    {
        groupsArray = new PuzzleGroupPrefab[groupParent.childCount];

        for (int i = 0; i < groupsArray.Length; i++)
        {
            groupsArray[i] = groupParent.GetChild(i).GetComponent<PuzzleGroupPrefab>();
            groupsArray[i].Init(menuSettings.groupNamesArray[i],
                new Sprite[] { menuSettings.groupMainColorArray[i], menuSettings.groupMainGreyArray[i] },
                menuSettings.groupFrameArray);
            groupsArray[i].SwitchAvtive(true);
        }

        if (nextNum < 27)
            groupsArray[groupsArray.Length - 1].SwitchAvtive(false);
    }

    #endregion

    #region Buttons

    private void CheckButtonAttention()
    {
        if(partPart.partsStatusArray.Contains(1) || newPuzzleCount > 0)
        {
            button.SwitchStatus(UIScreenButton.Status.Attention);
            button.SwitchAttentionActive(true);
        }
        else
        {
            button.SwitchStatus(UIScreenButton.Status.Open);
            button.SwitchAttentionActive(false);
        }
    }

    private void AnimateGetAnotherButton()
    {
        if (buttonAnimTimer > 0)
            buttonAnimTimer -= Time.deltaTime;
        else
        {
            getAnotherIsAnimated = false;

            getAnotherButton.DOPunchScale(new Vector3(0.03f, 0.03f, 0f), 1f, 3).OnComplete(() =>
            {
                buttonAnimTimer = menuSettings.timeBetweenGetAnotherAnim;
                CheckGetAnotherAnimation();
            });
        }
    }

    private void CheckGetAnotherAnimation() => getAnotherIsAnimated = !partPart.partsStatusArray.Contains(1);

    #endregion

    #region Tutorial

    private void UpdateTutorialAnimation()
    {
        if (tutorAnimIsActive)
        {
            if (Input.GetMouseButtonDown(0))
            {
                menuSave.UpdateTutorStatus();
                newPuzzleCount = 0;
            }
        }
        else
        {
            if (partPart.partsStatusArray[0] == 1 && windowObjectsArray[0].activeSelf && menuSave.tutorComplete == 0)
                StartTutorialAnimation();
        }
    }

    private void StartTutorialAnimation()
    {
        SwitcTutorhHand(true);
        handTutorSkelet.AnimationState.Complete += MoveTutorAnimation;

        handTutorSkelet.DOFade(1f, 0.5f).OnComplete(() =>
        {
            handTutorSkelet.AnimationState.SetAnimation(0, "Pressing", false);
            handTutorSkelet.AnimationState.TimeScale = 1f;
        });
    }

    private void MoveTutorAnimation(TrackEntry trackEntry)
    {
        handTutorSkelet.AnimationState.Complete -= MoveTutorAnimation;

        handTutorSkelet.transform.DOMoveY(startHandPosition.y + 3f, 1f).OnComplete(() =>
        {
            handTutorSkelet.DOFade(0f, 0.5f).OnComplete(() =>
            {
                handTutorSkelet.AnimationState.TimeScale = 0f;
                handTutorSkelet.transform.DOMoveY(startHandPosition.y, 0.5f).OnComplete(() => tutorAnimIsActive = false);
            });
        });
    }

    private void SwitcTutorhHand(bool isActive)
    {
        tutorAnimIsActive = isActive;
        handTutorSkelet.enabled = isActive;
        handTutorSkelet.AnimationState.TimeScale = isActive ? 1 : 0;
    }

    #endregion

    #region Tracking

    private void SendReport(Report report, string parameters = "")
    {
        switch (report)
        {
            case Report.MenuOpened:
                AppMetrica.reportEvent("Puzzle_Screen_Opened", "Puzzle screen opened");
                break;

            case Report.PartIsSet:
                AppMetrica.reportEvent("Puzzle_Part_Is_Set", parameters);
                break;

            case Report.PuzzleCollected:
                AppMetrica.reportEvent("Puzzle_Collected", "Full puzzle collected");
                break;

            case Report.ADButtonClicked:
                AppMetrica.reportEvent("Puzzle_ADButton_Clicked", "Advertisement for puzzle launched");
                break;
        }

        //Debug.Log($"Metrica_Event: {report} {parameters}");
    }

    private enum Report
    {
        MenuOpened,
        PartIsSet,
        PuzzleCollected,
        ADButtonClicked
    }

    #endregion

    #region Window

    public void CallWindow1()
    {
        mainCanvas.enabled = true;
        SwitchWindow(0);
        CheckPuzzleCount();
        EventBus.onWindowOpened?.Invoke();
        SendReport(Report.MenuOpened, "Menu with puzzles and parts was opened");
    }

    public void CallWindow2()
    {
        mainCanvas.enabled = true;
        SwitchWindow(1);
        if (nextNum >= 27)
            groupsArray[groupsArray.Length - 1].SwitchAvtive(true);
        EventBus.onWindowOpened?.Invoke();
        SendReport(Report.MenuOpened, "Menu with puzzle groups was opened");
    }

    public void CloseWindow()
    {
        getAnotherIsAnimated = false;
        mainCanvas.enabled = false;
        CloseMain();
        EventBus.onWindowClosed?.Invoke();
    }

    private void SwitchWindow(int num)
    {
        windowObjectsArray[0].SetActive(false);
        windowObjectsArray[1].SetActive(false);

        SetMainMenuOnject(windowObjectsArray[num]);
        OpenMain();
    }
    #endregion

    #region SaveLoad

    public void ResetSave()
    {
        menuSave.ResetSave();

        puzzlePart.ResetPuzzles();

        partPart.ResetParts();

        SetPuzzleArray();
        //SetPartArray();
        newPuzzleCount = 0;
    }

    private void SaveArrays()
    {
        menuSave.SavePuzzle(puzzlesOrderArray, partPart.partsOrderArray);
    }

    private void SavePart()
    {
        menuSave.SaveParts(nextNum, newPuzzleCount, partPart.partsStatusArray);
    }

    private void LoadSettings()
    {
        menuSave = new PuzzleMenuSave();
        menuSave.Load();
        nextNum = menuSave.nextNum;
        newPuzzleCount = menuSave.newPuzzleCount;
        puzzlesOrderArray = menuSave.puzzlesOrderArray;
        partPart.SetPartsOrder(menuSave.partsOrderArray);
        partPart.SetPartsStatusArray(menuSave.partsStatusArray, nextNum);
    }
    #endregion

    private void OnDestroy()
    {
        EventBus.onPuzzleOpened -= AddNewPuzzle;
        EventBus.onRewardRecieved -= CheckAvailableParts;
        EventBus.onWindowClosed -= CheckButtonVisibility;
        EventBus.onRewardedADClosed -= GetADRewardPart;
    }
}
