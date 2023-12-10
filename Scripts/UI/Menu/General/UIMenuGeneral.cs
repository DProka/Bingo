using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIMenuGeneral : MonoBehaviour
{
    [SerializeField] GameObject menuObj;
    [SerializeField] GameObject backObj;
    [SerializeField] GameObject shadeObj;

    public virtual void OpenMain()
    {
        menuObj.SetActive(true);

        if (shadeObj != null)
            shadeObj.SetActive(true);
        
        if(backObj != null)
        {
            backObj.transform.localScale = new Vector3(0.1f, 0.1f, 0);
            backObj.transform.DOScale(1, 0.2f);
        }
    }

    public virtual void CloseMain()
    {
        menuObj.SetActive(false);
    }

    public void SwitchShade(bool isOn) { shadeObj.SetActive(isOn); }
}
