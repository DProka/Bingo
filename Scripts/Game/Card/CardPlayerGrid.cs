
using UnityEngine;
using System.Collections;

public class CardPlayerGrid : MonoBehaviour
{
    private CardController cardController;
    private BoosterController boosterController;
    public ChipScript[] chipArray;

    public void Init(CardController card, BoosterController boost)
    {
        cardController = card;
        boosterController = boost;

        chipArray = new ChipScript[25];

        for (int i = 0; i < chipArray.Length; i++)
        {
            chipArray[i] = transform.GetChild(i).GetComponent<ChipScript>();
            chipArray[i].Init(cardController, boosterController, i);
        }

        ResetChips();
    }

    public IEnumerator OpenChip(int num)
    {
        SetChipClick(num);

        yield return new WaitForSeconds(2f);
        
        OpenFrame(num);
    }

    public void SetChipClick(int num) { chipArray[num].SetClick(true); }

    public void SetChestChip(int num)
    {
        chipArray[num].SetChestChip();
    }

    public void ResetChips()
    {
        for (int i = 0; i < chipArray.Length; i++)
        {
            chipArray[i].ResetChip();
        }

        //chipArray[12].OpenOnStart();
    }

    public void CloseChip(int num)
    {
        chipArray[num].OpenChip();
    }

    public Vector3 GetChipPosition(int num)
    {
        return chipArray[num].transform.position;
    }

    public void SetChestSprite(Sprite usedChest)
    {
        foreach(ChipScript chip in chipArray)
        {
            chip.SetChestSprite(usedChest);
        }
    }

    public bool CheckChipIsOpen(int num) { return chipArray[num].CheckIsOpen(); }

    public bool CheckCanBeChest(int num)
    {
        if (!chipArray[num].CheckIsOpen())
        {
            if(!chipArray[num].CheckIsChest())
                return true;
            else
                return false;
        }
        else
            return false;
    }

    public void CheckBingoChips(int num)
    {
        int[] array = GetConditionNumbers(num);
        
        float delay = 0f;

        for (int i = 0; i < array.Length; i++)
        {
            if(chipArray[array[i]].CheckIsOpen())
            {
                chipArray[array[i]].SetBingoChip();
                StartCoroutine(chipArray[array[i]].AnimateChipScale(delay + 0.1f));

                delay += 0.2f;
            }
        }
    }
    
    public void CheckJackpotChips()
    {
        int[] array = new int[] { 4, 3, 2, 1, 0, 6, 12, 16, 20, 21, 22, 23, 24 };

        float delay = 0f;

        for (int i = 0; i < array.Length; i++)
        {
            if(chipArray[array[i]].CheckIsOpen())
            {
                chipArray[array[i]].SetBingoChip();
                StartCoroutine(chipArray[array[i]].AnimateChipScale(delay + 0.1f));

                delay += 0.2f;
            }
        }
    }

    private void OpenFrame(int num) { chipArray[num].OpenFrame(true); }

    private int[] GetConditionNumbers(int num)
    {
        int[] array = new int[5];

        int[] condition13 = new int[] { 0, 4, 12, 20, 24 };

        for (int i = 0; i < array.Length; i++)
        {
            if (num == 0)
                array[i] = i;

            if (num == 1)
                array[i] = i + 5;
            
            if (num == 2)
                array[i] = i + 10;
            
            if (num == 3)
                array[i] = i + 15;
            
            if (num == 4)
                array[i] = i + 20;
            
            if (num == 5)
                array[i] = i * 5;
            
            if (num == 6)
                array[i] = i * 5 + 1;
            
            if (num == 7)
                array[i] = i * 5 + 2;
            
            if (num == 8)
                array[i] = i * 5 + 3;
            
            if (num == 9)
                array[i] = i * 5 + 4;
            
            if (num == 10)
                array[i] = i * 6;
            
            if (num == 11)
                array[i] = (i + 1) * 4;
            
            if (num == 12)
                array = condition13;
        }

        return array;
    }
}
