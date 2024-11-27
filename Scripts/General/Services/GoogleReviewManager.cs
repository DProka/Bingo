using Google.Play.Review;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleReviewManager : MonoBehaviour
{
    private ReviewManager _reviewManager;
    private PlayReviewInfo _playReviewInfo;

    public static Action callRequestReview;

    void Start()
    {
        _reviewManager = new ReviewManager();
        callRequestReview += CallRequestReview;
    }

    private void CallRequestReview()
    {
        Debug.Log("RequestReview is called");

        StartCoroutine(RequestReviewInfo());
    }

    private IEnumerator RequestReviewInfo()
    {
        var requestFlowOperation = _reviewManager.RequestReviewFlow();
        yield return requestFlowOperation;

        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
            yield break;
        }

        _playReviewInfo = requestFlowOperation.GetResult();

        var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
        yield return launchFlowOperation;
        _playReviewInfo = null; // Reset the object
        if (launchFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
            yield break;
        }
        // The flow has finished. The API does not indicate whether the user
        // reviewed or not, or even whether the review dialog was shown. Thus, no
        // matter the result, we continue our app flow.
    }

    private void OnDestroy()
    {
        callRequestReview -= CallRequestReview;
    }
}
