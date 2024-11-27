
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIItemPrefab : MonoBehaviour
{
    [SerializeField] Image mainImage;

    public void Init(Sprite newSprite, float animTime, Vector2 startPos, Vector2 finishPos)
    {
        mainImage.sprite = newSprite;
        mainImage.transform.position = startPos;
        mainImage.transform.DOScale(0.5f, 0);
        mainImage.enabled = true;

        StartItemAnimation(animTime, finishPos);
    }

    private void StartItemAnimation(float animTime, Vector2 finishPos)
    {
        float animStep = animTime / 3;

        Sequence puzzleSequence = DOTween.Sequence()
            .Append(mainImage.transform.DOScale(1f, animStep).SetEase(Ease.OutBack))
            .Join(mainImage.transform.DOMove(finishPos, animTime).SetEase(Ease.InCirc))
            .Append(mainImage.transform.DOScale(0f, animStep).SetEase(Ease.OutBack));

        puzzleSequence.Play().OnComplete(() => Destroy(gameObject));
    }
}
