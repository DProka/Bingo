
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class CardNumbers : MonoBehaviour
{
    [Header("Text Settings")]

    [SerializeField] TMP_FontAsset fontWithBorder;
    [SerializeField] TMP_FontAsset fontWithoutBorder;

    [SerializeField] Color textColorBlack;
    [SerializeField] Color textColorWhite;

    [Header("Column Links")]

    public CardColumn columnB;
    public CardColumn columnI;
    public CardColumn columnN;
    public CardColumn columnG;
    public CardColumn columnO;

    private int[] cardNumbersArray;
    private TextMeshProUGUI[] numbersTextArray;

    public void Init()
    {
        cardNumbersArray = new int[25];
        numbersTextArray = new TextMeshProUGUI[25];

        columnB.Init();
        columnI.Init();
        columnN.Init();
        columnG.Init();
        columnO.Init();

        for (int i = 0; i < numbersTextArray.Length; i++)
        {
            if (i <= 4)
                numbersTextArray[i] = columnB.GetTextArray()[i];
        
            if (i >=5 && i <= 9)
                numbersTextArray[i] = columnI.GetTextArray()[i - 5];
        
            if (i >=10 && i <= 14)
                numbersTextArray[i] = columnN.GetTextArray()[i - 10];
        
            if (i >=15 && i <= 19)
                numbersTextArray[i] = columnG.GetTextArray()[i - 15];
        
            if (i >=20 && i <= 24)
                numbersTextArray[i] = columnO.GetTextArray()[i - 20];
        }
    }

    public void FillEmptyNumbers(int[] numbersArray)
    {
        cardNumbersArray = numbersArray;

        int[] b = new int[5];
        int[] i = new int[5];
        int[] n = new int[5];
        int[] g = new int[5];
        int[] o = new int[5];

        for (int s = 0; s < cardNumbersArray.Length; s++)
        {
            if (s < 5)
                b[s] = cardNumbersArray[s];

            if (s >= 5 && s < 10)
                 i[s - 5] = cardNumbersArray[s];

            if (s >= 10 && s < 15)
                n[s - 10] = cardNumbersArray[s];

            if (s >= 15 && s < 20)
                g[s - 15] = cardNumbersArray[s];

            if (s >= 20 && s < cardNumbersArray.Length)
                o[s - 20] = cardNumbersArray[s];
        }

        int[] b2 = RandomizeNumbers(1, 16, b);
        int[] i2 = RandomizeNumbers(16, 31, i);
        int[] n2 = RandomizeNumbers(31, 46, n);
        int[] g2 = RandomizeNumbers(46, 61, g);
        int[] o2 = RandomizeNumbers(61, 76, o);

        for (int s = 0; s < cardNumbersArray.Length; s++)
        {
            if (cardNumbersArray[s] == 0)
            {
                if (s < 5)
                    cardNumbersArray[s] = b2[s];

                if (s >= 5 && s < 10)
                    cardNumbersArray[s] = i2[s - 5];

                if (s >= 10 && s < 15)
                    cardNumbersArray[s] = n2[s - 10];

                if (s >= 15 && s < 20)
                    cardNumbersArray[s] = g2[s - 15];

                if (s >= 20 && s < cardNumbersArray.Length)
                    cardNumbersArray[s] = o2[s - 20];
            }
        }

        FillNumbersText();
    }

    private void FillNumbersText()
    {
        int[] b = new int[5];
        int[] i = new int[5];
        int[] n = new int[5];
        int[] g = new int[5];
        int[] o = new int[5];

        for (int s = 0; s < cardNumbersArray.Length; s++)
        {
            if (s < 5)
                b[s] = cardNumbersArray[s];

            if (s >= 5 && s < 10)
                i[s - 5] = cardNumbersArray[s];

            if (s >= 10 && s < 15)
                n[s - 10] = cardNumbersArray[s];

            if (s >= 15 && s < 20)
                g[s - 15] = cardNumbersArray[s];

            if (s >= 20 && s < cardNumbersArray.Length)
                o[s - 20] = cardNumbersArray[s];
        }

        columnB.FillText(b);
        columnI.FillText(i);
        columnN.FillText(n);
        columnG.FillText(g);
        columnO.FillText(o);
    }

    private int[] RandomizeNumbers(int minValue, int maxValue, int[] arrayPart)
    {
        int[] values = arrayPart;

        List<int> newValues = new List<int>();

        for (int i = minValue; i < maxValue; i++)
        {
            newValues.Add(i);
        }

        foreach (int num in values)
        {
            if (newValues.Contains(num))
                newValues.Remove(num);
        }

        for (int i = 0; i < values.Length; i++)
        {
            if(values[i] == 0)
            {
                int newNum = Random.Range(0, newValues.Count);
                values[i] = newValues[newNum];
                newValues.Remove(newValues[newNum]);
            }
        }

        return values;
    }
   
    public int[] GetNumbersArray() { return cardNumbersArray; }

    public void ResetNumbersVisibility()
    {
        foreach (TextMeshProUGUI text in numbersTextArray)
        {
            text.gameObject.SetActive(true);
            text.font = fontWithoutBorder;
            text.color = textColorBlack;
        }
    }

    public void TurnOffTextNum(int num) { numbersTextArray[num].gameObject.SetActive(false); }
    
    public void SetTextBorder(int num) 
    {
        numbersTextArray[num].font = fontWithBorder; 
        numbersTextArray[num].color = textColorWhite;
    }

    #region Tutorial

    public void SetTutorialNumbers()
    {
        cardNumbersArray = new int[25];

        int[] b = new int[5] { 6, 15, 11, 8, 3 };
        int[] i = new int[5] { 23, 18, 29, 24, 16 };
        int[] n = new int[5] { 34, 31, 40, 38, 42 };
        int[] g = new int[5] { 53, 60, 57, 49, 52 };
        int[] o = new int[5] { 72, 67, 68, 61, 74 };

        columnB.FillText(b);
        columnI.FillText(i);
        columnN.FillText(n);
        columnG.FillText(g);
        columnO.FillText(o);

        for (int s = 0; s < cardNumbersArray.Length; s++)
        {
            if (s < 5)
                cardNumbersArray[s] = b[s];

            if (s >= 5 && s < 10)
                cardNumbersArray[s] = i[s - 5];

            if (s >= 10 && s < 15)
                cardNumbersArray[s] = n[s - 10];

            if (s >= 15 && s < 20)
                cardNumbersArray[s] = g[s - 15];

            if (s >= 20 && s < cardNumbersArray.Length)
                cardNumbersArray[s] = o[s - 20];
        }
    }
    #endregion
}
