using System;
using UnityEngine;

public class PlayerProfile : MonoBehaviour
{
    public string playerID;

    [Header("Profile")]

    public string playerNickName;
    public int avatarNumber;
    public int playerCountry;
    public string playerEMail;
    public int playerGender;
    public int[] playerBirthDate;

    [Header("Statistic")]

    public int playedCards;
    public int totalBingos;

    [Header("Game")]

    public int gameLevel;
    public int pointsToLevel;
    public int playerCredits;

    private const string saveKey = "mainSave";

    public void SetPlayerID(string id)
    {
        playerID = id;

        Save();
    }

    #region Player Credits
    public int GetCredits() { return playerCredits; }

    public void AddCredits(int cards)
    {
        playerCredits += cards;

        Save();
    }

    public void SubtractCredits(int price)
    {
        if (playerCredits >= price)
            playerCredits -= price;

        Save();
    }
    #endregion

    #region Player Profile
    
    public void SetPlayersAvatar(int number)
    {
        avatarNumber = number;

        Save();
    }
    
    public void SetPlayersName(string name)
    {
        playerNickName = name;

        Save();
    }

    public void SetPlayersCountry(int number)
    {
        playerCountry = number;

        Save();
    }

    public void SetPlayersEMail(string eMail)
    {
        playerEMail = eMail;

        Save();
    }

    public void SetPlayersGender(int number)
    {
        playerGender = number;

        Save();
    }

    public void SetPlayersBirthday(int[] date)
    {
        playerBirthDate = date;

        Save();
    }
    #endregion

    #region Info

    public void AddPlayedCards()
    {
        playedCards += 1;

        Save();
    }

    public void AddTotalBingos()
    {
        totalBingos += 1;

        Save();
    }

    public void SetPlayersLevel(int points, int level)
    {
        pointsToLevel = points;
        gameLevel = level;

        Save();
    }
    #endregion

    #region Save Load
    public void ResetSave()
    {
        PlayerProfile player = new PlayerProfile();

        playerID = DateTime.Now.ToString("yyyyMMddHHmmss");

        playerNickName = player.playerNickName;
        avatarNumber = player.avatarNumber;
        playerCountry = player.playerCountry;
        playerEMail = player.playerEMail;
        playerGender = player.playerGender;
        playerBirthDate = player.playerBirthDate;

        playedCards = player.playedCards;
        totalBingos = player.totalBingos;

        gameLevel = player.gameLevel;
        pointsToLevel = player.pointsToLevel;

        playerCredits = player.playerCredits;

        Save();
    }

    public void Save()
    {
        SaveManager.Save(saveKey, GetSaveSnapshot());
    }

    public void Load()
    {
        var data = SaveManager.Load<SaveData.PlayerProfile>(saveKey);

        playerID = data._playerID;

        playerNickName = data._playerNickName;
        avatarNumber = data._avatarNumber;
        playerCountry = data._playerCountry;
        playerEMail = data._playerEMail;
        playerGender = data._playerGender;
        playerBirthDate = data._playerBirthDate;

        playedCards = data._playedCards;
        totalBingos = data._totalBingos;

        gameLevel = data._gameLevel;
        pointsToLevel = data._pointsToLevel;

        playerCredits = data._playerCredits;
    }

    private SaveData.PlayerProfile GetSaveSnapshot()
    {
        SaveData.PlayerProfile data = new SaveData.PlayerProfile()
        {
            _playerID = playerID,

            _playerNickName = playerNickName,
            _avatarNumber = avatarNumber,
            _playerCountry = playerCountry,
            _playerEMail = playerEMail,
            _playerGender = playerGender,
            _playerBirthDate = playerBirthDate,

            _playedCards = playedCards,
            _totalBingos = totalBingos,

            _gameLevel = gameLevel,
            _pointsToLevel = pointsToLevel,

            _playerCredits = playerCredits,
        };

        return data;
    }
    #endregion
}



