using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class CometScript : MonoBehaviour
{   
    [SerializeField] float animationTime = 0.6f;
    [SerializeField] Transform startBone;
    [SerializeField] Transform finishBone;

    private SkeletonGraphic animation;

    public IEnumerator Init(Vector3 start, Vector3 finish)
    {
        animation = GetComponent<SkeletonGraphic>();
        transform.position = start;
        startBone.transform.position = start;
        finishBone.transform.position = finish;

        yield return new WaitForSeconds(0.1f);

        animation.timeScale = animationTime;
        Destroy(gameObject, 2f);
    }
}
