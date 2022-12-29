using System;
using System.Collections;
using System.Collections.Generic;
using com.adjust.sdk;
using UnityEngine;
using UnityEngine.Events;

public class AskAttController : MonoBehaviour
{

    public const int ATT_STATUS_NOT_DETERMINED = 0;
    public const int ATT_STATUS_RESTRICTED = 1;
    public const int ATT_STATUS_DENIED = 2;
    public const int ATT_STATUS_AUTHORIZED = 3;
    public const int ATT_STATUS_NOT_AVAILABLE = -1;

    private UnityAction askAttFinished;
    private UnityAction askOpenSettings;

    public void CheckAndShowATTPopup(UnityAction askAttFinished, UnityAction askAttOpenSettings)
    {
        this.askAttFinished = askAttFinished;
        this.askOpenSettings = askAttOpenSettings;
        //StartCoroutine(IECheckAndShowATT());
    }

    //private IEnumerator IECheckAndShowATT()
    //{
    //    if (Helper.FromIOS14())
    //    {
    //        while (!RemoteConfigManager.remoteConfigActivated)
    //        {
    //            yield return null;
    //        }
    //        bool shouldAskAtt = RemoteConfigManager.GetBool(StringConstants.RC_SHOULD_ASK_ATT);
    //        int attStatus = Adjust.getAppTrackingAuthorizationStatus();
    //        if (shouldAskAtt)
    //        {
    //            if (Helper.FromIOS145() || attStatus == ATT_STATUS_DENIED || attStatus == ATT_STATUS_RESTRICTED)
    //            {
    //                if (attStatus != ATT_STATUS_AUTHORIZED)
    //                {
    //                    AskATTPanel.Setup().Show(askAttFinished, askOpenSettings);
    //                    yield break;
    //                }
    //            }
    //        }
    //    }
    //    askAttFinished?.Invoke();
    //}
}
