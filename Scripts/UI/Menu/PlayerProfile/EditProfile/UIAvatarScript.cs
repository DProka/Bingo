
using UnityEngine;
using UnityEngine.UI;

public class UIAvatarScript : MonoBehaviour
{
    [SerializeField] UIEditAvatar editAvatar;

    [SerializeField] GameObject activeFrame;
    [SerializeField] Image photo;
    [SerializeField] Image frame;

    public bool isActive;

    private int avatarNumber;

    public void OnMouseDown()
    {
        if(isActive == true)
            SetAvatarActive();
    }

    public void SetManager(UIEditAvatar manager) { editAvatar = manager; }

    public void SetAvatarActive() 
    { 
        editAvatar.ChangeAvatar(avatarNumber); 
    }
    
    public void SetPhoto(Sprite image) { photo.sprite = image; }

    public void SetNumber(int number) { avatarNumber = number; }

    public void SetActiveFrame(bool isOn) { activeFrame.SetActive(isOn); }
    
}
