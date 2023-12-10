
using UnityEngine;
using DG.Tweening;
using System.Collections;

public class UIPileReward : MonoBehaviour
{
    [Header("Pile of Reward")]

    [SerializeField] RectTransform parent;
    [SerializeField] Vector3 finishPosition;

    public RectTransform[] childTransformArray;
    private Vector3[] initialPosition;
    private Quaternion[] initialRotation;

    public void Init()
    {
        FillChildrensArrays();

        SwitchActive(false);
    }

    public void FillChildrensArrays()
    {
        childTransformArray = new RectTransform[parent.childCount];

        initialPosition = new Vector3[childTransformArray.Length];
        initialRotation = new Quaternion[childTransformArray.Length];

        for (int i = 0; i < parent.childCount; i++)
        {
            childTransformArray[i] = parent.GetChild(i).GetComponent<RectTransform>();
            initialPosition[i] = new Vector3(childTransformArray[i].position.x, childTransformArray[i].position.y, 0);
            initialRotation[i] = childTransformArray[i].rotation;
            childTransformArray[i].DOScale(0, 0.1f);
        }
    }

    public void StartRewardPileAnimation()
    {
        SwitchActive(true);

        FillChildrensArrays();

        float delay = 0f;

        for (int i = 0; i < childTransformArray.Length; i++)
        {
            childTransformArray[i].DOScale(1f, 0.3f).SetDelay(delay).SetEase(Ease.OutBack);

            childTransformArray[i].DOMove(finishPosition, 0.5f).SetDelay(delay + 0.5f).SetEase(Ease.InCirc);

            childTransformArray[i].DOScale(0f, 0.3f).SetDelay(delay + 1.5f).SetEase(Ease.OutBack);

            delay += 0.1f;
        }

        StartCoroutine(ResetPosition());
    }

    public void StartBuyPileAnimation()
    {
        SwitchActive(true);

        FillChildrensArrays();

        float delay = 0f;

        for (int i = 0; i < childTransformArray.Length; i++)
        {
            childTransformArray[i].DOScale(1f, 0.3f).SetDelay(delay).SetEase(Ease.OutBack);

            childTransformArray[i].DOMove(finishPosition, 0.5f).SetDelay(delay + 0.5f).SetEase(Ease.InCirc);

            childTransformArray[i].DOScale(0f, 0.1f).SetDelay(delay + 1.5f).SetEase(Ease.OutBack);

            delay += 0.1f;
        }

        StartCoroutine(ResetPosition());
    }

    IEnumerator ResetPosition()
    {
        yield return new WaitForSeconds(3f);

        Reset();
    }

    public void SetFinishPosition(Vector3 pos) { finishPosition = pos; }

    public void SwitchActive(bool isActive) { parent.gameObject.SetActive(isActive); }

    private void Reset()
    {
        for (int i = 0; i < childTransformArray.Length; i++)
        {
            childTransformArray[i].position = initialPosition[i];
            childTransformArray[i].rotation = initialRotation[i];
        }
    }
}
