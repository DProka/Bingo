using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIPilePrefab : MonoBehaviour
{
    public float animationTime { get; private set; }

    [SerializeField] RectTransform parent;
    [SerializeField] Image[] imagerArray;

    private Transform[] childTransformArray;

    public void Init(Sprite newSprite, float lifeTime, Vector3 startPos, Vector3 finishPos)
    {
        parent.gameObject.SetActive(false);
        childTransformArray = new Transform[imagerArray.Length];

        for (int i = 0; i < imagerArray.Length; i++)
        {
            imagerArray[i].sprite = newSprite;
            childTransformArray[i] = imagerArray[i].transform;
            childTransformArray[i].DOScale(0f, 0f);
        }

        animationTime = lifeTime;
        transform.position = startPos;
        parent.gameObject.SetActive(true);

        StartCoroutine(StartPileAnimation(finishPos));
    }

    public IEnumerator StartPileAnimation(Vector3 finishPos)
    {
        float delay = 0f;

        for (int i = 0; i < childTransformArray.Length; i++)
        {
            Sequence partSequence = DOTween.Sequence();

            partSequence
            .Append(childTransformArray[i].DOScale(1f, 0.3f).SetDelay(delay).SetEase(Ease.OutBack))
            .Append(childTransformArray[i].DOMove(finishPos, 0.5f).SetEase(Ease.InCirc))
            .Append(childTransformArray[i].DOScale(0f, 0.3f).SetDelay(0.3f).SetEase(Ease.OutBack));
            partSequence.Play();

            delay += 0.1f;
        }

        yield return new WaitForSeconds(animationTime);

        Destroy(gameObject);
    }
}
