using System.Collections;
using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPPurchaseHandler : IAPListener {
    public const string REMOVE_ADS_PRODUCT_ID = "flintgo.removeads";
    public const string STARTER_PACK_PRODUCT_ID = "flintgo.starterpack";

    [SerializeField]
    private IAPPackContent iapPackContent;

    public void ProcessSuccessPurchase(Product product) {
        if (iapPackContent.ContainsKey(product.definition.id)) {
            ResourceItemCollection coll = iapPackContent[product.definition.id];
            if (Helper.VerifyIap(product)) {
                if(STARTER_PACK_PRODUCT_ID == product.definition.id)
                {
                    GameData.starterPack = true;
                }    
                GameData.ClaimResourceItems(coll.items);
                foreach (var item in coll.items) {
                    if (item.countable)
                        //GameAnalytics.LogEarnVirtualCurrency("shop_iap_" + product.definition.id, item.quantity, item.resourceItem.ToString());
                        if (item.type == ResourceType.RemoveAds) {
                            //this.PostEvent(EventID.BuyNoads);
                            //PopupNoAds.Setup().Close();
                        }
                }
                UIClaimResourcePanel.Setup(coll.items).Show();
                //GameAnalytics.LogBuyIAP(product.definition.id);
                GameAnalytics.LogInAppPurchase(product);
            }
        }
    }   
}

[System.Serializable]
public class IAPPackContent : SerializableDictionaryBase<string, ResourceItemCollection> {
}
