using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomPrefabSettings", menuName = "ScriptableObject/Room/RoomPrefabSettings")]
public class RoomPrefabSettings : ScriptableObject
{
    [SerializeField] int[] objectPricesArray;

    public int[] GetPricesArray() { return objectPricesArray; }

    public int GetObjectPriceByNum(int num)
    {
        int price = objectPricesArray[num < objectPricesArray.Length ? num : objectPricesArray.Length - 1];
        return price;
    }
}
