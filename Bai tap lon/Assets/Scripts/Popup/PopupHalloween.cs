using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class PopupHalloween : BaseBox
{
    public static PopupHalloween instance;

    [SerializeField] private Text txtPumpkin;
    [SerializeField] private Slider slider;
    [SerializeField] private List<ResourceItemHalloween> halloweenGifts;

    [SerializeField] private Text txtTime;

    protected override void OnAwake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public static PopupHalloween SetUp()
    {
        if (instance == null)
        {
            instance = Instantiate(Resources.Load<PopupHalloween>(Constants.PathPrefabs.POPUP_HALLOWEEN));
        }
        return instance;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if (!GameData.FirstOpenHalloweenPopup)
        {
            GameData.FirstTimeOpenHalloweenPopup = DateTime.Now.AddDays(14);           
            GameData.FirstOpenHalloweenPopup = true;
        }
        UpdatePumpkinMark();
        GameData.RegisterResourceChangedListener(ResourceType.Pumpkin, SetTextPumpkin);
        SetTextPumpkin();
    }

    protected override void OnDisable()
    {
        base.OnDisable(); 
        GameData.RemoveResourceChangedListener(ResourceType.Pumpkin, SetTextPumpkin);
    }

    private void Update()
    {
        TimeSpan time = GameData.FirstTimeOpenHalloweenPopup - DateTime.Now;
        txtTime.text = time.Days + " DAYS " + time.Hours + " : " + time.Minutes + " : " + time.Seconds;
        if (time.TotalSeconds < 0)
        {
            txtTime.text = "Event Ended!";
        }
    }

    public void ShowShopPumpkin()
    {
        PanelShopController panelShop = PanelShopController.Setup();
        panelShop.Show();
        panelShop.ShowTabPumpkin();
    }

    private void SetTextPumpkin()
    {
        txtPumpkin.text = GameData.Pumpkin.ToString();
    }

    internal void UpdatePumpkinMark()
    {
        if (GameData.PumpkinReceived >= 10 && GameData.PumpkinReceived < 50)
        {
            slider.value = 150;
        }
        else if (GameData.PumpkinReceived >= 50 && GameData.PumpkinReceived < 100)
        {
            slider.value = 320;
        }
        else if (GameData.PumpkinReceived >= 100 && GameData.PumpkinReceived < 250)
        {
            slider.value = 480;
        }
        else if (GameData.PumpkinReceived >= 250 && GameData.PumpkinReceived < 500)
        {
            slider.value = 660;
        }
        else if (GameData.PumpkinReceived >= 500 && GameData.PumpkinReceived < 1000)
        {
            slider.value = 820;
        }
        else if (GameData.PumpkinReceived >= 1000)
        {
            slider.value = 1000;
        }
        for (int i = 0; i < halloweenGifts.Count; i++)
        {
            halloweenGifts[i].UpdateBtnStatus();
        }
    }

    //IEnumerator CountDownTime()
    //{
    //    while (totalTime > 0)
    //    {
    //        totalTime--;
    //        txtTime.text = GameData.getFormattedTimeFromSeconds(totalTime);
    //        yield return new WaitForSecondsRealtime(1);
    //    }
    //}
}
