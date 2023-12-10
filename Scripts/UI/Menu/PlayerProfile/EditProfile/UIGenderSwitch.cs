
using UnityEngine;
using TMPro;

public class UIGenderSwitch : MonoBehaviour
{
    [SerializeField] UIEditPlayerGender editGenger;
    [SerializeField] GameObject switchObj;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] int switchNumber;

    public void Init()
    {
        SwitchOff();
    }

    public void OnMouseDown()
    {
        UpdateEdit();
    }

    public void SwitchON() { switchObj.SetActive(true); }
    
    public void SwitchOff() { switchObj.SetActive(false); }

    public void UpdateEdit() { editGenger.SetGenderNumber(switchNumber); }
    
    public string GetName() { return text.text; }
}
