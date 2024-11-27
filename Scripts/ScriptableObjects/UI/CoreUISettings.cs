
using UnityEngine;

[CreateAssetMenu(fileName = "CoreUISettings", menuName = "ScriptableObject/UI/CoreUISettings")]
public class CoreUISettings : ScriptableObject
{
    [Header("Loading Screen")]

    public float firstLoadingTime = 3f;
    public float loadindAnimationTime = 3f;

    [Header("Links")]

    public AnimationScreenSettings animationSettings;
    public GachaScreenSettings gachaSettings;
    public BackgroundSettings backGroundSettings;
    public TimeBoosterMenuSettings boosterMenuSettings;
}
