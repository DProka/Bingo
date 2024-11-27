
using UnityEngine;

public class RoomPrefabScript : MonoBehaviour
{
    [Header("Main Links")]

    private RoomSave roomSave;
    private DesignManager designManager;
    private string roomName;

    [Header("Objects")]

    [SerializeField] Transform parentObject;
    [SerializeField] GameObject clearRoom;

    public int adObjectID { get; private set; }

    public int currentTier;
    public int[] openedObjectsArray;
    public int[] spritesArray;

    private RoomObjectScript[] objectsArray;
    private int[] cashInRoom;

    public void Init(DesignManager manager, int[] pricesArray)
    {
        designManager = manager;

        roomSave = GetComponent<RoomSave>();
        roomSave.Load();

        SetObjectsArray(pricesArray);
        LoadPrefab();
        CheckObjectsStatus();
        CheckTierIsOpen();
    }

    public void UpdateRoomCash(int[] cash)
    {
        if (cashInRoom != null)

            cashInRoom[0] += cash[0];
        cashInRoom[1] += cash[1];

        Debug.Log($"In room earned coins: {cashInRoom[0]}, cash {cashInRoom[1]}");

        SavePrefab();
    }

    public Sprite[] GetShortSpritesByName(string key) { return GetRoomObject(key).GetShortSprites(); }

    public RoomObjectScript[] GetFirstTwoObjects()
    {
        RoomObjectScript[] objArray = new RoomObjectScript[] { objectsArray[0], objectsArray[1] };
        return objArray;
    }

    #region Room

    public void CheckTierIsOpen()
    {
        int count = 0;
        int isOpen = 0;

        foreach (RoomObjectScript obj in objectsArray)
        {
            if (obj._tier == currentTier && !obj._isAD)
            {
                count++;

                if (obj.isOpen)
                    isOpen++;
            }
        }

        if (count == isOpen)
            currentTier += 1;

        foreach (RoomObjectScript obj in objectsArray)
        {
            if (obj._tier == currentTier && openedObjectsArray[obj.id] == 0)
            {
                openedObjectsArray[obj.id] = 1;
                obj.SetObjectStatus(RoomObjectScript.Status.isOpen);
            }
        }

        CheckRoomIsPassed();
    }

    public void CallObjectMenu(RoomObjectScript obj) { designManager.CallObjectMenu(obj, false); }

    public bool CheckObjectsPurchased()
    {
        bool purchased = true;

        for (int i = 0; i < objectsArray.Length; i++)
        {
            if (!objectsArray[i]._isAD)
            {
                if (openedObjectsArray[i] == 0 || openedObjectsArray[i] == 1)
                    purchased = false;
            }
        }

        return purchased;
    }

    private int GetHighestTier()
    {
        int tier = 0;

        for (int i = 0; i < objectsArray.Length; i++)
        {
            if (objectsArray[i]._tier > tier)
            {
                tier = objectsArray[i]._tier;
            }
        }

        return tier;
    }

    private void CheckRoomIsPassed()
    {
        if (CheckObjectsPurchased())
            AppmetricaTimeTracking.reportRoomFinished(roomName, cashInRoom[0], cashInRoom[1]);
    }
    #endregion

    #region Objects

    public RoomObjectScript GetRoomObject(string key)
    {
        RoomObjectScript usedObj = objectsArray[0];

        foreach (RoomObjectScript obj in objectsArray)
        {
            if (obj.gameObject.name == key)
            {
                usedObj = obj;
                break;
            }
        }

        return usedObj;
    }

    public void PurchaseObject(int id, int price)
    {
        if (GameController.Instance.playerMoney >= price)
        {
            openedObjectsArray[id] = 2;
            spritesArray[id] = 1;
            currentTier = objectsArray[id]._tier;
        }

        designManager.BuyObject(objectsArray[id], price);

        CheckClearRoom();
        SavePrefab();
    }

    public void UpdateObjectSprite(string key, int id, int spriteNum)
    {
        objectsArray[id].SetImage(spriteNum - 1);
        spritesArray[id] = spriteNum;
    }

    public void SwitchActiveButtons(bool isVisible)
    {
        foreach (RoomObjectScript obj in objectsArray)
        {
            if (obj._tier == currentTier && !obj.isOpen)
            {
                obj.SwitchBuyButtonActive(isVisible);
            }
        }
    }

    public void SwitchButtonsCanBeClicked(bool canClick)
    {
        foreach (RoomObjectScript obj in objectsArray)
        {
            if (obj._tier == currentTier && !obj.isOpen)
            {
                obj.SwitchButtonCanBeClicked(canClick);
            }
        }
    }

    public int GetActiveObjectSpriteByNum(int num) { return spritesArray[num]; }

    public void SetObjectsPrices(int[] pricesArray)
    {
        for (int i = 0; i < parentObject.childCount; i++)
        {
            objectsArray[i].SetPrice(pricesArray[objectsArray[i]._tier - 1]);
        }
    }

    private void SetObjectsArray(int[] pricesArray)
    {
        objectsArray = new RoomObjectScript[parentObject.childCount];

        for (int i = 0; i < parentObject.childCount; i++)
        {
            objectsArray[i] = parentObject.GetChild(i).GetComponent<RoomObjectScript>();
            objectsArray[i].Init(this, i);
            objectsArray[i].SetPrice(pricesArray[objectsArray[i]._tier - 1]);

            if (objectsArray[i]._isAD)
                adObjectID = i;
        }
    }

    private void CheckClearRoom()
    {
        if (currentTier >= 4)
            clearRoom.SetActive(false);
        else
            clearRoom.SetActive(true);
    }

    private void CheckObjectsStatus()
    {
        foreach (RoomObjectScript obj in objectsArray)
        {
            int objID = obj.id;

            switch (openedObjectsArray[obj.id])
            {
                case 0:
                    obj.SetObjectStatus(RoomObjectScript.Status.isClosed);
                    break;

                case 1:
                    obj.SetObjectStatus(RoomObjectScript.Status.isOpen);
                    if (openedObjectsArray[objID] == 0)
                        openedObjectsArray[objID] = 1;
                    break;

                case 2:
                    obj.SetObjectStatus(RoomObjectScript.Status.isPurchased);
                    int objSpriteNum = spritesArray[objID];
                    if (objSpriteNum == 0)
                    {
                        if (obj._isAD)
                            objSpriteNum = 2;
                        else
                            objSpriteNum = 1;
                    }
                    obj.SetImage(objSpriteNum - 1);
                    break;
            }
        }

        CheckClearRoom();
    }
    #endregion

    #region AD Object

    public RoomObjectScript GetADObject() { return objectsArray[adObjectID]; }

    public void PurchaseADObject()
    {
        objectsArray[adObjectID].SetObjectStatus(RoomObjectScript.Status.isPurchased);
        openedObjectsArray[adObjectID] = 2;
        CheckTierIsOpen();
    }

    #endregion

    #region Save/Load

    public void LoadPrefab()
    {
        roomName = roomSave.GetRoomName();
        currentTier = roomSave.GetTierProgress();

        Debug.Log("RoomSave Array Length: " + roomSave.GetOpenArray().Length);

        if (roomSave.GetOpenArray().Length <= 1)
        {
            openedObjectsArray = new int[objectsArray.Length];
            spritesArray = new int[objectsArray.Length];
        }
        else
        {
            openedObjectsArray = roomSave.GetOpenArray();
            spritesArray = roomSave.GetSpriteArray();
        }
        Debug.Log("RoomHighest Tier: " + GetHighestTier());
        Debug.Log("RoomSave Array Length: " + openedObjectsArray.Length);

        cashInRoom = roomSave.GetCashRoomBonus();
    }

    public void SavePrefab() { roomSave.SaveRoomProgress(currentTier, openedObjectsArray, spritesArray, cashInRoom); }

    public void ResetSave() { roomSave.ResetSave(); }
    #endregion
}
