
public class BoosterSave
{
    public int ballsPlus5Count;
    public int doubleProgressCount;
    public int autoBingoCount;

    public int autoDoubCount;
    public int tripleDoubCount;
    public int airplaneCount;
    public int doubleMoneyCount;

    public string hintTime;
    public string batteryTime;
    public string autoBonusTime;
    public string wildDoubTime;
    public string doubleXpTime;

    private string saveKey = "BoosterSave";

    public BoosterSave()
    {
        Load();
    }

    public void SaveMenuBoosters(int[] save)
    {
        ballsPlus5Count = save[0];
        doubleProgressCount = save[1];
        autoBingoCount = save[2];

        Save();
    }

    public void SaveCountBoosters(int[] save)
    {
        autoDoubCount = save[0];
        tripleDoubCount = save[1];
        airplaneCount = save[2];
        doubleMoneyCount = save[3];

        Save();
    }

    public void SaveTimeBoosters(string[] save)
    {
        hintTime = save[0];
        batteryTime = save[1];
        autoBonusTime = save[2];
        wildDoubTime = save[3];
        doubleXpTime = save[4];

        Save();
    }

    #region Save Load

    public void ResetSave()
    {
        SaveData.BoosterData general = new SaveData.BoosterData();

        ballsPlus5Count = general._ballsPlus5Count;
        doubleProgressCount = general._doubleProgressCount;
        autoBingoCount = general._autoBingoCount;

        autoDoubCount = general._autoDoubCount;
        tripleDoubCount = general._tripleDoubCount;
        airplaneCount = general._airplaneCount;

        hintTime = general._hintTime;
        batteryTime = general._batteryTime;
        autoBonusTime = general._autoBonusTime;
        wildDoubTime = general._wildDoubTime;
        doubleXpTime = general._doubleXpTime;
        doubleMoneyCount = general._doubleMoneyCount;

        Save();
    }

    public void Save()
    {
        SaveManager.Save(saveKey, GetSaveSnapshot());
    }

    public void Load()
    {
        var data = SaveManager.Load<SaveData.BoosterData>(saveKey);

        ballsPlus5Count = data._ballsPlus5Count;
        doubleProgressCount = data._doubleProgressCount;
        autoBingoCount = data._autoBingoCount;

        autoDoubCount = data._autoDoubCount;
        tripleDoubCount = data._tripleDoubCount;
        airplaneCount = data._airplaneCount;

        hintTime = data._hintTime;
        batteryTime = data._batteryTime;
        autoBonusTime = data._autoBonusTime;
        wildDoubTime = data._wildDoubTime;
        doubleXpTime = data._doubleXpTime;
        doubleMoneyCount = data._doubleMoneyCount;
    }

    private SaveData.BoosterData GetSaveSnapshot()
    {
        SaveData.BoosterData data = new SaveData.BoosterData()
        {
            _ballsPlus5Count = ballsPlus5Count,
            _doubleProgressCount = doubleProgressCount,
            _autoBingoCount = autoBingoCount,

            _autoDoubCount = autoDoubCount,
            _tripleDoubCount = tripleDoubCount,
            _airplaneCount = airplaneCount,

            _hintTime = hintTime,
            _batteryTime = batteryTime,
            _autoBonusTime = autoBonusTime,
            _wildDoubTime = wildDoubTime,
            _doubleXpTime = doubleXpTime,
            _doubleMoneyCount = doubleMoneyCount,
        };

        return data;
    }
    #endregion
}
