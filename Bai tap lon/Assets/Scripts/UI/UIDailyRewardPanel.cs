﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.UI;

public class UIDailyRewardPanel : BaseBox
{

    [SerializeField]
    private GameObject btnClaim;

    [SerializeField]
    private GameObject btnClaimX2;

    //[SerializeField]
    //private VideoAdLoadingSpinnerController adLoadingSpinnerController;

    private DailyRewardItem currentDailyRewardItem;
    private DailyRewardItem[] dailyRewardItems;
    private int dayIndex;
    private static UIDailyRewardPanel instance;

    [SerializeField] private Text txtClaim;

    //private WatchVideoAdButtonController 

    private bool IsClaimable
    {
        get
        {
            return UnbiasedTime.Instance.Now() > GameData.LastDailyRewardClaim.Date;
        }
    }
    public static UIDailyRewardPanel Setup()
    {
        if (instance == null)
        {
            instance = Instantiate(Resources.Load<UIDailyRewardPanel>(Constants.PathPrefabs.DAILY_BOX));
        }
        return instance;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        dayIndex = GameData.CurrentDailyRewardDayIndex;
        dailyRewardItems = GetComponentsInChildren<DailyRewardItem>();
        currentDailyRewardItem = dailyRewardItems.Where(x => x.dayIndex == dayIndex).First();
        if (dayIndex != GameData.LastDailyRewardDayIndex)
        {
            btnClaimX2.SetActive(true);
            btnClaim.SetActive(true);
        }
        else
        {
            btnClaimX2.SetActive(false);
            btnClaim.SetActive(false);
        }
    }
    protected override void OnDisable()
    {
        btnClaim.SetActive(false);
        base.OnDisable();
    }

    public bool DetermineIfShow()
    {
        DateTime lastDailyRewardClaim = GameData.LastDailyRewardClaim;
        if (DateTime.Now.Date > lastDailyRewardClaim.Date)
        {
            Show();
            return true;
        }
        return false;
    }
    public void GetX2(bool success)
    {
        if (success)
            if (success)
            {
                GameAnalytics.LogWatchRewardAdsDone("daily_reward", true, "true");
                //Close();
                currentDailyRewardItem.Claim(2);
            }
            else GameAnalytics.LogWatchRewardAdsDone("daily_reward", true, "false");
    }

    public void Claim()
    {
        GameAnalytics.LogButtonClick("no_thanks", "popup_daily_reward");
        if (currentDailyRewardItem != null && IsClaimable)
        {
            currentDailyRewardItem.Claim(1);
        }
        //Close();
    }

    public void Show(bool closeButtonVisible)
    {
        base.Show();
        if (!closeButtonVisible)
        {
            btnClaim.GetComponent<Button>().interactable = false;
            btnClaim.transform.GetChild(0).GetComponent<Text>().color = Vector4.zero;
            StartCoroutine(Helper.StartAction(() => btnClaim.GetComponent<Button>().interactable = true, 2.5f));
            StartCoroutine(ShowBtnClaim());
        }
    }

    private IEnumerator ShowBtnClaim()
    {
        yield return new WaitForSeconds(2.15f);
        float alpha = 0;
        while (alpha <= 0.7f)
        {
            alpha += 0.1f;
            txtClaim.color = new Color(txtClaim.color.r, txtClaim.color.g, txtClaim.color.b, alpha);
            yield return new WaitForSeconds(0.05f);
        }        
    }

    public void ShowFail()
    {
        GameAnalytics.LogWatchRewardAdsDone("daily_reward", true, "cant_show");
        PopupNoVideo.Setup().Show();
    }
}
