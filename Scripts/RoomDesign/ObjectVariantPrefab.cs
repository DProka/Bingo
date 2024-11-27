
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ObjectVariantPrefab : MonoBehaviour
{
    [SerializeField] Image mainImage;
    [SerializeField] Image frameImage;
    [SerializeField] Image ADImage;

    public bool active { get; private set; }

    private bool adIsActive;

    [Header("Sprites")]

    [SerializeField] Sprite[] frameSpritesArray;
    
    public void SetMainSprite(Sprite sprite)
    {
        mainImage.sprite = sprite;
    }

    public void SetActive(bool isActive)
    {
        frameImage.sprite = isActive ? frameSpritesArray[1] : frameSpritesArray[0];
        transform.DOScale(isActive ? 1.3f : 1f, 0.3f);
        SwitchADIsActive(isActive ? false : true);
        active = isActive;
    }

    public void SwitchADIsActive(bool isActive)
    {
        ADImage.enabled = isActive;
    }
}
