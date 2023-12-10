using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TutorialScreen : MonoBehaviour
{
    [SerializeField] UIPileReward buyPile;

    private TutorialManager tutorialManager;

    [Header("Step1")]

    [SerializeField] GameObject step1;
    [SerializeField] Transform step1Message;
    [SerializeField] TutorialArrow step1Arrow;
    [SerializeField] TutorialBuyButton step1BuyButton;

    [Header("Step2")]
    
    [SerializeField] GameObject step2;
    [SerializeField] Transform step2Message;
    [SerializeField] TutorialArrow step2Arrow;

    [Header("Step3")]

    [SerializeField] GameObject step3;
    [SerializeField] Transform step3Message;
    [SerializeField] TutorialArrow step3Arrow;

    //[Header("Step4")]

    //[SerializeField] GameObject step4;
    //[SerializeField] TutorialArrow step4HandArrow;

    //[Header("Step5")]

    //[SerializeField] GameObject step5;
    //[SerializeField] TutorialArrow step5HandArrow;

    [Header("Step6")]

    [SerializeField] GameObject step6;

    [Header("Step7")]

    [SerializeField] GameObject step7;
    [SerializeField] Transform step7Message;
    [SerializeField] TutorialArrow step7HandArrow;

    [Header("Step9")]
    
    [SerializeField] GameObject step9;
    [SerializeField] TutorialArrow step9Arrow;
    [SerializeField] TutorialBuyButton step9BuyButton;
    [SerializeField] RoomObjectScript floorObject;
    
    [Header("Step10")]
    
    [SerializeField] GameObject step10;
    [SerializeField] TutorialArrow step10Arrow;
    [SerializeField] TutorialBuyButton step10BuyButton;
    [SerializeField] RoomObjectScript wallsObject;
    
    [Header("Step10")]

    [SerializeField] GameObject step11;

    [Header("KeepGoing")]

    [SerializeField] GameObject keepGoingScreen;
    [SerializeField] Transform keepGoingMessage;

    [Header("ChooseFloor")]

    [SerializeField] GameObject chooseFloorScreen;
    [SerializeField] TutorialArrow floorArrow1;
    [SerializeField] TutorialArrow floorArrow2;
    [SerializeField] TutorialArrow floorArrow3;

    [Header("Pause With Hand")]

    [SerializeField] GameObject stepWithHand;
    [SerializeField] Transform handButton;
    [SerializeField] TutorialArrow handArrow;

    private int boosterProgress;

    public void Init(TutorialManager manager)
    {
        tutorialManager = manager;

        PrepareSteps();
    }

    public void HideScreens()
    {
        step1.SetActive(false);
        step2.SetActive(false);
        step3.SetActive(false);
        //step4.SetActive(false);
        //step5.SetActive(false);
        step7.SetActive(false);
        step9.SetActive(false);
        step10.SetActive(false);
        step11.SetActive(false);
        keepGoingScreen.SetActive(false);
        SwitchKeepGoing(false);
        CloseFloorScreen();
        stepWithHand.SetActive(false);
    }

    private void PrepareSteps()
    {
        step1BuyButton.Init(this);
        step9BuyButton.Init(this);
        step10BuyButton.Init(this);
        HideScreens();
    }
    #region Step1

    public IEnumerator CallStep1Screen()
    {
        step1.SetActive(true);
        step1Message.localScale = new Vector3(0, 0, 0);
        step1Arrow.transform.localScale = new Vector3(0, 0, 0);

        yield return new WaitForSeconds(0.5f);

        step1Message.DOScale(new Vector3(1, 1, 0), 0.2f);
        step1Arrow.transform.DOScale(new Vector3(1, 1, 0), 0.2f);
        step1Arrow.SetActive(true);
    }

    public void ActivateBuyButton1()
    {
        tutorialManager.CalculateCoins(false, 50);
        StartCoroutine(ActivatePileAnim(step1BuyButton.GetPosition(), 2));
    }

    IEnumerator ActivatePileAnim(Vector3 finishPos, int nextStep)
    {
        buyPile.SetFinishPosition(finishPos);
        buyPile.StartBuyPileAnimation();
        step1Message.DOScale(new Vector3(0, 0, 0), 0.2f);
        step1Arrow.transform.DOScale(new Vector3(0, 0, 0), 0.2f);
        step1Arrow.SetActive(false);

        yield return new WaitForSeconds(2f);

        tutorialManager.UpdateTutorialProgress(nextStep);
    }
    #endregion

    #region Step2
    
    public void CallStep2Screen()
    {
        PrepareSteps();

        step2.SetActive(true);
        step2Message.localScale = new Vector3(0, 0, 0);
        //step2Arrow.transform.localScale = new Vector3(0, 0, 0);
        step2Message.DOScale(new Vector3(1, 1, 0), 0.2f);
        //step2Arrow.transform.DOScale(new Vector3(1, 1, 0), 0.2f);
        step2Arrow.SetActive(true);
    }

    public void StartTutorialGame() 
    {
        PrepareSteps();
        step2Arrow.SetActive(false);
        tutorialManager.UpdateTutorialProgress(3);
    }
    
    #endregion

    #region Step3

    public void CallStep3Screen()
    {
        step3.SetActive(true);
        step3Message.localScale = new Vector3(0, 0, 0);
        //step3Arrow.transform.localScale = new Vector3(0, 0, 0);
        step3Message.DOScale(new Vector3(1, 1, 0), 0.2f);
        //step3Arrow.transform.DOScale(new Vector3(1, 1, 0), 0.2f);
        step3Arrow.SetActive(true);
    }

    public void ContinueStep3()
    {
        step3.SetActive(false);
        step3Arrow.SetActive(false);
        tutorialManager.UpdateTutorialProgress(4);
    }
    #endregion

    //#region Step4

    //public void CallStep4Screen()
    //{
    //    step4.SetActive(true);
    //    step4HandArrow.transform.localScale = new Vector3(0, 0, 0);
    //    step4HandArrow.transform.DOScale(new Vector3(1, 1, 0), 0.2f);
    //    step4HandArrow.SetActive(true);
    //}

    //public void ContinueStep4()
    //{
    //    step4HandArrow.transform.localScale = new Vector3(0, 0, 0);
    //    tutorialManager.UpdateTutorialProgress(5);
    //    tutorialManager.CloseTutorialChip(0);
    //    step4HandArrow.SetActive(false);
    //}
    //#endregion

    //#region Step5

    //public void CallStep5Screen()
    //{
    //    step5.SetActive(true);
    //    step5HandArrow.transform.localScale = new Vector3(0, 0, 0);
    //    step5HandArrow.transform.DOScale(new Vector3(1, 1, 0), 0.2f);
    //    step5HandArrow.SetActive(true);
    //}

    //public void ContinueStep5()
    //{
    //    tutorialManager.UpdateTutorialProgress(6);
    //    tutorialManager.CloseTutorialChip(2);
    //    step5HandArrow.SetActive(false);
    //}
    //#endregion

    #region Step7

    public void CallStep7Screen()
    {
        step7.SetActive(true);
        step7Message.localScale = new Vector3(0, 0, 0);
        step7HandArrow.transform.localScale = new Vector3(0, 0, 0);
        step7Message.DOScale(new Vector3(1, 1, 0), 0.2f);
        step7HandArrow.transform.DOScale(new Vector3(1, 1, 0), 0.2f);
        step7HandArrow.SetActive(true);
    }

    public void ContinueStep7()
    {
        step7HandArrow.SetActive(false);
        step7.SetActive(false);
    }
    #endregion

    #region Step9

    public void CallStep9Screen()
    {
        step9.SetActive(true);
        step9BuyButton.SetActive(true);
        step9Arrow.SetActive(true);
    }

    public void ActivateBuyButton9()
    {
        tutorialManager.BuyRoomObject(floorObject, 50);
        StartCoroutine(ActivatePileAnim(step9BuyButton.GetPosition(), 20));
        ContinueStep9();
    }

    public void ContinueStep9()
    {
        step9BuyButton.SetActive(false);
        step9Arrow.SetActive(false);
        step9.SetActive(false);
    }
    #endregion

    #region Step10

    public void CallStep10Screen()
    {
        //CloseFloorScreen();
        step10.SetActive(true);
        step10BuyButton.SetActive(true);
        step10Arrow.SetActive(true);
    }

    public void ActivateBuyButton10()
    {
        tutorialManager.BuyRoomObject(wallsObject, 100);
        StartCoroutine(ActivatePileAnim(step10BuyButton.GetPosition(), 11));
        ContinueStep10();
    }

    public void ContinueStep10()
    {
        step10BuyButton.SetActive(false);
        step10Arrow.SetActive(false);
        step10.SetActive(false);
    }
    #endregion

    #region Step11

    public void CallStep11Screen()
    {
        step11.SetActive(true);
        ContinueStep11();
    }

    public void ContinueStep11()
    {
        tutorialManager.FinishTutorial();
        step11.SetActive(false);
    }
    #endregion

    #region KeepGoing

    public void SwitchKeepGoing(bool isActive) 
    {
        if (isActive)
            StartCoroutine(ShowKeepGoingMessage());
        else
            keepGoingScreen.SetActive(false); 
    }

    private IEnumerator ShowKeepGoingMessage()
    {
        keepGoingScreen.SetActive(true);
        keepGoingMessage.localScale = new Vector3(0, 0, 0); 
        keepGoingMessage.DOScale(new Vector3(1f, 1f, 0f), 0.4f);
        
        yield return new WaitForSeconds(5f);

        keepGoingMessage.DOScale(new Vector3(0f, 0f, 0f), 0.4f);

        yield return new WaitForSeconds(0.4f);

        keepGoingScreen.SetActive(false);
    }

    #endregion

    #region ChooseFloor

    public void SwitchChooseFoor(bool isActive) 
    { 
        chooseFloorScreen.SetActive(isActive);
        floorArrow1.SetActive(isActive);
        floorArrow2.SetActive(isActive);
        floorArrow3.SetActive(isActive);
    }
    
    public void OpenFloorScreen() 
    {
        chooseFloorScreen.SetActive(true);
        floorArrow1.SetActive(true);
        floorArrow2.SetActive(true);
        floorArrow3.SetActive(true);
    }

    private void CloseFloorScreen() 
    {
        chooseFloorScreen.SetActive(false);
        floorArrow1.SetActive(false);
        floorArrow2.SetActive(false);
        floorArrow3.SetActive(false);
    }
    #endregion

    public void OpenPauseWithHand(Vector3 chipPos)
    {
        stepWithHand.SetActive(true);
        handButton.transform.position = chipPos;
        handButton.transform.localScale = new Vector3(0f, 0f, 0f);
        handButton.transform.DOScale(1f, 0.3f);
        handArrow.SetActive(true);
    }

    public void ClosePauseWithHand()
    {
        tutorialManager.CloseStepWithHand();
        handArrow.SetActive(false);
        stepWithHand.SetActive(false);
    }
}
