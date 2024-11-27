
public class UISave
{
    public static UISave Instance;

    public int capsulesCount { get; private set; }
    public string lastCapsuleTime { get; private set; }
    public string nextCapsuleTime { get; private set; }
    public bool gachaTutorWasShown;

    public int rateUs { get; private set; }

    public int backGroundNum { get; private set; }
    public int lastOpenedBackground { get; private set; }
    public bool backGroundWasShown { get; private set; }

    private const string saveKey = "uiSave";

    public UISave()
    {
        Instance = this;
    }

    public void SaveGacha(int count, string lastTime, string nextTime)
    {
        capsulesCount = count;
        lastCapsuleTime = lastTime;
        nextCapsuleTime = nextTime;

        Save();
    }

    public void SaveBackGround(int num, int lastBackNum, bool wasShown)
    {
        backGroundNum = num;
        lastOpenedBackground = lastBackNum;
        backGroundWasShown = wasShown;

        Save();
    }

    #region Save Load

    public void ResetSave()
    {
        SaveData.UIData general = new SaveData.UIData();

        capsulesCount = general._capsulesCount;
        lastCapsuleTime = general._lastCapsuleTime;
        nextCapsuleTime = general._nextCapsuleTime;
        gachaTutorWasShown = general._gachaTutorWasShown;

        rateUs = general._rateUs;

        backGroundNum = general._backGroundNum;
        lastOpenedBackground = general._lastOpenedBackground;
        backGroundWasShown = general._backGroundWasShown;

        Save();
    }

    public void Save()
    {
        SaveManager.Save(saveKey, GetSaveSnapshot());
    }

    public void Load()
    {
        var data = SaveManager.Load<SaveData.UIData>(saveKey);

        capsulesCount = data._capsulesCount;
        lastCapsuleTime = data._lastCapsuleTime;
        nextCapsuleTime = data._nextCapsuleTime;
        gachaTutorWasShown = data._gachaTutorWasShown;

        rateUs = data._rateUs;

        backGroundNum = data._backGroundNum;
        lastOpenedBackground = data._lastOpenedBackground;
        backGroundWasShown = data._backGroundWasShown;
    }

    private SaveData.UIData GetSaveSnapshot()
    {
        SaveData.UIData data = new SaveData.UIData()
        {
            _capsulesCount = capsulesCount,
            _lastCapsuleTime = lastCapsuleTime,
            _nextCapsuleTime = nextCapsuleTime,
            _gachaTutorWasShown = gachaTutorWasShown,

            _rateUs = rateUs,

            _backGroundNum = backGroundNum,
            _lastOpenedBackground = lastOpenedBackground,
            _backGroundWasShown = backGroundWasShown,
        };

        return data;
    }
    #endregion
}
