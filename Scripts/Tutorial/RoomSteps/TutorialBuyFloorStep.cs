using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBuyFloorStep : MonoBehaviour
{
    [SerializeField] GameObject stepObj;
    [SerializeField] TutorialArrow stepArrow;
    [SerializeField] TutorialBuyButton stepBuyButton;
    [SerializeField] RoomObjectScript stepObject;

    public void SwitchStepActive(bool isActive) { stepObj.SetActive(isActive); }

    public void CallScreen()
    {
        stepBuyButton.Init();
        stepObj.SetActive(true);
        stepBuyButton.SetActive(true);
        stepArrow.SetActive(true);
        stepObject.SetObjectStatus(RoomObjectScript.Status.isClosed);
    }

    public void ActivateBuyButton()
    {
        GameController.tutorialManager.BuyRoomObject(stepObject, 50);
        stepObject.SetObjectStatus(RoomObjectScript.Status.isOpen);
        ContinueStep();
    }

    public void ContinueStep()
    {
        stepBuyButton.SetActive(false);
        stepArrow.SetActive(false);
        stepObj.SetActive(false);
    }

    public void SetStepObject(RoomObjectScript obj) 
    { 
        stepObject = obj;
        stepBuyButton.SetMainSprite(obj.GetButtonSprite());
        stepBuyButton.SetPosition(obj.GetBuyObjectPosition());
        stepArrow.transform.position = new Vector3(stepBuyButton.GetPosition().x, stepBuyButton.transform.position.y + 1.5f, 0);
    }
}
