
using TMPro;
using UnityEngine;

public class CardColumn : MonoBehaviour
{
    public TextMeshProUGUI[] columnNumbersArray;

    public void Init()
    {
        columnNumbersArray = new TextMeshProUGUI[5];

        for (int i = 0; i < columnNumbersArray.Length; i++)
        {
            columnNumbersArray[i] = transform.GetChild(i).GetComponent<TextMeshProUGUI>();
            columnNumbersArray[i].raycastTarget = false;
        }
    }

    public void FillText(int[] array)
    {
        for (int i = 0; i < columnNumbersArray.Length; i++)
        {
            columnNumbersArray[i].text = array[i].ToString();
        }
    }

    public TextMeshProUGUI[] GetTextArray() { return columnNumbersArray; }
}
