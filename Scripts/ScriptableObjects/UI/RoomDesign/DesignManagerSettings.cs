using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DesignManagerSettings", menuName = "ScriptableObject/Room/DesignManagerSettings")]
public class DesignManagerSettings : MonoBehaviour
{
    [SerializeField] float zoomSpeed = 0.4f;
    [SerializeField] RoomPrefabScript[] roomPrefabArray;
    [SerializeField] RoomPrefabSettings[] roomSettingsArray;

}
