
using System;
using System.Collections;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    private GameController gameController;

    public static event Action<int> callRoomStep;
    public static event Action<int> callGameStep;

    [Header("Tutorial Links")]

    [SerializeField] RoomPrefabScript roomPrefab;
    [SerializeField] GameObject roomScreens;
    [SerializeField] GameObject gameScreens;

    private int roomProgress;
    private int gameProgress;
    private int stepProgress;

    [Header("Tutorial Room Steps")]
 
    [SerializeField] TutorialCleaningStep stepCleaning;
    [SerializeField] TutorialPlayGameStep stepPlayGame;
    [SerializeField] TutorialBuyFloorStep stepBuyFloor;
    [SerializeField] TutorialChooseFloorStep stepChooseFloor;
    [SerializeField] TutorialBuyWallsStep stepBuyWalls;
    [SerializeField] TutorialBackGroundsStep backGroundsStep;
    [SerializeField] TutorialNewRoomStep newRoomStep;

    [Header("Tutorial Game Steps")]

    [SerializeField] TutorialBallsCountStep stepBallsCount;
    [SerializeField] TutorialHandArrowStep stepHand;
    [SerializeField] TutorialBoosterStep stepBooster;
    [SerializeField] TutorialKeepGoingStep stepKeepGoing;

    [Header("In Game Pause")]

    [SerializeField] float timeBeforePause;

    private float pauseTimer;
    private bool pauseSwitchTimer;
    private int pauseChipNum;
    private Vector3 pauseChipPos;

    public void Init(GameController gc, RoomPrefabScript currentRoom)
    {
        gameController = gc;
        roomPrefab = currentRoom;

        roomProgress = 0;
        gameProgress = 0;
        stepProgress = 0;

        roomScreens.SetActive(true);
        gameScreens.SetActive(false);

        callRoomStep += UpdateRoomProgress;
        callGameStep += UpdateGameProgress;

        PrepareRoomSteps();

        pauseSwitchTimer = false;
        pauseTimer = timeBeforePause;
    }

    public void UpdateTutorial()
    {
        if(gameProgress < 5)
        {
            if (pauseSwitchTimer)
            {
                if (pauseTimer > 0)
                    pauseTimer -= Time.fixedDeltaTime;

                else
                    OpenGamePauseStep();
            }
            else
                pauseTimer = timeBeforePause;
        }
    }

    public void StartTutorial()
    {
        HideSteps();
        UpdateRoomProgress(1);
    }

    public void HideSteps()
    {
        HideRoomTutorial();
        HideGameTutorial();
    }

    public void HideRoomTutorial()
    {
        stepCleaning.SwitchStepActive(false);
        stepPlayGame.SwitchStepActive(false);
        stepBuyFloor.SwitchStepActive(false);
        stepChooseFloor.SwitchStepActive(false);
        stepBuyWalls.SwitchStepActive(false);
        backGroundsStep.SwitchStepActive(false);
    }

    public void HideGameTutorial()
    {
        stepBallsCount.SwitchStepActive(false);
        stepBooster.SwitchStepActive(false);
        stepHand.CloseStep();
        SwitchKeepGoingScreen(false);
    }

    public void UpdateRoomProgress(int step)
    {
        roomProgress = step;

        Debug.Log($"roomProgress = {roomProgress}");

        CheckRoomProgress();
    }
    
    private void CheckRoomProgress()
    {
        switch (roomProgress)
        {
            case 1:
                CallStepCleaningRoom();
                break;

            case 2:
                CallStepPlayGame();
                break;

            case 3:
                UpdateGameProgress(1);
                break;

            case 4:
                CallStepBuyFloor();
                break;
                
            case 5:
                CallStepBuyWalls();
                break;
                
            case 6:
                FinishTutorial();
                break;
        }

        gameController.UpdateTutorialProgress();
    }

    public void CallScreenByNum(int num)
    {
        switch (num)
        {
            case 6:
                backGroundsStep.CallScreen();
                break;
        }
    }

    public int GetRoomProgress() { return roomProgress; }

    public void UpdateGameProgress(int step)
    {
        gameProgress = step;

        Debug.Log($"gameProgress = {gameProgress}");

        CheckGameProgress();
    }

    private void CheckGameProgress()
    {
        switch (gameProgress)
        {
            case 1:
                StartCoroutine(CallStartGame());
                break;

            case 2:
                StartTutorialGame();
                break;

            case 3:
                StartCoroutine(CallBoosterStep());
                break;

            case 4:
                SwitchKeepGoingScreen(true);
                break;
        }

        gameController.UpdateTutorialProgress();
    }

    public int GetGameProgress() { return gameProgress; }

    public void FinishTutorial() 
    {
        HideSteps();
        gameController.FinishTutorial(); 
    }

    private void SwitchGameActive(bool isActive) => GameController.Instance.SwitchGameIsActive(isActive);

    #region Room Steps

    public void BuyRoomObject(RoomObjectScript obj, int price) => roomPrefab.PurchaseObject(obj.id, price);

    public void OpenFloorColorScreen()
    {
        if (roomProgress == 4)
            stepChooseFloor.OpenFloorScreen();
    }

    public void CloseFloorColorScreen() => stepChooseFloor.CloseFloorScreen();
    public void CloseWallsScreen() => stepBuyWalls.ContinueStep();

    public void SwitchRoomVisibility(bool isActive) => roomScreens.SetActive(isActive);
    public void CallStepNewRoom() => newRoomStep.CallScreen();
    private void CallStepCleaningRoom() => StartCoroutine(stepCleaning.CallCleaningRoomScreen());
    private void CallStepPlayGame() => stepPlayGame.CallPlayGameScreen();

    private void CallStepBuyFloor()
    {
        roomScreens.SetActive(true);
        stepBuyFloor.CallScreen();
    }

    private void CallStepBuyWalls() => stepBuyWalls.CallScreen();
    private void PrepareRoomSteps()
    {
        RoomObjectScript[] oldArray = roomPrefab.GetFirstTwoObjects();
        RoomObjectScript[] newArray = new RoomObjectScript[oldArray.Length];

        for (int i = 0; i < oldArray.Length; i++)
        {
            for (int j = 0; j < newArray.Length; j++)
            {
                if (oldArray[i]._tier == j + 1)
                    newArray[j] = oldArray[i];
            }
        }

        stepBuyFloor.SetStepObject(newArray[0]);
        stepBuyWalls.SetStepObject(newArray[1]);
    }
    #endregion

    #region Game Steps

    public void SwitchPauseBool(bool pause) { pauseSwitchTimer = pause; }

    public void CloseTutorialChip(int chipNum) { gameController.CloseTutorialChip(chipNum); }

    public void SwitchKeepGoingScreen(bool isActive) 
    { 
        if(gameProgress < 5)
            stepKeepGoing.SwitchKeepGoing(isActive); 
    }

    public void ActivateStepWithHand(int chipNum, Vector3 chipPos)
    {
        if (gameProgress < 5)
        {
            if (!pauseSwitchTimer && GameController.tutorialIsActive)
            {
                stepProgress++;

                if (stepProgress <= 2)
                    pauseTimer = 0.2f;

                else
                    pauseTimer = timeBeforePause;

                SwitchGameActive(false);
                pauseChipNum = chipNum;
                pauseChipPos = chipPos;
                pauseSwitchTimer = true;
            }
        }
    }

    public void CloseStepWithHand()
    {
        if (GameController.tutorialIsActive && gameProgress < 5)
        {
            if (stepHand.gameObject.activeSelf)
            {
                stepHand.CloseStep();
            }

            pauseSwitchTimer = false;
            SwitchGameActive(true);

            if (pauseTimer == 3)
            {
                CloseTutorialChip(pauseChipNum);
                gameController.SetBallGenTimer(0.3f);
            }
            else
            {
                gameController.SetBallGenTimer(4.5f - pauseTimer);
                stepHand.CloseStep();
            }
        }
    }

    public IEnumerator CallBoosterStep()
    {
        if(gameProgress < 5)
        {
            SwitchGameActive(false);

            yield return new WaitForSeconds(0.2f);

            stepBooster.CallStep();
        }
    }

    public void ContinueBoosterStep() { stepBooster.ContinueStep(); }

    public void SwitchTutorialVisibility(bool isVisible) 
    {
        roomScreens.SetActive(isVisible);

        if (stepBuyFloor.isActiveAndEnabled)
            stepBuyFloor.CallScreen();

        if (stepBuyWalls.isActiveAndEnabled)
            stepBuyWalls.CallScreen();
    }

    private IEnumerator CallStartGame()
    {
        roomScreens.SetActive(false);
        gameController.PrepareTutorialGame();

        yield return new WaitForSeconds(0.5f);

        gameScreens.SetActive(true);
        SwitchGameActive(false);
        stepBallsCount.CallStep();
    }

    private void StartTutorialGame()
    {
        roomScreens.SetActive(false);
        gameScreens.SetActive(true);

        gameController.StartGameLogic(1);
    }

    private void OpenGamePauseStep()
    {
        if (gameProgress < 5)
            stepHand.OpenPauseWithHand(pauseChipPos);
        pauseSwitchTimer = false;
    }
    #endregion

    private void OnDestroy()
    {
        callRoomStep -= UpdateRoomProgress;
        callGameStep -= UpdateGameProgress;
    }
}
