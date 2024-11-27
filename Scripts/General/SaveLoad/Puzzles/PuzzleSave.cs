
using UnityEngine;

public class PuzzleSave
{
    public int[] partsArray { get; private set; }

    private string saveKey = "puzzleSave";

    public PuzzleSave(string key)
    {
        saveKey = "puzzleSave" + key;
    }

    public void SavePuzzle(int[] array)
    {
        partsArray = array;
        Debug.Log("PuzzleSaved by Key: " + saveKey);
        Save();
    }

    #region Save Load

    public void ResetSave()
    {
        SaveData.PuzzleData general = new SaveData.PuzzleData();

        partsArray = general._partsArray;

        Save();
    }

    public void Load()
    {
        var data = SaveManager.Load<SaveData.PuzzleData>(saveKey);

        partsArray = data._partsArray;
    }

    private void Save()
    {
        SaveManager.Save(saveKey, GetSaveSnapshot());
    }

    private SaveData.PuzzleData GetSaveSnapshot()
    {
        SaveData.PuzzleData data = new SaveData.PuzzleData()
        {
            _partsArray = partsArray,
        };

        return data;
    }
    #endregion
}
