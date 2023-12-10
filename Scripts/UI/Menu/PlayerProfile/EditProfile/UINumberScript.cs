
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UINumberScript : MonoBehaviour
{
    [SerializeField] UIEditPlayerBirthday editBirthday;
    [SerializeField] TextMeshProUGUI textNumber;
    [SerializeField] Image image;
    [SerializeField] Color notActive;
    [SerializeField] Color active;

    public bool isDay;

    private int number;

    public void Init(UIEditPlayerBirthday edit, int _number)
    {
        editBirthday = edit;
        number = _number;
        SetText(_number);
        image.color = notActive;
    }

    public UINumberScript GetScript() { return this; } 

    public void SetDayBool(bool _isDay) { isDay = _isDay; }

    public void SetColorActive(bool isActive) 
    {
        if (isActive)
            image.color = active;
        else
            image.color = notActive;
                
    }

    public void SetText(int number)
    {
        textNumber.text = $"{number}";
    }

    public void OnMouseDown()
    {
        if (isDay)
        {
            editBirthday.SetDay(number);
        }
        else
        {
            editBirthday.SetMonth(number);
        }
    }
}
