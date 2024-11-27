using System.Collections;
using UnityEngine;
using Spine.Unity;
using Spine;

public class FirstLoadingScreen : MonoBehaviour
{
    [SerializeField] GameObject window;
    [SerializeField] SkeletonGraphic logoSkel;

    [Header("Progress Bar")]

    [SerializeField] ProgressBar progressBar;
    
    private float progressTime;

    public void Init(float time)
    {
        progressTime = time;
    }

    private void Update()
    {
        if (window.activeSelf)
            progressBar.UpdateTextInPercentage();
    }

    private IEnumerator ActivateProgress()
    {
        float step = 1 / progressTime;

        progressBar.FillBarByStep(step, 1);

        yield return new WaitForSeconds(1f);

        if (!progressBar.GetBarFullness())
        {
            StartCoroutine(ActivateProgress());
        }
    }

    #region Logo Animation

    private void StartLodoAnimation()
    {
        logoSkel.AnimationState.Complete += ContinueLogoAnimation;

        logoSkel.AnimationState.TimeScale = 1f;
        logoSkel.AnimationState.SetAnimation(0, "Create", false);
    }

    private void ContinueLogoAnimation(TrackEntry trackEntry)
    {
        logoSkel.AnimationState.Complete -= ContinueLogoAnimation;

        logoSkel.AnimationState.SetAnimation(0, "IDLE", true);
    }

    #endregion

    #region Window

    public void OpenWindow() 
    {
        window.SetActive(true);
        progressBar.ResetProgress();
        StartLodoAnimation();
        StartCoroutine(ActivateProgress());
        EventBus.onWindowOpened?.Invoke();

        AppMetrica.reportEvent("Loading_begin", "");
    }

    public void CloseWindow() 
    {
        window.SetActive(false);
        logoSkel.AnimationState.TimeScale = 0f;

        EventBus.onWindowClosed?.Invoke();
    }

    #endregion
}
