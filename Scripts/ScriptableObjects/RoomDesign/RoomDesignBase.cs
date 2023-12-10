
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomDesignBase", menuName = "ScriptableObject/RoomDesignBase")]
public class RoomDesignBase : ScriptableObject
{
    [Header("Prices")]

    public int[] priceArray;

    [Header("Images")]

    public Sprite[] FloorImage;
    public Sprite[] WallsImage;
    public Sprite[] CeilingImage;
    public Sprite[] WindowsImage;
    public Sprite[] FireplaceImage;
    public Sprite[] CouchImage;
    public Sprite[] ArmchairImage;
    public Sprite[] TableImage;

    private Dictionary<string, Sprite[]> arrays = new Dictionary<string, Sprite[]>();

    public void Init()
    {
        UpdateDictionary();
    }

    public void UpdateDictionary()
    {
        if(arrays.Count == 0)
        {
            arrays.Add("Floor", FloorImage);
            arrays.Add("Wall", WallsImage);
            arrays.Add("Ceiling", CeilingImage);
            arrays.Add("Window", WindowsImage);
            arrays.Add("Fireplace", FireplaceImage);
            arrays.Add("Couch", CouchImage);
            arrays.Add("Armchair", ArmchairImage);
            arrays.Add("Table", TableImage);
        }
    }

    public Sprite[] GetArrayByKey(string key)
    {
        if (arrays.TryGetValue(key, out Sprite[] array))
        {
            return array;
        }
        else
        {
            return null;
        }
    }
}
