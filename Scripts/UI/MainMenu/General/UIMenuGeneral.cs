
using UnityEngine;
using DG.Tweening;

public class UIMenuGeneral : MonoBehaviour
{
    [Header("Main Part")]

    [SerializeField] GameObject menuObj;
    [SerializeField] GameObject backObj;
    [SerializeField] GameObject shadeObj;

    public virtual void OpenMain()
    {
        if (menuObj != null)
            menuObj.SetActive(true);

        if (shadeObj != null)
            shadeObj.SetActive(true);

        if (backObj != null)
        {
            backObj.SetActive(true);
            backObj.transform.localScale = new Vector3(0.1f, 0.1f, 0);
            backObj.transform.DOScale(1, 0.2f);
        }

        EventBus.onWindowOpened?.Invoke();
    }

    public virtual void CloseMain()
    {
        if (menuObj != null)
            menuObj.SetActive(false);
    }

    public void SwitchShade(bool isOn) { shadeObj.SetActive(isOn); }

    public void SetMainMenuOnject(GameObject obj) => menuObj = obj;
}
