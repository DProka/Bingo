using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSave : MonoBehaviour
{
    public int currentRoom;
    public int[] roomProgressArray;
    public int[] room1SpritesArray;

    private const string saveKey = "roomSave";

    #region Player Credits

    public void SetActiveRoom(int roomNum) 
    {
        currentRoom = roomNum;

        Save();
    }

    public int GetActiveRoom() { return currentRoom; }

    public void SetTierProgress(int num) 
    { 
        roomProgressArray[currentRoom] = num;
        
        Save();
    }

    public int GetTierProgress() { return roomProgressArray[currentRoom]; }

    public void SetRoomProgress(int roomNum, int[] roomProgress)
    {
        if(roomNum == 0)
            room1SpritesArray = roomProgress;

        Save();
    }

    public int[] GetRoomProgress(int roomNum)
    {
        if (roomNum == 0)
            return room1SpritesArray;
        else
            return null;
    }

    #endregion

    #region Save Load

    public void ResetSave()
    {
        SaveData.RoomData general = new SaveData.RoomData();

        currentRoom = general._activeRoom;
        room1SpritesArray = general._room1SpritesArray;
        roomProgressArray = general._roomProgressArray;

        Save();
    }

    public void Save()
    {
        SaveManager.Save(saveKey, GetSaveSnapshot());
    }

    public void Load()
    {
        var data = SaveManager.Load<SaveData.RoomData>(saveKey);

        currentRoom = data._activeRoom;
        roomProgressArray = data._roomProgressArray;
        room1SpritesArray = data._room1SpritesArray;
    }

    private SaveData.RoomData GetSaveSnapshot()
    {
        SaveData.RoomData data = new SaveData.RoomData()
        {
            _activeRoom = currentRoom,
            _roomProgressArray = roomProgressArray,
            _room1SpritesArray = room1SpritesArray,
        };

        return data;
    }
    #endregion
}
