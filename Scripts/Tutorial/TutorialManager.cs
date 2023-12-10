
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Header("Main Links")]

    private GameController gameController;
    private UIController uiController;

    [Header("Tutorial Links")]

    [SerializeField] TutorialScreen tutorialScreen;
    
    private int tutorialProgress;
    private int stepProgress;

    [Header("In Game Pause")]

    [SerializeField] float timeBeforePause;
    public float pauseTimer;
    public bool pauseSwitchTimer;

    private int pauseChipNum;
    private Vector3 pauseChipPos;

    public void Init(GameController gc, UIController ui)
    {
        tutorialScreen.Init(this);

        gameController = gc;
        uiController = ui;

        tutorialProgress = 0;
        stepProgress = 0;

        pauseSwitchTimer = false;
        pauseTimer = timeBeforePause;
    }

    public void UpdateTutorial()
    {
        if (pauseSwitchTimer)
        {
            if (pauseTimer > 0)
                pauseTimer -= Time.fixedDeltaTime;
            else
            {
                OpenGamePauseStep();
            }
        }
        else
            pauseTimer = timeBeforePause;
    }

    public void CalculateCoins(bool isCoins, int count)
    {
        if (isCoins)
            gameController.CalculateCoins(false, count);
        else
            gameController.CalculateMoney(false, count);
    }

    public void CloseTutorialChip(int chipNum)
    {
        gameController.CloseTutorialChip(chipNum);
    }

    public void StartTutorial()
    {
        UpdateTutorialProgress(1);
    }

    public void UpdateTutorialProgress(int step)
    {
        tutorialProgress = step;
        CheckTutorialProgress();
    }
    
    private void CheckTutorialProgress()
    {
        if (tutorialProgress == 1)
            CallStep1();
        
        if (tutorialProgress == 2)
            CallStep2();

        if (tutorialProgress == 3)
            StartCoroutine(CallStep3());

        if (tutorialProgress == 4)
            StartTutorialGame();
            //StartCoroutine(CallStep4());
        
        if (tutorialProgress == 5)
            //StartCoroutine(CallStep5());

        if (tutorialProgress == 6)
            CallStep6();

        if (tutorialProgress == 8)
            CallStep8();
        
        if (tutorialProgress == 9)
            CallStep9();

        if (tutorialProgress == 10)
            CallStep10();
        
        if (tutorialProgress == 11)
            CallStep11();

        if (tutorialProgress == 20)
            OpenChooseFloorColor();

        Debug.Log($"Step Progress Updated to {tutorialProgress}");
    }

    private void StartTutorialGame()
    {
        gameController.StartTutorialGame();
        //UpdateTutorialProgress(5);
    }

    private void ClearSteps()
    {
        //stepProgress = 0;
        tutorialScreen.HideScreens();
    }

    public void ActivateBooster()
    {
        tutorialScreen.HideScreens();
    }

    public void BuyRoomObject(RoomObjectScript obj, int price)
    {
        uiController.BuyTutorialObject(obj, price);
    }

    public void FinishTutorial() { gameController.SetTutorialDone();
        Debug.Log($"TutorialStatus - {GameController.tutorialIsActive}");
    }
    
    public void SwitchKeepGoingScreen(bool isActive) { tutorialScreen.SwitchKeepGoing(isActive); }
    
    public void OpenChooseFloorColor() { tutorialScreen.OpenFloorScreen(); }
    
    #region Steps

    private void CallStep1()
    {
        StartCoroutine(tutorialScreen.CallStep1Screen());
    }

    private void CallStep2()
    {
        tutorialScreen.CallStep2Screen();
    }
    
    private IEnumerator CallStep3()
    {
        ClearSteps();
        gameController.PrepareTutorialGame();
        

        yield return new WaitForSeconds(0.5f);

        gameController.SwitchGameIsActive(false);
        tutorialScreen.CallStep3Screen();
    }

    //private IEnumerator CallStep4()
    //{
    //    ClearSteps();

    //    //yield return new WaitForSeconds(3f);

    //    //StartTutorialGame();

    //    yield return new WaitForSeconds(4f);

    //    gameController.SwitchGameIsActive(false);
    //    tutorialScreen.CallStep4Screen();
    //}
    
    //private IEnumerator CallStep5()
    //{
    //    ClearSteps();
    //    gameController.SwitchGameIsActive(true);

    //    yield return new WaitForSeconds(5.5f);

    //    gameController.SwitchGameIsActive(false);
    //    tutorialScreen.CallStep5Screen();
    //}

    private void CallStep6()
    {
        ClearSteps();
        gameController.SwitchGameIsActive(true);
    }

    public IEnumerator CallStep7()
    {
        ClearSteps();
        UpdateTutorialProgress(7);
        gameController.SwitchGameIsActive(false);
     
        yield return new WaitForSeconds(0.2f);
        
        tutorialScreen.CallStep7Screen();
    }

    private void CallStep8()
    {
        ClearSteps();
        tutorialScreen.ContinueStep7();
        gameController.SwitchGameIsActive(true);
    }

    private void CallStep9()
    {
        ClearSteps();
        tutorialScreen.CallStep9Screen();
    }
    
    private void CallStep10()
    {
        ClearSteps();
        tutorialScreen.CallStep10Screen();
    }
    
    private void CallStep11()
    {
        ClearSteps();
        FinishTutorial();
    }
    #endregion

    public void HideFloorColorScreen() { tutorialScreen.SwitchChooseFoor(false); }

    public void ActivateStepWithHand(int chipNum, Vector3 chipPos)
    {
        if(!pauseSwitchTimer && GameController.tutorialIsActive)
        {
            stepProgress++;

            if (stepProgress <= 2)
            {
                pauseTimer = 0.2f;   
            }
            else
            {
                pauseTimer = timeBeforePause;
            }

            gameController.SwitchGameIsActive(false);
            pauseChipNum = chipNum;
            pauseChipPos = chipPos;
            pauseSwitchTimer = true;
        }
    }

    private void OpenGamePauseStep()
    {
        tutorialScreen.OpenPauseWithHand(pauseChipPos);
        pauseSwitchTimer = false;
    }

    public void CloseStepWithHand()
    {
        if (GameController.tutorialIsActive)
        {
            pauseSwitchTimer = false;
            gameController.SwitchGameIsActive(true);

            if (pauseTimer == 3)
            {
                gameController.CloseTutorialChip(pauseChipNum);
                gameController.SetBallGenTimer(0.3f);
            }
            else
            {
                gameController.SetBallGenTimer(4.5f - pauseTimer);
            }  
        }
    }

    public void SwitchPauseBool(bool pause) { pauseSwitchTimer = pause; }
}
