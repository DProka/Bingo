
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;
using System.Linq;

public class PuzzlePrefabScript : MonoBehaviour
{
    public int[] openedPartsArray { get; private set; }
    public bool isComplete { get; private set; }

    [SerializeField] Image backImage;
    [SerializeField] Image backMainImage;
    [SerializeField] Image girdImage;
    [SerializeField] Transform partParent;
    [SerializeField] SkeletonGraphic frameAnimation;

    private PuzzleSave puzzleSave;
    private int puzzleID;
    private Image[] partImagesArray;

    public void Init(PuzzlePrefabSettings settings, int id)
    {
        puzzleID = id;

        backMainImage.sprite = settings.mainSprite;

        puzzleSave = new PuzzleSave(puzzleID.ToString());
        puzzleSave.Load();
        openedPartsArray = puzzleSave.partsArray;

        SetPartsArray(settings);
    }

    public void SetPartsArray(PuzzlePrefabSettings settings)
    {
        partImagesArray = new Image[partParent.childCount];

        if (openedPartsArray.Length == 0)
            openedPartsArray = new int[partImagesArray.Length];

        for (int i = 0; i < partParent.childCount; i++)
        {
            partImagesArray[i] = partParent.GetChild(i).GetComponent<Image>();
            partImagesArray[i].sprite = settings.partSpritesArray[i];
            partImagesArray[i].enabled = false;
        }

        CheckPartsActive();
        CheckCompleteAnimation();
    }

    public Vector2 GetPartPositionByNum(int num)
    {
        Vector2 pos = new Vector2(-10, -10);

        if (partImagesArray[num].transform.position.x > -4.7 && partImagesArray[num].transform.position.x < 4.7)
            pos = partImagesArray[num].transform.position;

        return pos;
    }

    public void OpenPartByNum(int num)
    {
        partImagesArray[num].enabled = true;
        partImagesArray[num].DOFade(1f, 0.3f);
        openedPartsArray[num] = 1;
        CheckCompleteAnimation();
        puzzleSave.SavePuzzle(openedPartsArray);

        if (isComplete)
            foreach (Image img in partImagesArray)
                img.enabled = false;
    }

    public void CheckActivity()
    {
        bool isActive;

        if (transform.position.x > -7 && transform.position.x < 7)
            isActive = true;
        else
            isActive = false;

        backImage.enabled = isActive;
        backMainImage.enabled = isActive;
        girdImage.enabled = isActive;
        frameAnimation.enabled = isActive;

        CheckPartsActive();
    }

    public void ResetSave()
    {
        puzzleSave.ResetSave();
        puzzleSave.Load();
        openedPartsArray = puzzleSave.partsArray;
    }

    public void CheckPuzzleComplete() => isComplete = !openedPartsArray.Contains(0);
    
    private void CheckCompleteAnimation()
    {
        CheckPuzzleComplete();
        frameAnimation.AnimationState.TimeScale = 1f;
        frameAnimation.AnimationState.SetAnimation(0, isComplete ? "AllPuzzle_AllColors" : "New_Puzzle", false);
        CheckPartsActive();
    }

    private void CheckPartsActive()
    {
        if (openedPartsArray.Contains(0))
        {
            for (int i = 0; i < openedPartsArray.Length; i++)
            {
                partImagesArray[i].enabled = openedPartsArray[i] == 1 ? true : false;
            }
        }
        else
        {
            foreach (Image img in partImagesArray)
                img.enabled = false;

            backMainImage.color = new Color(255, 255, 255, 255);
        }
    }
}
