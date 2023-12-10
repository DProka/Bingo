
using UnityEngine;

[CreateAssetMenu(fileName = "DataManager", menuName = "ScriptableObject/DataManager")]
public class DataManager : ScriptableObject
{
    public GeneralSave generalSave;
    public PlayerProfile playerProfile;
    public FriendsList friendsListData;
}
