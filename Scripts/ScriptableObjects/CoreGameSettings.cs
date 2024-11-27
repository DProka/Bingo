
using UnityEngine;

[CreateAssetMenu(fileName = "CoreGameSettings", menuName = "ScriptableObject/Game/CoreGameSettings")]
public class CoreGameSettings : ScriptableObject
{
    public PlayerStatsSettings statsSettings;
    public TableSettings tableSettings;
    public CoreUISettings uiSettings;

    [Header("Advertisment Settings")]

    public string appID;

}
