using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class FirstLoadingScreen : MonoBehaviour
{
    [SerializeField] GameObject window;
    [SerializeField] GameObject nameImage;

    [Header("Progress Bar")]

    [SerializeField] ProgressBar progressBar;
    [SerializeField] float progressTime;

    IEnumerator AnimateWindow()
    {
        yield return new WaitForSeconds(1f);

        Vector3 scale = new Vector3(0.05f, 0.05f, 0);
        nameImage.transform.DOPunchScale(scale, 0.5f, 1);
        
        StartCoroutine(AnimateWindow());
    }

    IEnumerator ActivateProgress()
    {
        float step = 1 / progressTime;

        progressBar.IncreaseScaleOnStep(step, 1);

        yield return new WaitForSeconds(1f);

        if (!progressBar.GetBarFullness())
        {
            StartCoroutine(ActivateProgress());
        }
    }

    public void UpdateText()
    {
        progressBar.UpdateText();
    }

    public void OpenWindow() 
    {
        window.SetActive(true);
        progressBar.ResetProgress();
        StartCoroutine(AnimateWindow());
        StartCoroutine(ActivateProgress());
    }

    public void CloseWindow() 
    {
        window.SetActive(false);
        StopCoroutine(AnimateWindow());
    }
}
