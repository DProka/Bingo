using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UILangSwitch : MonoBehaviour
{
    [SerializeField] UISwitchLocalization editLanguage;
    [SerializeField] GameObject switchObj;
    [SerializeField] string key;
    [SerializeField] int languageNum;

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

    public void UpdateEdit() { editLanguage.SwitchLanguage(key, languageNum); }
}
