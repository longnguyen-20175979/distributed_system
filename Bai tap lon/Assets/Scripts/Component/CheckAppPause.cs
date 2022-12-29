using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckAppPause : MonoBehaviour
{
    private static CheckAppPause instance = null;
    private float pauseBeginTime;
    [SerializeField] private ShowInterstitialController showInterstitial;
    private float lastTimescale;
    private string screenName;

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

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SetExitTime();
        }
        else
        {
            if (RemoteConfigManager.remoteConfigActivated)
            {
                long sleepDuration = RemoteConfigManager.GetLong(StringConstants.RC_DURATION_SLEEP_GAME);
                if (Time.realtimeSinceStartup > pauseBeginTime + sleepDuration && !GameData.inIap && !GameData.showAds)
                {
                    showInterstitial.Show();
                }
            }
            GameData.inIap = false;
        }
    }

    private void SetExitTime()
    {
        pauseBeginTime = Time.realtimeSinceStartup;
    }

    public void ShowVipPopUp()
    {
        if (!GameData.vip)
        {
            screenName = SceneManager.GetActiveScene().name;
            GameAnalytics.LogUIAppear("popup_vip_after_10s", screenName);
            lastTimescale = Time.timeScale;
            UIVipController popup = UIVipController.Setup();
            popup.OnCloseBox = VipPopupClosed;
            popup.Show();
            if (GameData.inGame) {
                Time.timeScale = 0;
            }
        }
    }

    private void VipPopupClosed() {
        Time.timeScale = 1;
    }
}
