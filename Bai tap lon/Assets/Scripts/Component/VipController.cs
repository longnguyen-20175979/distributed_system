using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using System.Linq;
using System;
using UnityEngine.SceneManagement;
using com.adjust.sdk;
using System.Text.RegularExpressions;
using UnityEngine.Purchasing.MiniJSON;
using UnityEngine.Purchasing.Security;
//using com.adjust.sdk;

public class VipController : IAPListener {
    public static VipController Instance;
    public const string SUBS_VIP_YEARLY_PRODUCT_ID = "flintgo.vip.year";
    public const string SUBS_VIP_WEEKLY_PRODUCT_ID = "flintgo.vip.week";
    public const string SUBS_VIP_MONTHLY_PRODUCT_ID = "flintgo.vip.month";
    //public const string IAP_VIP_ONE_TIME_PRODUCT_ID = "colorball_iap_vip_one_time";
    public static readonly string[] VIP_PRODUCT_IDS = new string[] { SUBS_VIP_WEEKLY_PRODUCT_ID, SUBS_VIP_MONTHLY_PRODUCT_ID, SUBS_VIP_YEARLY_PRODUCT_ID };

    [SerializeField]
    private ResourceItemCollection resourceItemColl;

    private void Awake() {
        Instance = this;
    }

    // Start is called before the first frame update
    private IEnumerator Start() {
        while (!CodelessIAPStoreListener.initializationComplete) {
            yield return null;
        }
        CheckValidVip();
    }

    public void CheckValidVip() {
        if (GameData.vip) {
            bool foundExpired = false;
            bool foundValid = false;
            foreach (string productId in VIP_PRODUCT_IDS) {

                Result isSubscriptionExpired = IsExpired(productId);
                if (isSubscriptionExpired == Result.True) {
                    foundExpired = true;
                } else if (isSubscriptionExpired == Result.False) {
                    foundValid = true;
                }
            }
            if (foundExpired && !foundValid) {
                GameData.vip = false;
            }
        } else {
            foreach (string productId in VIP_PRODUCT_IDS) {
                Product product = CodelessIAPStoreListener.Instance.GetProduct(productId);
                if (product != null && product.hasReceipt) {
                    ProcessVipPurchase(product);
                }
            }
        }
    }

    public void ProcessVipPurchase(Product product) {

        if (IsValidProductId(product.definition.id) && !GameData.vip) {

#if !UNITY_EDITOR
            var validator = new CrossPlatformValidator(GooglePlayTangle.Data(),
    AppleTangle.Data(), Application.identifier);
            var validationResult = validator.Validate(product.receipt);
#endif
#if UNITY_EDITOR
            bool validPurchase = true;
#else
            IPurchaseReceipt purchaseReceipt = null;
            bool validPurchase = false;
            //Debug.Log("Receipt is valid: " + product.transactionID);
            foreach (IPurchaseReceipt productReceipt in validationResult)
            {
				//Debug.Log("productId: " + productReceipt.productID);
				//Debug.Log("purchaseDate: " + productReceipt.purchaseDate);
				//Debug.Log("transactionId: " + productReceipt.transactionID);
#if UNITY_IOS
				//AppleInAppPurchaseReceipt appleReceipt = productReceipt as AppleInAppPurchaseReceipt;
				//Debug.Log("original Transaction Id: " + appleReceipt.originalTransactionIdentifier);
				//if ((product.definition.type == ProductType.Subscription && appleReceipt.subscriptionExpirationDate <= UnbiasedTime.Instance.Now())
				//    || (appleReceipt.productType == (int)ProductType.NonConsumable && product.definition.type == ProductType.NonConsumable))
				//{
				//    purchaseReceipt = productReceipt;
				validPurchase = true;
                    break;
                //}
#else
                GooglePlayReceipt googlePlayReceipt = productReceipt as GooglePlayReceipt;                
                if (googlePlayReceipt.purchaseState == GooglePurchaseState.Purchased) {
                    purchaseReceipt = productReceipt;
                    validPurchase = true;
                    break;
                }    
#endif
        }
#endif
			Debug.Log("vip success");
            if (validPurchase && IsExpired(product.definition.id) == Result.False) {
                GameData.vip = true;
                GameData.ClaimResourceItems(resourceItemColl.items);
                //GameAnalytics.LogEarnVirtualCurrency("shop_iap_" + product.definition.id, resourceItemColl.items);
                ClaimAllVip();
                //GameAnalytics.LogBuyIAP(product.definition.id);
                this.PostEvent(EventID.BuyVip);
                UIVipController.Setup().Close();
                StartCoroutine(IEShowClaimVipPanel());
                if (product.definition.type == ProductType.Subscription) {
                    if (product.hasReceipt) {
#if !UNITY_EDITOR
#if UNITY_IOS

                        AdjustAppStoreSubscription subscription = new AdjustAppStoreSubscription(
            product.metadata.localizedPrice.ToString(),
            product.metadata.isoCurrencyCode,
            product.transactionID,
            product.receipt);
                        //subscription.transactionDate = ((DateTimeOffset)purchaseReceipt.purchaseDate).ToUnixTimeMilliseconds().ToString();
                        Adjust.trackAppStoreSubscription(subscription);
#else
                        GooglePlayReceipt googleReceipt = purchaseReceipt as GooglePlayReceipt;
                        var dict = Json.Deserialize(product.receipt) as Dictionary<string, object>;
                        var googleJsonDict = Json.Deserialize((string)dict["Payload"]) as Dictionary<string, object>;
                        string signature = (string)googleJsonDict["signature"];
                        long transactionDate = ((DateTimeOffset)purchaseReceipt.purchaseDate).ToUnixTimeMilliseconds();
                        AdjustPlayStoreSubscription subscription = new AdjustPlayStoreSubscription(
            product.metadata.localizedPrice.ToString(),
            product.metadata.isoCurrencyCode,
            product.definition.id,
            product.transactionID,
            signature,
            googleReceipt.purchaseToken);
                        subscription.setPurchaseTime(transactionDate.ToString());
                        Adjust.trackPlayStoreSubscription(subscription);
#endif
#endif
                    }
                } else {
                    GameAnalytics.LogInAppPurchase(product);
                }
            }
        }
    }

    private IEnumerator IEShowClaimVipPanel() {
        while (SceneManager.GetActiveScene().buildIndex == StringConstants.SCENE.LOADING) {
            yield return new WaitForEndOfFrame();
        }
        PopupYouAreVip.Setup().Show();
    }

    private bool IsValidProductId(string productId) {
        return VIP_PRODUCT_IDS.Contains(productId);
    }

    public Result IsExpired(string productId) {
        var product = CodelessIAPStoreListener.Instance.GetProduct(productId);
        if (product != null && product.availableToPurchase && product.hasReceipt) {
            if (product.definition.type == ProductType.NonConsumable) {
                return Result.False;
            }
            IAppleExtensions m_AppleExtensions = CodelessIAPStoreListener.Instance.GetStoreExtensions<IAppleExtensions>();
            Dictionary<string, string> introductory_info_dict = m_AppleExtensions.GetIntroductoryPriceDictionary();

            string intro_json = (introductory_info_dict == null || !introductory_info_dict.ContainsKey(product.definition.storeSpecificId)) ? null : introductory_info_dict[product.definition.storeSpecificId];
#if UNITY_EDITOR
            return Result.False;
#else
            SubscriptionManager p = new SubscriptionManager(product, intro_json);
            SubscriptionInfo info = p.getSubscriptionInfo();
            //Debug.Log("expired date = " + info.getExpireDate().ToLongDateString());
            return info.isExpired();
#endif
        }
        return Result.Unsupported;
    }

    private void ClaimAllVip() {
        PopupYouAreVip.Setup().Show();
    }
}
