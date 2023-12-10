
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIFlagScript : MonoBehaviour
{
    [SerializeField] UIEditPlayerCountry editCountry;

    [SerializeField] GameObject activeFrame;
    [SerializeField] Image flagImage;
    [SerializeField] TextMeshProUGUI countryName;


    public bool isActive;

    private int countryNumber;

    public void OnMouseDown()
    {
        if (isActive == true)
            SetAvatarActive();
    }

    public void SetManager(UIEditPlayerCountry manager) { editCountry = manager; }

    public void SetAvatarActive()
    {
        editCountry.ChangeCountry(countryNumber);
    }

    public void SetActiveFrame(bool isActive) { activeFrame.SetActive(isActive); }

    public void SetFlag(Sprite image) 
    { 
        flagImage.sprite = image;
        countryName.text = image.name;
    }

    public void SetNumber(int number) { countryNumber = number; }
}
