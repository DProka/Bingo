
using UnityEngine;

public class PlayerSettings : MonoBehaviour
{
    public int voice;
    public int music;
    public int sound;

    public int gameLanguage;

    private const string saveKey = "gameSettings";

    #region Edit Settings

    public void SetVoice(bool isOn)
    {
        if (isOn)
            voice = 1;
        else
            voice = 0;

        Save();
    }
    
    public void SetMusic(bool isOn)
    {
        if (isOn)
            music = 1;
        else
            music = 0;

        Save();
    }
    
    public void SetSound(bool isOn)
    {
        if (isOn)
            sound = 1;
        else
            sound = 0;

        Save();
    }
    
    public void SetLanguage(int number)
    {
        gameLanguage = number;

        Save();
    }
    #endregion

    #region Save Load

    public void ResetSave()
    {
        PlayerSettings player = new PlayerSettings();

        voice = player.voice;
        music = player.music;
        sound = player.sound;

        gameLanguage = player.gameLanguage;

        Save();
    }

    public void Save()
    {
        SaveManager.Save(saveKey, GetSaveSnapshot());
    }

    public void Load()
    {
        var data = SaveManager.Load<SaveData.PlayerSettings>(saveKey);

        voice = data._voice;
        music = data._music;
        sound = data._sound;

        gameLanguage = data._gameLanguage;
    }

    private SaveData.PlayerSettings GetSaveSnapshot()
    {
        SaveData.PlayerSettings data = new SaveData.PlayerSettings()
        {
            _voice = voice,
            _music = music,
            _sound = sound,

            _gameLanguage = gameLanguage,
        };

        return data;
    }
    #endregion
}
