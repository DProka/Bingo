using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialHandArrowStep : MonoBehaviour
{
    [SerializeField] GameObject stepWithHand;
    [SerializeField] Transform handButton;
    [SerializeField] TutorialArrow handArrow;

    public void OpenPauseWithHand(Vector3 chipPos)
    {
        if (!stepWithHand.activeSelf)
        {
            stepWithHand.SetActive(true);
            handButton.transform.position = chipPos;
            handButton.transform.localScale = new Vector3(0f, 0f, 0f);
            handButton.transform.DOScale(1f, 0.3f);
            handArrow.SetActive(true);
        }
    }

    public void ClosePauseWithHand()
    {
        GameController.tutorialManager.CloseStepWithHand();
        CloseStep();
    }

    public void CloseStep()
    {
        handArrow.SetActive(false);
        stepWithHand.SetActive(false);
    }
}
