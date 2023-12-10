
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BoosterProgressSlot : MonoBehaviour
{
    [SerializeField] Sprite closedSprite;
    [SerializeField] Sprite openedSprite;

    private Image slotImage;

    public void Init()
    {
        slotImage = GetComponent<Image>();
    }

    public void OpenSlot()
    {
        slotImage.sprite = openedSprite;
        transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0), 0.3f, 10);
    }

    public void CloseSlot()
    {
        slotImage.sprite = closedSprite;
    }
}
