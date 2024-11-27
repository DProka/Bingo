using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialChooseFloorStep : MonoBehaviour
{
    [SerializeField] GameObject stepObj;
    [SerializeField] TutorialArrow[] floorArrowArray;

    public void SwitchStepActive(bool isActive) { stepObj.SetActive(isActive); }

    public void OpenFloorScreen()
    {
        stepObj.SetActive(true);

        foreach(TutorialArrow arrow in floorArrowArray)
        {
            arrow.SetActive(true);
        }
    }

    public void CloseFloorScreen()
    {
        foreach (TutorialArrow arrow in floorArrowArray)
        {
            arrow.SetActive(false);
        }

        stepObj.SetActive(false);
    }
}
