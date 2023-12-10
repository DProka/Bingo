using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialBuyButton : MonoBehaviour
{
    [SerializeField] GameObject mainObject;
    [SerializeField] GameObject mainButton;
    [SerializeField] GameObject buyButton;
    [SerializeField] TutorialArrow arrow;

    private bool isOpen;
    private bool isMoving;
    private TutorialScreen tutorialScreen;

    public void Init(TutorialScreen tutorial)
    {
        tutorialScreen = tutorial;
        buyButton.transform.localScale = new Vector3(0, 1, 0);
        isOpen = false;
    }

    public void SwitchButton()
    {
        if (!isOpen)
        {
            buyButton.transform.DOScaleX(1, 0.2f);
            isOpen = true;
            StartCoroutine(arrow.MoveArrow(1));
        }
        else
        {
            buyButton.transform.DOScaleX(0, 0.2f);
            isOpen = false;
            StartCoroutine(arrow.MoveArrow(-1));
        }
    }

    public void ActivateButton(int stepNum)
    {
        if(stepNum == 1)
        tutorialScreen.ActivateBuyButton1();
        
        if(stepNum == 9)
        tutorialScreen.ActivateBuyButton9();
    
        if(stepNum == 10)
        tutorialScreen.ActivateBuyButton10();
    }

    public void SetActive(bool isActive)
    {
        mainObject.SetActive(isActive);
    }

    public Vector3 GetPosition() { return mainButton.transform.position; }

    public void SwitchActive(bool isActive) { arrow.SetActive(isActive); }
}
