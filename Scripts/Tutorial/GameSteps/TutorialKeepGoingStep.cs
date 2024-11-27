using System.Collections;
using UnityEngine;
using DG.Tweening;

public class TutorialKeepGoingStep : MonoBehaviour
{
    [SerializeField] GameObject keepGoingScreen;
    [SerializeField] Transform keepGoingMessage;

    public void SwitchKeepGoing(bool isActive)
    {
        if (isActive)
        {
            keepGoingScreen.SetActive(true);
            StartCoroutine(ShowKeepGoingMessage());
        }
            
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
}
