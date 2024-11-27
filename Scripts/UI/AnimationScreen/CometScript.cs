using System.Collections;
using UnityEngine;
using Spine.Unity;

public class CometScript : MonoBehaviour
{   
    [SerializeField] Transform startBone;
    [SerializeField] Transform finishBone;
    [SerializeField] SkeletonGraphic SkeletonAnim;

    private float animationTime = 0.6f;

    public void Init(Vector3 start, Vector3 finish, float animTime)
    {
        transform.position = start;
        startBone.transform.position = start;
        finishBone.transform.position = finish;
        animationTime = animTime;

        StartCoroutine(StartAnimation());
    }

    private IEnumerator StartAnimation()
    {
        yield return new WaitForSeconds(0.1f);

        SkeletonAnim.timeScale = animationTime;
        Destroy(gameObject, 2f);
    }
}
