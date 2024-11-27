
using UnityEngine;

public class RoomSave : MonoBehaviour
{
    [SerializeField] string saveKey = "roomSave1";

    public int roomTier;
    public int[] openedObjectsArray;
    public int[] roomSpritesArray;

    public int[] cashBonusInRoom;

    #region Player Credits

    public void SaveRoomProgress(int tier, int[] openArray, int[] spriteArray, int[] cash)
    {
        roomTier = tier;
        openedObjectsArray = new int[openArray.Length];
        roomSpritesArray = new int[spriteArray.Length];

        cashBonusInRoom = cash;

        for (int i = 0; i < openedObjectsArray.Length; i++)
        {
            openedObjectsArray[i] = openArray[i];
            roomSpritesArray[i] = spriteArray[i];
        }

        Save();
    }

    public string GetRoomName() { return saveKey; }
    public int GetTierProgress() { return roomTier; }
    public int[] GetOpenArray() { return openedObjectsArray; }
    public int[] GetSpriteArray() { return roomSpritesArray; }
    public int[] GetCashRoomBonus() { return cashBonusInRoom; }
    #endregion

    #region Save Load

    public void ResetSave()
    {
        SaveData.RoomData general = new SaveData.RoomData();

        roomTier = general._roomTier;
        openedObjectsArray = general._openedObjectsArray;
        roomSpritesArray = general._roomSpritesArray;

        cashBonusInRoom = general._cashBonusInRoom;

        Save();
    }

    public void Save()
    {
        SaveManager.Save(saveKey, GetSaveSnapshot());
    }

    public void Load()
    {
        var data = SaveManager.Load<SaveData.RoomData>(saveKey);

        roomTier = data._roomTier;
        openedObjectsArray = data._openedObjectsArray;
        roomSpritesArray = data._roomSpritesArray;

        cashBonusInRoom = data._cashBonusInRoom;
    }

    private SaveData.RoomData GetSaveSnapshot()
    {
        SaveData.RoomData data = new SaveData.RoomData()
        {
            _roomTier = roomTier,
            _openedObjectsArray = openedObjectsArray,
            _roomSpritesArray = roomSpritesArray,

            _cashBonusInRoom = cashBonusInRoom,
        };

        return data;
    }
    #endregion
}
