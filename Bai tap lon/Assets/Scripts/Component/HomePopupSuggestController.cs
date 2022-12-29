using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomePopupSuggestController : MonoBehaviour
{
    private static HomePopupSuggestController instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            return;
        }
        Destroy(this.gameObject);
    }

    void Start()
    {
        SuggesetPopup();
    }

    private void SuggesetPopup()
    {
        if (!GameData.vip && !GameData.removeAds)
        {
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                if (GameData.countOpenAppOneDay == 3)
                {
                    //GameData.buyIAPFrom = "auto_suggest_third_in_day";
                    //GameData.showPopupFrom = GameData.buyIAPFrom;
                    //GameData.lastScenario = "auto_suggest_third_in_day";
                    GameAnalytics.LogUIAppear("popup_remove_ads_third_open", "HomeScene");
                    PopupRemoveAds.Setup().Show();
                }
                else if (GameData.countOpenAppOneDay == 2)
                {
                    GameAnalytics.LogUIAppear("popup_vip_second_open", "HomeScene");
                    UIVipController.Setup().Show();
                    //GameData.buyIAPFrom = "auto_suggest_seconds_in_day";
                    //GameData.lastScenario = "auto_suggest_seconds_in_day";
                    //GameData.showPopupFrom = GameData.buyIAPFrom;

                }
            }
            GameData.SetStartTimeOpenDay(UnbiasedTime.Instance.Now());
        }
    }
}
