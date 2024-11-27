
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIPuzzleMenuPartsPart : MonoBehaviour
{
    public PuzzlePartPrefabScript[] partPrefabArray { get; private set; }
    public int[] partsOrderArray { get; private set; }
    public int[] partsStatusArray { get; private set; }

    [SerializeField] RectTransform partParentTransform;
    [SerializeField] Transform partParent;
    [SerializeField] Image partImagePrefab;
    [SerializeField] ScrollRect scroll;

    private PuzzleMenuSettings menuSettings;
    
    private bool partParentInMove;

    public void Init(PuzzleMenuSettings _menuSettings)
    {
        menuSettings = _menuSettings;
    }

    public void UpdateScript()
    {
        if (scroll.velocity.magnitude > 0.1f || partParentInMove)
            CheckPartsActivity();
    }

    public PuzzlePartPrefabScript GetPartByPosition(Vector2 pos)
    {
        PuzzlePartPrefabScript part = null;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(pos);

        for (int i = 0; i < partPrefabArray.Length; i++)
        {
            if (partPrefabArray[i].CheckAvailability())
            {
                Vector2 partPos = partPrefabArray[i].GetPartPosition();
                if (mousePos.x > partPos.x - 0.7 && mousePos.x < partPos.x + 0.7
                    && mousePos.y > partPos.y - 0.7 && mousePos.y < partPos.y + 0.7)
                {
                    part = partPrefabArray[i];
                    part.SwitchPartImage(false);
                }
            }
        }

        return part;
    }

    public void SwitchScroll(bool isActive) => scroll.enabled = isActive;

    public void OpenNewPartByNum(int num)
    {
        partsStatusArray[num] = 1;
        partPrefabArray[num].OpenPart();
    }

    public void SetPartStatus(int partNum, int status) => partsStatusArray[partNum] = status;

    public void MovePartsParentInNewPos(int partNum)
    {
        partParentInMove = true;
        float newX = partParent.position.x - partPrefabArray[partNum].transform.position.x;
        partParent.DOMove(new Vector2(newX, partParent.position.y), 0.3f).OnComplete(() => partParentInMove = false);
    }

    public void SetPartsOrder(int[] order) { partsOrderArray = order; }

    public void SetPartsStatusArray(int[] array, int nextNum) 
    { 
        partsStatusArray = array;

        for (int i = 0; i < partsStatusArray.Length; i++)
        {
            if (i >= nextNum)
                partsStatusArray[i] = 0;
        }
    }

    public void ResetParts()
    {
        for (int i = 0; i < partPrefabArray.Length; i++)
        {
            Destroy(partPrefabArray[i].gameObject);
        }

        SetPartArray();
    }

    public void GetClosedPartReward(Vector2 partPos)
    {
        int reward = Random.Range(1, 6);

        UIAnimationScreen.Instance.StartBonusItemAnimation(reward, partPos);
        GameController.Instance.CalculateCrystals(reward);
    }

    public void SetPartArray()
    {
        partPrefabArray = new PuzzlePartPrefabScript[menuSettings.puzzlesArray.Length * 9];

        if (partsOrderArray.Length == 0)
        {
            int[] partsOrderArray1 = CreateAndShuffleArray(0, (3 * 9) - 1);
            int[] partsOrderArray2 = CreateAndShuffleArray(3 * 9, partPrefabArray.Length - 1);
            partsOrderArray = partsOrderArray1.Concat(partsOrderArray2).ToArray();
            SetPartAtFirstPlace(3);
        }
        else if (partsOrderArray.Length < partPrefabArray.Length - 1)
        {
            int[] partsOrderArray2 = CreateAndShuffleArray(partsOrderArray.Length, partPrefabArray.Length - 1);
            partsOrderArray = partsOrderArray.Concat(partsOrderArray2).ToArray();
        }

        if (partsStatusArray == null || partsStatusArray.Length == 0)
            partsStatusArray = new int[partsOrderArray.Length];
        else if (partsStatusArray.Length < partPrefabArray.Length)
        {
            int[] newParts = new int[partPrefabArray.Length - partsStatusArray.Length];
            partsStatusArray = partsStatusArray.Concat(newParts).ToArray();
        }

        GridLayoutGroup partGrid = partParent.GetComponent<GridLayoutGroup>();
        partGrid.cellSize = menuSettings.partCellSize;
        partGrid.spacing = menuSettings.partSpacing;

        int partNum = 0;

        for (int i = 0; i < menuSettings.puzzlesArray.Length; i++)
        {
            for (int j = 0; j < menuSettings.puzzlesArray[i].partSpritesArray.Length; j++)
            {
                partPrefabArray[partNum] = Instantiate(menuSettings.partPrefab, partParent);
                partPrefabArray[partNum].Init(new int[] { partsOrderArray[partNum], i, j }, menuSettings);

                switch (partsStatusArray[partNum])
                {
                    case 0:
                        partPrefabArray[partNum].SetStatus(PuzzlePartPrefabScript.Status.Closed);
                        break;

                    case 1:
                        partPrefabArray[partNum].SetStatus(PuzzlePartPrefabScript.Status.Open);
                        break;

                    case 2:
                        partPrefabArray[partNum].SetStatus(PuzzlePartPrefabScript.Status.Recieved);
                        break;
                }

                partNum++;
            }
        }

        float cellWidth = menuSettings.partCellSize.x * partPrefabArray.Length;
        float spacing = menuSettings.partSpacing.x * (partPrefabArray.Length - 1);
        float contentWidht = cellWidth + spacing;
        partParentTransform.sizeDelta = new Vector2(contentWidht, partParentTransform.sizeDelta.y);

        Invoke("CheckPartsActivity", 0.5f);
    }

    private int[] CreateAndShuffleArray(int first, int second)
    {
        int[] array = Enumerable.Range(first, second - first + 1).ToArray();

        System.Random rand = new System.Random();
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = rand.Next(0, i + 1);
            int temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
        return array;
    }

    private void CheckPartsActivity()
    {
        foreach (PuzzlePartPrefabScript part in partPrefabArray)
            part.CheckActivity();
    }

    private void SetPartAtFirstPlace(int num)
    {
        int index = System.Array.IndexOf(partsOrderArray, num);

        if (index > 0)
        {
            int temp = partsOrderArray[0];
            partsOrderArray[0] = partsOrderArray[index];
            partsOrderArray[index] = temp;
        }
    }

}
