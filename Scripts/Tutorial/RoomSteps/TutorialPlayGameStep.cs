
using UnityEngine;
using DG.Tweening;

public class TutorialPlayGameStep : MonoBehaviour
{
    [SerializeField] GameObject stepObj;
    [SerializeField] Transform stepMessage;
    [SerializeField] TutorialArrow stepArrow;
    
    private TutorialManager tutorialManager;

    public void SwitchStepActive(bool isActive) { stepObj.SetActive(isActive); }

    public void CallPlayGameScreen()
    {
        tutorialManager = GameController.tutorialManager;

        stepObj.SetActive(true);
        stepMessage.localScale = new Vector3(0, 0, 0);
        stepMessage.DOScale(new Vector3(1, 1, 0), 0.2f);
        stepArrow.SetActive(true);
    }

    public void StartTutorialGame()
    {
        stepArrow.SetActive(false);
        stepObj.SetActive(false);
        tutorialManager.UpdateRoomProgress(3);
    }
}
