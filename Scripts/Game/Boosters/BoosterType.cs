
using UnityEngine;
using UnityEngine.UI;

public class BoosterType : MonoBehaviour
{
    [SerializeField] Image boosterImg;

    [SerializeField] Sprite[] typeSpriteArray;

    public void SetType(int typeNum)
    {
        boosterImg.sprite = typeSpriteArray[typeNum];
    }
}
