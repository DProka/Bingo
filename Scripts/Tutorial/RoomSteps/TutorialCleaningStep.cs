using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;

public class TutorialCleaningStep : MonoBehaviour
{
    [SerializeField] GameObject stepObj;
    [SerializeField] Transform stepMessage;
    [SerializeField] TutorialArrow stepArrow;
    [SerializeField] TutorialBuyButton stepBuyButton;
    [SerializeField] Image dirtyRoomImg;

    [Header("Broom")]

    [SerializeField] SkeletonGraphic broomSpine;
    [SerializeField] float animationTime;
    [SerializeField] Vector3[] broomPosArray;

    public void SwitchStepActive(bool isActive) 
    { 
        stepObj.SetActive(isActive); 
        
        if(!isActive)
            broomSpine.enabled = false;
    }

    public IEnumerator CallCleaningRoomScreen()
    {
        stepBuyButton.Init();
        broomSpine.enabled = false;

        SwitchStepActive(true);
        stepMessage.localScale = new Vector3(0, 0, 0);
        stepArrow.transform.localScale = new Vector3(0, 0, 0);
        dirtyRoomImg.color = new Color(255, 255, 255, 255);

        yield return new WaitForSeconds(0.5f);

        stepMessage.DOScale(new Vector3(1, 1, 0), 0.2f);
        stepArrow.transform.DOScale(new Vector3(1, 1, 0), 0.2f);
        stepArrow.SetActive(true);
    }

    public void ActivateBuyButton()
    {
        GameController.Instance.CalculateMoney(-50);
        StartCoroutine(ContinueStep());

        AppMetrica.reportEvent("Room_item_bought", "Lets_get_rid");
    }

    IEnumerator ContinueStep()
    {
        stepBuyButton.SwitchButton();
        UIAnimationScreen.Instance.StartBuyObjAnimation(stepBuyButton.GetPosition());
        stepMessage.DOScale(0, 0.2f);
        stepArrow.transform.DOScale(0, 0.2f);
        stepArrow.SetActive(false);

        broomSpine.enabled = true;
        broomSpine.AnimationState.SetAnimation(0, "Animation", true);
        broomSpine.DOFade(0, 0f);

        yield return new WaitForSeconds(1.5f);
        stepBuyButton.SetActive(false);

        AnimateTrash();
        AnimateBroom();

        yield return new WaitForSeconds(5f);

        GameController.tutorialManager.UpdateRoomProgress(2);
        SwitchStepActive(false);
    }

    private void AnimateBroom()
    {
        Sequence moveSequence = DOTween.Sequence();
        float moveDuration = animationTime / broomPosArray.Length - 0.5f;

        moveSequence.Append(broomSpine.DOFade(1, 0.5f));

        foreach (Vector3 position in broomPosArray)
        {
            moveSequence.Append(broomSpine.rectTransform.DOMove(position, moveDuration).SetEase(Ease.Linear));
            moveSequence.AppendInterval(0.5f);
        }

        moveSequence.Append(broomSpine.DOFade(0, 0.5f));
        moveSequence.Play();
    }

    private void AnimateTrash()
    {
        Sequence moveSequence = DOTween.Sequence();
        float fadeDuration = animationTime / broomPosArray.Length - 0.5f;

        float[] fadeCount = new float[] { 1f, 0.6f, 0.3f };
 
        foreach (float count in fadeCount)
        {
            moveSequence.Append(dirtyRoomImg.DOFade(count, fadeDuration).SetEase(Ease.Linear));
            moveSequence.AppendInterval(0.5f);
        }

        moveSequence.Append(dirtyRoomImg.DOFade(0, 0.5f));
        moveSequence.Play();
    }
}
