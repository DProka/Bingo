
using UnityEngine;
using DG.Tweening;

public class TutorialBoosterStep : MonoBehaviour
{
    [SerializeField] GameObject stepObj;
    [SerializeField] Transform stepMessage;
    [SerializeField] TutorialArrow stepHandArrow;

    public void SwitchStepActive(bool isActive) { stepObj.SetActive(isActive); }

    public void CallStep()
    {
        stepObj.SetActive(true);
        stepMessage.localScale = new Vector3(0, 0, 0);
        stepHandArrow.transform.localScale = new Vector3(0, 0, 0);
        stepMessage.DOScale(new Vector3(1, 1, 0), 0.2f);
        stepHandArrow.transform.DOScale(new Vector3(1, 1, 0), 0.2f);
        stepHandArrow.SetActive(true);
        //GameController.switchGameIsActive?.Invoke(false);
        GameController.Instance.SwitchGameIsActive(false);
    }

    public void ContinueStep()
    {
        //GameController.switchGameIsActive?.Invoke(true);
        GameController.Instance.SwitchGameIsActive(true);
        stepHandArrow.SetActive(false);
        stepObj.SetActive(false);
    }
}
