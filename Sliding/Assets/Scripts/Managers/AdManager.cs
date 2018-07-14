using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour {

    public UnityEvent successCallback;

    public UnityEvent finishedCallback;

    public bool testMode = false;

    private string androidID = "2652968";



    public void Awake()
    {
        if (Advertisement.isSupported && !Advertisement.isInitialized)
        {
            Advertisement.Initialize(androidID, testMode);
        }
    }

    public void ShowVideo()
    {
        if (Advertisement.IsReady("video"))
        {
            Advertisement.Show("video", new ShowOptions() { resultCallback = HandleAdResult });
        }
    }

    public void ShowRewardedVideo()
    {
        if (Advertisement.IsReady("rewardedVideo"))
        {
            Advertisement.Show("rewardedVideo", new ShowOptions() { resultCallback = HandleAdResult });
        }
    }

    public bool IsReady(string id)
    {
        return Advertisement.IsReady(id);
    }
    
    private void HandleAdResult(ShowResult result)
    {
        switch(result)
        {
            case ShowResult.Finished: if(successCallback != null) successCallback.Invoke();
                break;
            case ShowResult.Skipped: 
                break;
            case ShowResult.Failed: 
                break;
        }
        if(finishedCallback != null)
            finishedCallback.Invoke();
    }
    
}
