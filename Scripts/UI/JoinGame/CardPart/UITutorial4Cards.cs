using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UITutorial4Cards : MonoBehaviour
{
    [SerializeField] GameObject mainObject;
    [SerializeField] Image shadeImage;
    [SerializeField] Transform messageTransform;
    [SerializeField] GameObject closeButton;

    private UIJoinGame menu;
    private int tutorialWasShown = 0;

    public void Init()
    {
        mainObject.SetActive(false);
        closeButton.SetActive(false);
        shadeImage.DOFade(0, 0);
        messageTransform.DOScale(0, 0);
        tutorialWasShown = 0;

        //if (!PlayerPrefs.HasKey("4cardsTutorial"))
        //    PlayerPrefs.SetInt("4cardsTutorial", tutorialWasShown);

        if (PlayerPrefs.HasKey("4cardsTutorial"))
            tutorialWasShown = PlayerPrefs.GetInt("4cardsTutorial");
        else
            PlayerPrefs.SetInt("4cardsTutorial", tutorialWasShown);
    }

    public void OpenTutorial()
    {
        //tutorialWasShown = PlayerPrefs.GetInt("4cardsTutorial");

        if (tutorialWasShown == 1)
            return;

        mainObject.SetActive(true);

        shadeImage.DOFade(0.7f, 0.3f).OnComplete(() => 
        {
            messageTransform.DOScale(1, 0.3f).OnComplete(() => closeButton.SetActive(true));
        });
    }

    public void CloseTutorial()
    {
        closeButton.SetActive(false);
        messageTransform.DOScale(0, 0.3f).OnComplete(() =>
        {
            shadeImage.DOFade(0f, 0.3f);
        });

        tutorialWasShown = 1;
        PlayerPrefs.SetInt("4cardsTutorial", tutorialWasShown);
    }
}
