
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using DG.Tweening;
using TMPro;

public class UIJoinGameBetTutorial : MonoBehaviour
{
    [SerializeField] Image shadeImage;
    [SerializeField] GameObject messageObject;
    [SerializeField] TextMeshProUGUI[] messageTextArray; 
    [SerializeField] SkeletonGraphic handSkeleton;

    private UIJoinGame mainScript;
    private int step = 0;

    public void Init(UIJoinGame joinGameScript)
    {
        mainScript = joinGameScript;
        ResetTutor();
    }

    private void Update()
    {
        if (gameObject.activeSelf && Input.GetMouseButton(0) && step == 1)
            CloseTutorial();
    }

    public void StartTutor()
    {
        mainScript.SetTutorBet(1);
        gameObject.SetActive(true);
        shadeImage.enabled = true;
        messageObject.SetActive(true);
        shadeImage.DOFade(0.5f, 0.5f).OnComplete(() => 
        {
            messageObject.transform.DOScale(1, 0.5f);
            handSkeleton.DOFade(1f, 0.5f).OnComplete(() =>
            {
                handSkeleton.AnimationState.SetAnimation(0, "Pressing", true);
                handSkeleton.AnimationState.TimeScale = 1f;
            });
        });
    }

    public void ContinueTutor()
    {
        mainScript.SetTutorBet(2);
        shadeImage.DOFade(0f, 0.5f);
        handSkeleton.transform.DOMoveX(2f, 0.5f);
        messageObject.transform.DOScale(0, 0.5f).OnComplete(() => 
        {
            SetMessageText("Great! With this bet you can win more rewards. Try it out for FREE now!");
            messageObject.transform.DOScale(1, 0.5f);
            shadeImage.enabled = false;
            step = 1;
        });
    }

    public void CloseTutorial()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
            mainScript.UpdateTutor();
        }
    }

    private void ResetTutor()
    {
        step = 0;
        gameObject.SetActive(false);
        shadeImage.enabled = false;
        shadeImage.DOFade(0, 0);
        messageObject.transform.DOScale(0, 0f);
        messageObject.SetActive(false);
        SetMessageText("Wow, you unlocked the Second Bet! Check it out!");
        handSkeleton.AnimationState.TimeScale = 0;
        handSkeleton.transform.DOMoveX(4f, 0f);
    }

    private void SetMessageText(string newText)
    {
        foreach (TextMeshProUGUI text in messageTextArray)
            text.text = newText;
    }
}
