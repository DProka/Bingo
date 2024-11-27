
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialStepSettings", menuName = "ScriptableObject/Tutorial/TutorialStepSettings")]
public class TutorialStepSettings : ScriptableObject
{
    [Header("Message Settings")]

    public Vector2 messagePosition;
    public Vector2 messageSize;
    public string messageText;

    [Header("Arrow Settings")]

    public Vector2 arrowPosition;
}
