using com.adjust.sdk;
using DarkTonic.MasterAudio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HomeUI : MonoBehaviour
{
    [SerializeField] Text txtGem;
    [SerializeField] Text txtHeart;
    [SerializeField] Text txtBullet;
    [SerializeField] Text txtPumpkin;
    [SerializeField]
    [Playlist]
    private string introPlaylist;
    [SerializeField] GameObject btnVip;
    [SerializeField] ClaimCoinFx claimCoin;
    [SerializeField] GameObject btnAddGem;
    [SerializeField] GameObject indebug;
    public void ShowDailyReward()
    {
        UIDailyRewardPanel.Setup().Show();
        GameAnalytics.LogUIAppear("popup_daily_reward", "HomeScene");
    }
    private void Start()
    {
        GameAnalytics.LogUIAppear("screen_home", "HomeScene");
        GameData.RegisterResourceChangedListener(ResourceType.Heart, SetTextHeart);
        GameData.RegisterResourceChangedListener(ResourceType.Gem, SetTextGem);
        GameData.RegisterResourceChangedListener(ResourceType.BulletNormal, SetTextBullets);
        GameData.RegisterResourceChangedListener(ResourceType.Pumpkin, SetTextPumpkin);
        SetTextGem();
        SetTextHeart();
        SetTextBullets();
        SetTextPumpkin();
        Application.targetFrameRate = 60;
        MasterAudio.StartPlaylist(introPlaylist);
        CheckBtnVip(null);
        this.RegisterListener(EventID.BuyVip, CheckBtnVip);
        if (GameData.isPendingShowHalloweenPopup) {
            GameData.isPendingShowHalloweenPopup = false;
            ShowHalloweenPopupWithCallback(CheckAndShowDailyRewardPopup);
        } else {
            CheckAndShowDailyRewardPopup();
        }

        if (!GameData.newUser && GameData.Bullets > 0)
        {
            GameData.Bullets = 0;
        }
        Invoke("AddGiftNewUser", 0.1f);       
        AdsMediationController.Instance.InitInterstitial();
#if ENV_PROD
  indebug.SetActive(false);
#else
        indebug.SetActive(true);
#endif
        StartCoroutine(AskATT());

    }
    IEnumerator AskATT()
    {
#if UNITY_IOS
        while (!RemoteConfigManager.remoteConfigActivated) {
            yield return null;
        }
        int attStatus = Adjust.getAppTrackingAuthorizationStatus();
        if (Helper.FromIOS145() && attStatus == AskAttController.ATT_STATUS_NOT_DETERMINED) {
            Adjust.requestTrackingAuthorizationWithCompletionHandler((status) => {
                bool authorized = status == AskAttController.ATT_STATUS_AUTHORIZED;
                GameAnalytics.LogFirebaseUserProperty("idfa_enabled", authorized.ToString());
                GameAnalytics.LogAttRequest(authorized);
            });
        } 
#endif
        yield return null;
    }

    void AddGiftNewUser()
    {
        if (GameData.newUser)
        {
            GameData.Bullets = 1;
            GameData.Heart = 1;
            GameData.SetTotalResourceDetail(ResourceType.Character, ResourceDetail.CharacterFlint, 1);
        }
    }
    //private void OnDailyRewardClose()
    //{
    //    //CheckAndShowVipPopup();
    //    UIDailyRewardPanel.Setup().OnCloseBox = null;
    //}
    //private void CheckAndShowVipPopup()
    //{
    //    if (!GameData.isVipSuggested && !GameData.vip)
    //    {
    //        UIVipController.Setup().Show(GameData.IsTodayFirstOpen);
    //        GameData.isVipSuggested = true;
    //    }
    //}
    void CheckBtnVip(object a)
    {
        if (GameData.vip)
        {
            btnVip.SetActive(false);
        }
        else
        {
            btnVip.SetActive(true);
        }
    }
    void SetTextGem()
    {
        txtGem.text = GameData.Gem.ToString();
    }
    void SetTextHeart()
    {
        txtHeart.text = GameData.Heart.ToString();
    }
    void SetTextBullets()
    {
        txtBullet.text = GameData.Bullet.ToString();
    }
    void SetTextPumpkin()
    {
        txtPumpkin.text = GameData.Pumpkin.ToString();
    }
    private void OnDisable()
    {
        GameData.RemoveResourceChangedListener(ResourceType.Heart, SetTextHeart);
        GameData.RemoveResourceChangedListener(ResourceType.Gem, SetTextGem);
        GameData.RemoveResourceChangedListener(ResourceType.BulletNormal, SetTextBullets);
        GameData.RemoveResourceChangedListener(ResourceType.Pumpkin, SetTextPumpkin);
        this.RemoveListener(EventID.BuyVip, CheckBtnVip);
    }
    public void PlayGame()
    {       
        if (GameData.LevelUnlock > Constants.MAX_UNLOCK_LEVEL)
        {
            PopupComingSoon popup = PopupComingSoon.Setup();
            popup.OnCloseBox = null;
            popup.Show();
            return;
        }
        GameData.levelSelected = GameData.LevelUnlock;
        PopupStartLevel.Setup().Show();
        GameAnalytics.LogButtonClick("play_game_button", "HomeScene");
    }
    public void ShowShop(int typeShop)
    {
        GameAnalytics.LogButtonClick("shop_button", "HomeScene");
        PanelShopController panelShop = PanelShopController.Setup();
        panelShop.Show();
        if (typeShop == 0)
        {
            panelShop.ShowTapCombo();
        }
        else if (typeShop == 1)
        {
            panelShop.ShowTapGem();
        }
        else if (typeShop == 2)
        {
            panelShop.ShowTapItem();
        }
        else if (typeShop == 3)
        {
            panelShop.ShowTabCharacter();
        }
        else if (typeShop == 4)
        {
            panelShop.ShowTabPumpkin();
        }
    }
    public void ShowVip()
    {
        UIVipController.Setup().Show();
        GameAnalytics.LogButtonClick("vip_button", "HomeScene");
        GameAnalytics.LogUIAppear("popup_vip", "HomeScene");
    }
    public void GemAds(bool success)
    {
        if (success)
        {
            GameAnalytics.LogWatchRewardAdsDone("gem_ads_home", true, "true");
            ClaimCoinFx claim = Instantiate(claimCoin, btnAddGem.transform);
            claim.gameObject.SetActive(true);
            claim.transform.localPosition = Vector3.zero;
            claim.PlayCoin(txtGem.transform);
            StartCoroutine(Helper.StartAction(() => GameData.Gem += 5, 1.2f));
            GameData.GemReceived += 5;
            GameAnalytics.LogGetItem("Gem", 5, "ads", 1, 1, "HomeScene", "ui_diamond");
        }
        else GameAnalytics.LogWatchRewardAdsDone("gem_ads_home", true, "false");
    }

    public void ShowFail()
    {
        GameAnalytics.LogWatchRewardAdsDone("gem_ads_home", true, "cant_show");
        PopupNoVideo.Setup().Show();
    }

    public void ShowHalloweenPopup()
    {
        GameAnalytics.LogButtonClick("halloween_button", "HomeScene");
        ShowHalloweenPopupWithCallback(null);
    }

    private void CheckAndShowDailyRewardPopup() {
        if (GameData.LastDailyRewardDayIndex != GameData.CurrentDailyRewardDayIndex) {
            UIDailyRewardPanel rewardPanel = UIDailyRewardPanel.Setup();
            rewardPanel.OnCloseBox = null;
            rewardPanel.Show(false);
            //GameData.CountShowGetMoreLife = 0;
            GameAnalytics.LogUIAppear("popup_daily_reward", "HomeScene");
        }
    }

    private void ShowHalloweenPopupWithCallback(UnityAction callback) {
        PopupHalloween popup = PopupHalloween.SetUp();
        popup.OnCloseBox = callback;
        GameAnalytics.LogUIAppear("popup_halloween", "HomeScene");
        popup.Show();
    }
}