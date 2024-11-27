
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class JackpotDisplay : MonoBehaviour
{
    public static JackpotDisplay Instance;

    private int cardNum = 0;
    private Image[] chipImagesArray;

    public void Init()
    {
        Instance = this;

        EventBus.onRoundStarted += ResetDisplay;

        FillImagesArray();
    }

    public void SetCardNum(int _cardNum)
    {
        cardNum = _cardNum;
    }

    public void OpenChip(int _cardNum, int chipNum)
    {
        //4, 3, 2, 1, 0, 6, 12, 16, 20, 21, 22, 23, 24

        if (cardNum == _cardNum)
        {
            int num = -1;

            switch (chipNum)
            {
                case >= 0 and <= 4:
                    num = chipNum;
                    break;

                case 6:
                    num = 5;
                    break;

                case 12:
                    num = 6;
                    break;

                case 16:
                    num = 7;
                    break;

                case >= 20 and <= 24:
                    num = chipNum - 12;
                    break;
            }

            if (num >= 0)
            {
                chipImagesArray[num].enabled = true;
                chipImagesArray[num].DOFade(1, 0.3f).OnComplete(() => chipImagesArray[num].transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.5f, 3));
            }
        }
    }

    public void SetSize(bool isBig)
    {
        if (isBig)
            gameObject.transform.DOScale(1.4f, 0);

        else
            gameObject.transform.DOScale(1, 0);
    }

    private void ResetDisplay(int cardsCount = 0)
    {
        foreach (Image img in chipImagesArray)
        {
            img.enabled = false;
            img.DOFade(0, 0f);
        }
    }

    private void FillImagesArray()
    {
        chipImagesArray = new Image[transform.childCount];

        for (int i = 0; i < chipImagesArray.Length; i++)
        {
            chipImagesArray[i] = transform.GetChild(i).GetComponent<Image>();
        }

        ResetDisplay();
    }

    private void OnDestroy()
    {
        EventBus.onRoundStarted -= ResetDisplay;
    }
}
