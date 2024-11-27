
using UnityEngine;
using UnityEngine.UI;

public class UISwitchVoice : MonoBehaviour
{
    [SerializeField] Image buttonImage;
    [SerializeField] Sprite active;
    [SerializeField] Sprite notActive;

    private bool isActive;

    public void Init()
    {
        SwitchOn();
    }

    public bool GetStatus() { return isActive; }

    public void SwitchOn() 
    {
        isActive = true;
        buttonImage.sprite = active;
    }

    public void SwitchOff() 
    {
        isActive = false;
        buttonImage.sprite = notActive;
    }

}
