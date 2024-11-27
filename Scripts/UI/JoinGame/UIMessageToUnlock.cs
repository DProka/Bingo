
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIMessageToUnlock : MonoBehaviour
{
    [SerializeField] Transform mainObj;
    [SerializeField] Image messageImage;
    [SerializeField] TextMeshProUGUI messageText;

    private bool messageIsActive;

    public void SetText(int lvlToUnlock)
    {
        messageText.text = "Unlock at XP Level " + lvlToUnlock;
    }

    public void HideMessage()
    {
        mainObj.localScale = new Vector3(0, 0, 0);
        messageImage.enabled = false;
        messageText.enabled = false;
        messageIsActive = false;
    }

    public void StartMessageAnimation()
    {
        if (!messageImage.enabled)
        {
            messageImage.enabled = true;
            messageText.enabled = true;

            transform.DOScale(1, 0.3f).OnComplete(() =>
            {
                transform.DOScale(0, 0.3f).SetDelay(2.7f).OnComplete(() => HideMessage());
            });
        }

        Debug.Log("Message was shown. is enabled = " + messageImage.enabled);
    }

    public void SwitchMessageActive(bool isActive)
    {
        if(messageIsActive != isActive)
        {
            messageIsActive = isActive;

            if (messageIsActive)
            {
                messageImage.enabled = true;
                messageText.enabled = true;

                transform.DOScale(1, 0.3f);
            }
            else
            {
                transform.DOScale(0, 0.3f).OnComplete(() => HideMessage());
            }
        }
    }
}
