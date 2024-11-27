
public class PlayerSettings
{
    public int voice;
    public int music { get; private set; }
    public int sound { get; private set; }

    public int currentBackgroundNum { get; private set; }

    public int lastBid { get; private set; }
    public int cardsTutorial { get; private set; }

    public int gameLanguage;

    private const string saveKey = "gameSettings";

    #region Edit Settings

    public void SaveSettings(bool _music, bool _sound)
    {
        music = _music ? 1 : 0;
        sound = _sound ? 1 : 0;

        Save();
    }

    public int[] GetSettings()
    {
        return new int[] { music, sound };
    }

    public void SaveBackGround(int num)
    {
        currentBackgroundNum = num;
        Save();
    }

    public void SaveLastBid(int bid, int tutorial)
    {
        lastBid = bid;
        cardsTutorial = tutorial;
        Save();
    }

    #endregion

    #region Save Load

    public void ResetSave()
    {
        SaveData.PlayerSettingsData player = new SaveData.PlayerSettingsData();

        voice = player._voice;
        music = player._music;
        sound = player._sound;

        currentBackgroundNum = player._currentBackgroundNum;

        lastBid = player._lastBid;
        cardsTutorial = player._cardsTutorial;

        gameLanguage = player._gameLanguage;

        Save();
    }

    public void Save()
    {
        SaveManager.Save(saveKey, GetSaveSnapshot());
    }

    public void Load()
    {
        var data = SaveManager.Load<SaveData.PlayerSettingsData>(saveKey);

        voice = data._voice;
        music = data._music;
        sound = data._sound;

        currentBackgroundNum = data._currentBackgroundNum;

        lastBid = data._lastBid;
        cardsTutorial = data._cardsTutorial;

        gameLanguage = data._gameLanguage;
    }

    private SaveData.PlayerSettingsData GetSaveSnapshot()
    {
        SaveData.PlayerSettingsData data = new SaveData.PlayerSettingsData()
        {
            _voice = voice,
            _music = music,
            _sound = sound,

            _currentBackgroundNum = currentBackgroundNum,

            _lastBid = lastBid,
            _cardsTutorial = cardsTutorial,

            _gameLanguage = gameLanguage,
        };

        return data;
    }
    #endregion
}
