using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialArrow : MonoBehaviour
{
    [SerializeField] bool animationIsDown;
    [SerializeField] bool isNotMoving;

    private float animationTime = 1.2f;
    private Vector3 startPos;
    private bool isActive;
    private bool isMoving;

    private void LateUpdate()
    {
        if(isNotMoving)
            transform.position = new Vector3(startPos.x, transform.position.y); 
    }

    public void SetActive(bool active) 
    { 
        isActive = active;
        //startPos = transform.position;

        if (isActive && !isMoving)
            StartCoroutine(AnimateArrow());
    }

    public IEnumerator AnimateArrow()
    {
        startPos = transform.position;
        float dir = -15f;

        if (animationIsDown)
            dir = +15f;
        else
            dir = -15f;

        Vector3 newPos = new Vector3(startPos.x, startPos.y + dir);

        transform.DOPunchPosition(newPos, animationTime - 0.5f, 0, 1).SetDelay(0.5f);

        yield return new WaitForSeconds(animationTime);

        if (isActive)
            StartCoroutine(AnimateArrow());
    }

    public IEnumerator MoveArrow(float pos) 
    {
        isMoving = true;
        StopCoroutine(AnimateArrow());
        transform.DOComplete();
        transform.DOMoveX(transform.position.x + pos, 0.2f);

        yield return new WaitForSeconds(0.2f);

        isMoving = false;
        transform.DORestart();
    }
}
