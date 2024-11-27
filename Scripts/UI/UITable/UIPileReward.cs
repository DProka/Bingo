
using UnityEngine;
using DG.Tweening;

public class UIPileReward : MonoBehaviour
{
    public float animationTime { get; private set; }

    private RectTransform parent;
    private Transform[] childTransformArray;
    private Vector3[] initialPosition;
    private Quaternion[] initialRotation;

    public void Init()
    {
        parent = GetComponent<RectTransform>();
        childTransformArray = new Transform[parent.childCount];
        initialPosition = new Vector3[childTransformArray.Length];
        initialRotation = new Quaternion[childTransformArray.Length];

        for (int i = 0; i < parent.childCount; i++)
        {
            childTransformArray[i] = parent.GetChild(i).GetComponent<Transform>();
        }

        animationTime = 0.3f + 0.5f + 0.3f + (0.1f * childTransformArray.Length);

        ResetPile();

        SwitchActive(false);
    }

    public void StartPileAnimation(Vector3 finishPosition, Vector3 ? start = null)
    {
        transform.position = start ?? transform.position;

        ResetPile();
        SwitchActive(true);

        float delay = 0f;

        for (int i = 0; i < childTransformArray.Length; i++)
        {
            Sequence partSequence = DOTween.Sequence();

            partSequence
            .Append(childTransformArray[i].DOScale(1f, 0.3f).SetDelay(delay).SetEase(Ease.OutBack))
            .Append(childTransformArray[i].DOMove(finishPosition, 0.5f).SetEase(Ease.InCirc))
            .Append(childTransformArray[i].DOScale(0f, 0.3f).SetDelay(0.3f).SetEase(Ease.OutBack));
            partSequence.Play();

            delay += 0.1f;
        }

        Invoke("ResetPosition", animationTime);
    }
    
    private void ResetPosition()
    {
        for (int i = 0; i < childTransformArray.Length; i++)
        {
            childTransformArray[i].position = initialPosition[i];
            childTransformArray[i].rotation = initialRotation[i];
        }
    }
    
    private void ResetPile()
    {
        for (int i = 0; i < childTransformArray.Length; i++)
        {
            initialPosition[i] = childTransformArray[i].position;
            initialRotation[i] = childTransformArray[i].rotation;
            childTransformArray[i].DOScale(0, 0.1f);
        }
    }

    public void SwitchActive(bool isActive) { gameObject.SetActive(isActive); }
}
