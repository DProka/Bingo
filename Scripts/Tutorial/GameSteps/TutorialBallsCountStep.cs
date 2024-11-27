
using UnityEngine;
using DG.Tweening;

public class TutorialBallsCountStep : MonoBehaviour
{
    [SerializeField] GameObject stepObj;
    [SerializeField] Transform stepMessage;
    [SerializeField] TutorialArrow stepArrow;

    public void SwitchStepActive(bool isActive) { stepObj.SetActive(isActive); }

    public void CallStep()
    {
        stepObj.SetActive(true);
        stepMessage.localScale = new Vector3(0, 0, 0);
        stepMessage.DOScale(new Vector3(1, 1, 0), 0.2f);
        stepArrow.SetActive(true);
    }

    public void ContinueStep()
    {
        stepObj.SetActive(false);
        stepArrow.SetActive(false);
        GameController.tutorialManager.UpdateGameProgress(2);
    }
}
