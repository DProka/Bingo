using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UICardCountButton : MonoBehaviour
{
    [SerializeField] Image shadeImage;
    [SerializeField] Image chestsImage;
    [SerializeField] Image buttonImage;

    private JoinGameMenuSettings settings;
    private bool isX2;

    public void Init(JoinGameMenuSettings _settings, bool _isX2)
    {
        settings = _settings;
        isX2 = _isX2;
    }

    public void SwitchActive(bool isActive)
    {
        shadeImage.enabled = !isActive;
    }

    public void SetChestSprite(int num)
    {
        chestsImage.sprite = settings.menuChestsSpritesArray[num];
        chestsImage.transform.DOPunchScale(new Vector3(-0.5f, -0.5f, 0), 0.1f, 0).OnComplete(() => { chestsImage.transform.localScale = new Vector3(1, 1, 0); });
        shadeImage.sprite = isX2 ? settings.shadowX2SpritesArray[num] : settings.shadowX4SpritesArray[num];
        shadeImage.SetNativeSize();
    }

    public void SetButtonActive(bool isActive)
    {
        buttonImage.sprite = isActive ? settings.buttonSpritesArray[0] : settings.buttonSpritesArray[1];
    }
}
