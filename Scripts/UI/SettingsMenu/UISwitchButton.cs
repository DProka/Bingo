using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISwitchButton : MonoBehaviour
{
    [SerializeField] Image buttonImage;
    //[SerializeField] Sprite active;
    //[SerializeField] Sprite notActive;

    //private bool isActive;

    //public void Init()
    //{
    //    isActive = true;
    //}

    //public bool GetStatus() { return isActive; }

    public void SwitchImage(Sprite newSprite)
    {
        buttonImage.sprite = newSprite;
        //if (isActive)
        //{
        //    buttonImage.sprite = notActive;
        //    isActive = false;
        //}
        //else
        //{
        //    buttonImage.sprite = active;
        //    isActive = true;
        //}
    }
}
