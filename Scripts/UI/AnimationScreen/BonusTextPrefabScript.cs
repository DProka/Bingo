
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class BonusTextPrefabScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] textsArray;
    [SerializeField] Image itemImage;

    public void Init(Color[] textColorsArray, string bonusText, bool isItem)
    {
        itemImage.enabled = isItem;

        foreach (TextMeshProUGUI text in textsArray)
            text.text = bonusText;
        
        Material instanceMaterial0 = textsArray[0].fontMaterial;
        instanceMaterial0.SetColor(ShaderUtilities.ID_UnderlayColor, textColorsArray[0]);
        textsArray[0].fontMaterial = instanceMaterial0;

        textsArray[1].color = textColorsArray[1];

        Material instanceMaterial2 = textsArray[2].fontMaterial;
        instanceMaterial2.SetColor(ShaderUtilities.ID_UnderlayColor, textColorsArray[2]);
        textsArray[2].fontMaterial = instanceMaterial2;

        StartBonusTextAmin();
    }

    private void StartBonusTextAmin()
    {
        transform.DOScale(0, 0f);
        transform.DOScale(1, 0.3f);
        transform.DOMoveY(transform.position.y + 1f, 1.5f);
        transform.DOScale(0, 0.3f).SetDelay(1.7f).OnComplete(() =>
        {
            Destroy(transform.gameObject);
        });
    }
}
