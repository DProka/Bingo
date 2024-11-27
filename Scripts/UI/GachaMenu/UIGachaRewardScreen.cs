using System.Collections;
using UnityEngine;
using Spine.Unity;
using TMPro;
using DG.Tweening;
using Spine;

public class UIGachaRewardScreen : MonoBehaviour
{
    [SerializeField] SkeletonGraphic capsuleSkelet;
    [SerializeField] Transform rewardObjTransform;
    [SerializeField] RectTransform buttonsObjTransform;
    [SerializeField] RectTransform x2Transform;
    [SerializeField] SkeletonGraphic glowSkelet;
    [SerializeField] TextMeshProUGUI rewardText;
    [SerializeField] Transform[] rewardPositions;

    private UIGachaMenuScript mainScript;
    private int lastCapsuleNum;

    public void Init(UIGachaMenuScript _mainScript)
    {
        mainScript = _mainScript;
    }

    public void SwitchCapsuleVisibility(bool isVisible) => capsuleSkelet.DOFade(isVisible ? 1f : 0f, 0f);

    public void SwitchButtons(bool isActive) => buttonsObjTransform.gameObject.SetActive(isActive);

    public void StartCoinAnimation() => UIAnimationScreen.Instance.StartPileAnimation(UIAnimationPilePart.Type.Coins, rewardPositions[1].position, rewardPositions[0].position);

    public void UpdateRewardText(int count) => rewardText.text = "x" + count;

    public void StartCapsuleAnimation()
    {
        mainScript.SwitchWindow(1);

        rewardObjTransform.DOScale(0, 0f);
        buttonsObjTransform.DOSizeDelta(new Vector2(1060, buttonsObjTransform.sizeDelta.y), 0);
        x2Transform.DOScale(1.35f, 0);
        glowSkelet.AnimationState.TimeScale = 0f;
        capsuleSkelet.AnimationState.Complete += OpenCapsuleAnimation;
        lastCapsuleNum = lastCapsuleNum = Random.Range(0, 4);

        switch (lastCapsuleNum)
        {
            case 0:
                capsuleSkelet.AnimationState.SetAnimation(0, "Create_Blue", false);
                break;

            case 1:
                capsuleSkelet.AnimationState.SetAnimation(0, "Create_Green", false);
                break;

            case 2:
                capsuleSkelet.AnimationState.SetAnimation(0, "Create_Orang", false);
                break;

            case 3:
                capsuleSkelet.AnimationState.SetAnimation(0, "Create_Purple", false);
                break;
        }

        capsuleSkelet.AnimationState.TimeScale = 1f;
    }

    private void OpenCapsuleAnimation(TrackEntry trackEntry)
    {
        capsuleSkelet.AnimationState.Complete -= OpenCapsuleAnimation;
        capsuleSkelet.AnimationState.Complete += HideCapsuleAnimation;

        switch (lastCapsuleNum)
        {
            case 0:
                capsuleSkelet.AnimationState.SetAnimation(0, "Open1_Blue", false);
                break;

            case 1:
                capsuleSkelet.AnimationState.SetAnimation(0, "Open1_Green", false);
                break;

            case 2:
                capsuleSkelet.AnimationState.SetAnimation(0, "Open1_Orang", false);
                break;

            case 3:
                capsuleSkelet.AnimationState.SetAnimation(0, "Open1_Purple", false);
                break;
        }
        SoundController.Instance.PlaySound(SoundController.Sound.PuzzleFull);
    }

    private void HideCapsuleAnimation(TrackEntry trackEntry)
    {
        capsuleSkelet.AnimationState.Complete -= HideCapsuleAnimation;

        capsuleSkelet.DOFade(0f, 0.5f).OnComplete(() =>
        {
            glowSkelet.AnimationState.SetAnimation(0, "Idel", true);
            glowSkelet.AnimationState.TimeScale = 1f;
        });

        rewardObjTransform.DOScale(1, 0.5f);
    }

}