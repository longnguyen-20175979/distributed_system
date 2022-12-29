using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WidogameFoundation.Ads;

public class ShowHideBannerController : MonoBehaviour
{
    [SerializeField]
    private bool showOnEnabled;
    private void OnEnable()
    {
        if (showOnEnabled)
        {
            if (GameData.inGame)
                AdsMediationController.Instance.ShowBanner(BannerPosition.Top);
            else
                AdsMediationController.Instance.ShowBanner(BannerPosition.Bottom);
        }
        else
        {
            AdsMediationController.Instance.HideBanner();
        }
    }

    private void OnDisable()
    {
        if (showOnEnabled)
        {
            AdsMediationController.Instance.HideBanner();
        }
        else
        {
            if (GameData.inGame)
                AdsMediationController.Instance.ShowBanner(BannerPosition.Top);
            else
                AdsMediationController.Instance.ShowBanner(BannerPosition.Bottom);
        }
    }
}
