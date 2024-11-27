
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIPuzzleMenuPuzzlesPart : MonoBehaviour
{
    public int lastPuzzleNum { get; private set; }

    [SerializeField] Transform puzzleParent;
    [SerializeField] ScrollRect scroll;

    private PuzzleMenuSettings menuSettings;

    private PuzzlePrefabScript[] puzzlePrefabArray;
    private RectTransform puzzleParentTransform;

    private bool puzzleParentInMove;

    public void Init(PuzzleMenuSettings _menuSettings)
    {
        puzzleParentTransform = puzzleParent.GetComponent<RectTransform>();
        menuSettings = _menuSettings;

        puzzleParentInMove = false;
        lastPuzzleNum = 1;
    }

    public void UpdateScript()
    {
        if (scroll.velocity.magnitude > 0.1f || puzzleParentInMove)
            CheckPuzzlesActivity();
    }

    public void SwitchPuzzleByButton(int num)
    {
        lastPuzzleNum += num;

        if (lastPuzzleNum < 1)
            lastPuzzleNum = 1;
        else if (lastPuzzleNum > puzzlePrefabArray.Length - 2)
            lastPuzzleNum = puzzlePrefabArray.Length - 2;

        MovePuzzleParentInNewPosBuButton(lastPuzzleNum);
    }

    public void SetPuzzleArray()
    {
        puzzlePrefabArray = new PuzzlePrefabScript[menuSettings.puzzlesArray.Length];

        GridLayoutGroup puzzleGrid = puzzleParent.GetComponent<GridLayoutGroup>();
        puzzleGrid.cellSize = menuSettings.puzzleCellSize;
        puzzleGrid.spacing = menuSettings.puzzleSpacing;

        for (int i = 0; i < puzzlePrefabArray.Length; i++)
        {
            puzzlePrefabArray[i] = Instantiate(menuSettings.puzzlePrefab, puzzleParent);
            puzzlePrefabArray[i].Init(menuSettings.puzzlesArray[i], i);
        }

        float cellWidth = menuSettings.puzzleCellSize.x * puzzlePrefabArray.Length;
        float spacing = menuSettings.puzzleSpacing.x * (puzzlePrefabArray.Length - 1);
        float contentWidht = cellWidth + spacing;
        puzzleParentTransform.sizeDelta = new Vector2(contentWidht, puzzleParentTransform.sizeDelta.y);
    }

    public void MovePuzzleParentInNewPos(int num)
    {
        int puzzleNum = num;

        if (num > 2)
        {
            puzzleParentInMove = true;
            float newX = puzzleParent.position.x - puzzlePrefabArray[puzzleNum].transform.position.x;
            puzzleParent.DOMove(new Vector2(newX, puzzleParent.position.y), 0.3f).OnComplete(() => puzzleParentInMove = false);
            lastPuzzleNum = puzzleNum;
        }
        else
        {
            CheckPuzzlesActivity();
        }
    }

    public void SwitchScroll(bool isActive) => scroll.enabled = isActive;

    public void ResetPuzzles()
    {
        for (int i = 0; i < puzzlePrefabArray.Length; i++)
        {
            Destroy(puzzlePrefabArray[i].gameObject);
        }
    }

    public Vector2 GetPartPosByNum(int puzzleNum, int partNum) { return puzzlePrefabArray[puzzleNum].GetPartPositionByNum(partNum); }

    public void OpenPartByNum(int puzzleNum, int partNum) => puzzlePrefabArray[puzzleNum].OpenPartByNum(partNum);

    public bool CheckPuzzleCompleteByNum(int puzzleNum) => puzzlePrefabArray[puzzleNum].isComplete;

    private void MovePuzzleParentInNewPosBuButton(int num)
    {
        puzzleParentInMove = true;
        float newX = puzzleParent.position.x - puzzlePrefabArray[num].transform.position.x;
        puzzleParent.DOMove(new Vector2(newX, puzzleParent.position.y), 0.3f).OnComplete(() => puzzleParentInMove = false);
    }

    private void CheckPuzzlesActivity()
    {
        foreach (PuzzlePrefabScript puzzle in puzzlePrefabArray)
            puzzle.CheckActivity();
    }

}
