using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialNewRoomStep : MonoBehaviour
{
    [SerializeField] GameObject stepObjMain;
    [SerializeField] Transform stepMessage1;
    [SerializeField] Transform stepMessage2;
    [SerializeField] GameObject shadeObj;
    //[SerializeField] UIPileReward pileReward;
    [SerializeField] Transform pileStart;
    [SerializeField] Transform pileFinish;
    [SerializeField] UIController uIController;

    private int stepCount;

    public void SwitchStepActive(bool isActive) { stepObjMain.SetActive(isActive); }

    public void CallScreen()
    {
        stepCount = 1;
        stepMessage1.gameObject.SetActive(false);
        stepMessage2.gameObject.SetActive(false);
        shadeObj.SetActive(false);
        CallMessage();
    }

    public void ActivateButton()
    {
        stepCount++;
        CallMessage();
    }

    public void CallMessage()
    {
        switch (stepCount)
        {
            case 1:
                stepObjMain.SetActive(true);
                stepMessage1.gameObject.SetActive(true);
                stepMessage1.localScale = new Vector3(0, 0, 0);
                stepMessage1.DOScale(new Vector3(1, 1, 0), 0.2f);
                break;
        
            case 2:
                StartCoroutine(SwitchMessage());
                break;
                
            case 3:
                StartCoroutine(CloseScreen());
                break;
        }
    }

    private IEnumerator SwitchMessage()
    {
        stepMessage1.DOScale(new Vector3(0, 0, 0), 0.3f);

        yield return new WaitForSeconds(0.3f);

        stepMessage1.gameObject.SetActive(false);
        stepMessage2.gameObject.SetActive(true);
        shadeObj.SetActive(true);
        stepMessage2.localScale = new Vector3(0, 0, 0);
        stepMessage2.DOScale(new Vector3(1, 1, 0), 0.2f);
        uIController.LoadNextRoom();
    }

    private IEnumerator CloseScreen()
    {
        //pileReward.SetFinishPosition(pileFinish.position);
        //pileReward.StartRewardPileAnimation();

        UIAnimationScreen.Instance.StartPileAnimation(UIAnimationPilePart.Type.Money, pileFinish.position, pileStart.position);
        //UIAnimationScreen.Instance.StartMoneyPileAnimation(pileFinish.position, pileStart.position);
        //GameController.updatePlayerMoney?.Invoke(200);
        GameController.Instance.CalculateMoney(200);
        stepMessage2.DOScale(new Vector3(0, 0, 0), 0.2f);
        shadeObj.SetActive(false);
        //uIController.SwitchMainMenuActive(true);
        EventBus.onWindowClosed?.Invoke();

        yield return new WaitForSeconds(3f);

        stepObjMain.SetActive(false);
        uIController.SwitchDesignManagerActive(true);
    }
}
