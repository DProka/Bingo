using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BoosterProgressBar : MonoBehaviour
{
    [SerializeField] GameObject boosterObj;
    [SerializeField] Image boosterImage;
    [SerializeField] Sprite[] progressSprites;

    private bool isActive;
    private BoosterProgressSlot[] slotArray;
    
    public void Init()
    {
        slotArray = new BoosterProgressSlot[transform.childCount];

        //for (int i = 0; i < slotArray.Length; i++)
        //{
        //    slotArray[i] = transform.GetChild(i).GetComponent<BoosterProgressSlot>();
        //    slotArray[i].Init();
        //}

        ResetProgress();
    }

    public void UpdateProgress(int progress)
    {
        //for (int i = 0; i < progress; i++)
        //{
        //    slotArray[i].OpenSlot();
        //}

        boosterImage.sprite = progressSprites[progress];
    }

    public void ResetProgress()
    {
        //foreach (BoosterProgressSlot slot in slotArray)
        //{
        //    slot.CloseSlot();
        //}

        boosterImage.sprite = progressSprites[0];
    }

    private void ChangeSprite()
    {

    }

    //public void SwitchBoosterAnimation(bool _isActive)
    //{
    //    isActive = _isActive;

    //    if (isActive)
    //        StartCoroutine(StartBoosterAnim());

    //    else
    //        StopCoroutine(StartBoosterAnim());
    //}

    private IEnumerator StartBoosterAnim()
    {
        if (!isActive)
            yield break;
        
        else
        {
            boosterObj.transform.DOPunchScale(new Vector3(0.3f, 0.3f, 0), 0.3f, 1);

            yield return new WaitForSeconds(0.5f);

            StartCoroutine(StartBoosterAnim());
        }
    }
}
