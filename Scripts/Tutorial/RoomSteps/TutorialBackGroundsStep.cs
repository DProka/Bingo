
using UnityEngine;
using DG.Tweening;
using System.Collections;

public class TutorialBackGroundsStep : MonoBehaviour
{
    [SerializeField] GameObject stepObj;
    [SerializeField] GameObject arrowObj;
    [SerializeField] Transform stepMessage;
    //[SerializeField] TutorialArrow stepArrow;
    private Vector3 startArrowPos;

    private bool arrowIsActive;

    private void LateUpdate()
    {
        if (arrowIsActive)
            arrowObj.transform.position = new Vector3(startArrowPos.x, arrowObj.transform.position.y);
    }

    public void SwitchStepActive(bool isActive) { stepObj.SetActive(isActive); }

    public void CallScreen()
    {
        startArrowPos = arrowObj.transform.position;
        stepObj.SetActive(true);
        stepMessage.localScale = new Vector3(0, 0, 0);
        stepMessage.DOScale(new Vector3(1, 1, 0), 0.2f);
        //stepArrow.SetActive(true);
        arrowIsActive = true;
        StartCoroutine(StartArrowAnimation());
    }

    public void CloseBackGroundScreen()
    {
        arrowIsActive = false;
        //stepArrow.SetActive(false);
        stepObj.SetActive(false);
    }

    private IEnumerator StartArrowAnimation()
    {
        arrowObj.transform.DOPunchPosition(new Vector3(arrowObj.transform.position.x, arrowObj.transform.position.y + 15, 0), 0.8f, 1);

        yield return new WaitForSeconds(1.3f);

        if (arrowIsActive)
            StartCoroutine(StartArrowAnimation());
    }
}
