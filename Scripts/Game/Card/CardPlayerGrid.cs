
using UnityEngine;
using System.Collections;

public class CardPlayerGrid : MonoBehaviour
{
    public ChipScript[] chipArray { get; private set; }
    public int[] chipStatusArray { get; private set; }

    private CardController cardController;

    public void Init(CardController card)
    {
        cardController = card;

        chipArray = new ChipScript[25];
        chipStatusArray = new int[chipArray.Length];

        for (int i = 0; i < chipArray.Length; i++)
        {
            chipArray[i] = transform.GetChild(i).GetComponent<ChipScript>();
            chipArray[i].Init(i, cardController);
        }

        ResetChips();
    }

    public IEnumerator OpenChip(int num)
    {
        SetChipClick(num);

        yield return new WaitForSeconds(2f);

        chipArray[num].OpenFrame(true);
    }

    public void ActivateChipFrame(int num) { chipArray[num].OpenFrame(true); }

    public void SetChipClick(int num)
    {
        chipStatusArray[num] = 1;
        chipArray[num].SetChipState(ChipScript.State.Active);
    }

    public void SetChestChip(int num) => chipArray[num].SetChipType(ChipScript.Type.Chest);

    public void SetAutoBingoChip(int num) => chipArray[num].SetChipType(ChipScript.Type.AutoBingo);

    public void SetPuzzleChip(int num)
    {
        chipArray[num].SetChipType(ChipScript.Type.Puzzle);
        UIAnimationScreen.Instance.StartCometAnimation(chipArray[num].transform.position);
    }

    public void ResetChips()
    {
        chipStatusArray = new int[chipArray.Length];

        for (int i = 0; i < chipArray.Length; i++)
        {
            chipArray[i].ResetChip();
        }
    }

    public void CloseChip(int num, bool isTap)
    {
        chipStatusArray[num] = 2;
        chipArray[num].OpenChip(isTap);
    }

    public void SetChestSprite(Sprite usedChest)
    {
        foreach (ChipScript chip in chipArray)
        {
            chip.SetChestSprite(usedChest);
        }
    }

    public void AnimateBingoChips(int[] array)
    {
        float delay = 0f;

        for (int i = 0; i < array.Length; i++)
        {
            if (chipStatusArray[array[i]] != 4)
            {
                chipArray[array[i]].SetChipState(ChipScript.State.Bingo);
                chipArray[array[i]].AnimateChipScale(delay + 0.1f);
                chipStatusArray[array[i]] = 3;

                delay += 0.2f;
            }
        }

        cardController.ActivateBingoScreen(delay);
    }

    public void CheckJackpotChips()
    {
        int[] array = new int[] { 4, 3, 2, 1, 0, 6, 12, 16, 20, 21, 22, 23, 24 };

        float delay = 0f;

        for (int i = 0; i < array.Length; i++)
        {
            chipArray[array[i]].SetChipState(ChipScript.State.JackPot);
            chipArray[array[i]].AnimateChipScale(delay + 0.1f);
            chipStatusArray[array[i]] = 4;

            delay += 0.1f;
        }

        cardController.ActivateBingoScreen(delay);
    }
}
