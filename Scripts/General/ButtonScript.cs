using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    public int clickSound;

    void Start()
    {
        Button thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(() => ClickSound());
    }

    public void ClickSound() 
    {
        switch (clickSound)
        {
            case 0:
                SoundController.Instance.PlaySound(SoundController.Sound.ButtonClick1);
                break;
        
            case 1:
                SoundController.Instance.PlaySound(SoundController.Sound.ButtonClick2);
                break;
        }
    }
}
