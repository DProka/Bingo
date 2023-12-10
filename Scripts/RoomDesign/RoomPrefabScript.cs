using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomPrefabScript : MonoBehaviour
{
    [Header("Main Links")]

    private DesignManager designManager;
    [SerializeField] RoomDesignBase roomBase;

    [Header("Objects")]

    [SerializeField] Transform parentObject;

    private RoomObjectScript[] objectsArray;
    private int currentTier;
    private int[] openedObjectsArray;

    public void Init(DesignManager manager, int tier)
    {
        designManager = manager;
        currentTier = tier;

        roomBase.Init();

        SetObjectsArray();
    }

    public void LoadRoomPrefab(int tier, int[] spriteNumbersArray)
    {
        currentTier = tier;

        foreach (RoomObjectScript obj in objectsArray)
        {
            int objTier = obj.GetTier();
            string objKey = obj.GetKey();

            obj.SetImage(roomBase.GetArrayByKey(objKey)[spriteNumbersArray[objTier - 1]]);
            obj.SetPrice(roomBase.priceArray[objTier - 1]);

            if (objTier <= tier)
            {
                obj.SetActive(true);
                obj.SetIsOpen(true);
            }
            else if (objTier == tier + 1)
            {
                obj.SetActive(true);
            }
        }
    }

    public void ActivateTier(int tier)
    {
        currentTier = tier;

        foreach (RoomObjectScript obj in objectsArray)
        {
            if (obj.GetTier() == tier)
                obj.SetActive(true);
        }
    }

    public bool GetTierOpenStatus(int tier)
    {
        int count = 0;
        int isOpen = 0;

        foreach (RoomObjectScript obj in objectsArray)
        {

            if (obj.GetTier() == tier)
            {
                count++;

                if (obj.GetStatus())
                    isOpen++;
            }
        }

        if (count == isOpen)
            return true;
        else
            return false;
    }

    public RoomObjectScript GetRoomObject(string key) 
    {
        RoomObjectScript usedObj = objectsArray[0];

        foreach (RoomObjectScript obj in objectsArray)
        {
            if (obj.GetKey() == key)
            {
                usedObj = obj;
                break;
            }
        }

        return usedObj;
    }

    public void UpdateObjectSprite(string key, int spriteNum, Sprite sprite)
    {
        foreach (RoomObjectScript obj in objectsArray)
        {
            if (obj.GetKey() == key)
            {
                obj.SetImage(sprite);
                designManager.UpdateRoomObjectsArray(obj.GetTier(), spriteNum);
            }
        }
    }

    public void SwitchActiveButtons(bool isVisible)
    {
        foreach (RoomObjectScript obj in objectsArray)
        {
            if (obj.GetTier() == currentTier && !obj.GetStatus())
            {
                obj.SwitchBuyButtonAvtive(isVisible);
            }
        }
    }

    public int GetObjectsCount() { return objectsArray.Length; }

    private void SetObjectsArray()
    {
        objectsArray = new RoomObjectScript[parentObject.childCount];

        for (int i = 0; i < parentObject.childCount; i++)
        {
            objectsArray[i] = parentObject.GetChild(i).GetComponent<RoomObjectScript>();
            objectsArray[i].Init(designManager);
        }

        HideAllObjects();
    }

    private void HideAllObjects()
    {
        foreach (RoomObjectScript obj in objectsArray)
        {
            obj.SetIsOpen(false);
        }
    }
}
