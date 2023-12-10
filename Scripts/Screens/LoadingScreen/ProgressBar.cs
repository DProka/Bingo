
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] Transform progressBar;
    [SerializeField] Image progressImg;
    [SerializeField] TextMeshProUGUI progressText;

    public void ResetProgress()
    {
        //progressBar.localScale = new Vector3(0, progressBar.localScale.y, 0);
        progressImg.fillAmount = 0;
    }

    public void IncreaseScaleOnStep(float step, float time)
    {
        if (progressImg.fillAmount < 0.98f)
            progressImg.DOFillAmount(progressImg.fillAmount + step, time * 0.8F);
        else
            progressImg.fillAmount = 1F;
        
        //if (progressBar.localScale.x < 0.98f)
        //    progressBar.DOScaleX(progressBar.localScale.x + step, time * 0.8f);
        //else
        //    progressBar.localScale = new Vector3(1, progressBar.localScale.y, 0);
    }

    public bool GetBarFullness()
    {
        bool isFull;
        
        if (progressImg.fillAmount < 0.98)// progressBar.localScale.x < 0.98f)
            isFull = false;
        else
            isFull = true;

        return isFull;
    }

    public void UpdateText()
    {
        int proc = (int)(progressImg.fillAmount * 100);
        //int proc = (int)(progressBar.localScale.x * 100);
        
        progressText.text = $"{proc} %";
    }
}
