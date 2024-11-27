
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PuzzleGroupPrefab : MonoBehaviour
{
    [SerializeField] Image mainImage; 
    [SerializeField] Image frameImage;
    [SerializeField] TextMeshProUGUI[] nameArray;

    private Sprite[] mainArray;
    private Sprite[] frameArray;
    private bool buttonActive;

    public void Init(string name, Sprite[] main, Sprite[] frame)
    {
        foreach (TextMeshProUGUI text in nameArray)
            text.text = name;

        mainArray = main;
        frameArray = frame;
    }

    public void SwitchAvtive(bool isActive)
    {
        nameArray[0].enabled = isActive ? true : false;
        nameArray[1].enabled = isActive ? false : true;
        nameArray[2].enabled = isActive ? false : true;

        mainImage.sprite = isActive ? mainArray[0] : mainArray[1];
        frameImage.sprite = isActive ? frameArray[0] : frameArray[1];
        buttonActive = isActive;
    }

    public void GoToPuzzles()
    {
        if(buttonActive)
            UIController.Instance.CallScreen(UIController.Menu.PuzzleMenu1);
    }
}
