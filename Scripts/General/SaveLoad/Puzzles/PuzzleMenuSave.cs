
public class PuzzleMenuSave
{
    public int nextNum { get; private set; }
    public int newPuzzleCount { get; private set; }
    public int[] puzzlesOrderArray { get; private set; }
    public int[] partsOrderArray { get; private set; }
    public int[] partsStatusArray { get; private set; }
    public int buttonIsActive { get; private set; }
    public int tutorComplete { get; private set; }
    
    private string saveKey = "puzzleMenuSave";

    public PuzzleMenuSave() { }
    
    public void SavePuzzle(int[] puzzles, int[] parts)
    {
        puzzlesOrderArray = puzzles;
        partsOrderArray = parts;

        Save();
    }

    public void SaveParts(int _nextNum, int _newPuzzles, int[] partsStatus)
    {
        nextNum = _nextNum;
        newPuzzleCount = _newPuzzles;
        partsStatusArray = partsStatus;

        Save();
    }

    public void UpdateButtonStatus()
    {
        if (buttonIsActive == 0)
            buttonIsActive = 1;

        Save();
    }
    
    public void UpdateTutorStatus()
    {
        if (tutorComplete == 0)
            tutorComplete = 1;

        Save();
    }

    #region Save Load

    public void ResetSave()
    {
        SaveData.PuzzleMenuData general = new SaveData.PuzzleMenuData();

        nextNum = general._nextNum;
        newPuzzleCount = general._newPuzzleCount;
        puzzlesOrderArray = general._puzzlesOrderArray;
        partsOrderArray = general._partsOrderArray;
        partsStatusArray = general._partsStatusArray;
        buttonIsActive = general._buttonIsActive;
        tutorComplete = general._tutorComplete;

        Save();
    }

    public void Load()
    {
        var data = SaveManager.Load<SaveData.PuzzleMenuData>(saveKey);

        nextNum = data._nextNum;
        newPuzzleCount = data._newPuzzleCount;
        puzzlesOrderArray = data._puzzlesOrderArray;
        partsOrderArray = data._partsOrderArray;
        partsStatusArray = data._partsStatusArray;
        buttonIsActive = data._buttonIsActive;
        tutorComplete = data._tutorComplete;
    }

    private void Save()
    {
        SaveManager.Save(saveKey, GetSaveSnapshot());
    }

    private SaveData.PuzzleMenuData GetSaveSnapshot()
    {
        SaveData.PuzzleMenuData data = new SaveData.PuzzleMenuData()
        {
            _nextNum = nextNum,
            _newPuzzleCount = newPuzzleCount,
            _puzzlesOrderArray = puzzlesOrderArray,
            _partsOrderArray = partsOrderArray,
            _partsStatusArray = partsStatusArray,
            _buttonIsActive = buttonIsActive,
            _tutorComplete = tutorComplete,
        };

        return data;
    }
    #endregion
}
