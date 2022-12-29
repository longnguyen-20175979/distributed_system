using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupRemoveAds : BaseBox
{
    public static PopupRemoveAds instance;
    public static PopupRemoveAds Setup()
    {
        if (instance == null)
        {
            instance = Instantiate(Resources.Load<PopupRemoveAds>(StringConstants.PathPrefabs.POPUP_REMOVE_ADS));
        }
        return instance;
    }
}
