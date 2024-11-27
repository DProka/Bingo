
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] Image progressImg;
    [SerializeField] TextMeshProUGUI progressText;

    public void ResetProgress()
    {
        progressImg.fillAmount = 0;
    }

    public void FillBarByStep(float step, float time)
    {
        if (progressImg.fillAmount < 0.98f)
            progressImg.DOFillAmount(progressImg.fillAmount + step, time * 0.8f);
        else
            progressImg.fillAmount = 1F;
    }
    
    public void SetBarFullness(float currentPoints, float maxPoints, float animTime)
    {
        progressImg.DOFillAmount(currentPoints / maxPoints, animTime);
        progressText.text = $"{currentPoints} / {maxPoints}";
    }

    public bool GetBarFullness()
    {
        bool isFull;
        
        if (progressImg.fillAmount < 0.98)
            isFull = false;
        else
            isFull = true;

        return isFull;
    }

    public void UpdateTextInPercentage()
    {
        int proc = (int)(progressImg.fillAmount * 100);
        
        progressText.text = $"{proc} %";
    }
}
