using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class UIVipController : BaseBox {
    [SerializeField]
    private IAPButton iapButton;

    private static UIVipController instance;
    [SerializeField] private Button restoreBtn;
    [SerializeField] private Text priceTxt;
    protected string price;

    public static UIVipController Setup() {
        if (instance == null) {
            instance = Instantiate(Resources.Load<UIVipController>(StringConstants.PathPrefabs.POPUP_VIP));
        }
        instance.OnCloseBox = null;
        return instance;
    }

    private IEnumerator Start()
    {
        while (!CodelessIAPStoreListener.initializationComplete)
        {
            yield return null;
        }
        Product product = CodelessIAPStoreListener.Instance.GetProduct("flintgo.vip.week");
        price = product.metadata.localizedPriceString;
        yield return new WaitForEndOfFrame();
        priceTxt.text = price + " PER WEEK";
    }

    protected override void OnEnable()
    {
        base.OnEnable();
#if UNITY_ANDROID
        restoreBtn.interactable = false;       
#endif
    }

    public void OnVipPurchaseOptionChanged(int selectedOption) {
        iapButton.productId = VipController.VIP_PRODUCT_IDS[selectedOption];
    }

    public void BuySuccess(Product product) {
        //Debug.Log(product.transactionID);
        VipController.Instance.ProcessVipPurchase(product);
        //shouldSuggestVipOneTime = false;
        DelayClose();
    }
    public void OpenURLTermOfUse() {
        Application.OpenURL("https://commandoo247.blogspot.com/2021/08/term-of-use-commandoo.html");
        GameAnalytics.LogButtonClick("terms", "popup_vip");
    }
    public void OpenURLPrivacy() {
        Application.OpenURL("https://commandoo247.blogspot.com/2021/08/privacy.html");
        GameAnalytics.LogButtonClick("privacy", "popup_vip");
    }
}
