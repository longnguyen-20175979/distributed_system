using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceItemShop : MonoBehaviour 
{
    public ResourceItem[] resourceItems;
    public int numberAds;
    public int numberPrices;
    public int numberPumpkin;
    [SerializeField] Text txtAds;
    public int currentAds;
    public string idPack;
    [SerializeField] Text txtGem;
    [SerializeField] Text txtPumpkin;

    public virtual void OnEnable() {

        InitPopup();
    }
    void InitPopup() {
        currentAds = GameData.GetCurrentAdsItem(idPack);
        if (txtAds != null)
            UpdateTextCurAds();
        if(txtGem != null) {
            txtGem.text = numberPrices.ToString();
        }
        if (txtPumpkin != null)
        {
            txtPumpkin.text = numberPumpkin.ToString();
        }
    }
    public virtual void BuyComplete() {
        GameData.ClaimResourceItems(resourceItems);
        UIClaimResourcePanel.Setup(resourceItems).Show();        
    }
    public void BuyItemByAds(bool success)
    {
        if (success)
        {
            currentAds++;
            if (currentAds == numberAds)
            {
                currentAds = 0;               
                BuyComplete();
                GameData.GemReceived += resourceItems[0].quantity;
            }
            UpdateTextCurAds();
            GameData.SetCurrentAdsItem(idPack, currentAds);
            GameAnalytics.LogWatchRewardAdsDone("item_shop", true, "true");
            if (resourceItems[0].type == ResourceType.Character)
            {
                GameAnalytics.LogGetItem(resourceItems[0].detail.ToString(), 1, "ads", currentAds, numberAds, "HomeScene", "tab_character");
            }
        }
        else
        {
            GameAnalytics.LogWatchRewardAdsDone("item_shop", true, "false");
        }
    }

    public void BuyItemByGem() {
        if (GameData.Gem >= numberPrices) {
            GameData.Gem -= numberPrices;
            if (resourceItems[0].type == ResourceType.Character)
            {
                GameAnalytics.LogGetItem(resourceItems[0].detail.ToString(), 1, "gem", 0, 0, "HomeScene", "tab_character");
            }
            BuyComplete();
        }
        else
        {
            PanelShopController panelShop = PanelShopController.Setup();
            panelShop.Show();
            panelShop.OutOfGem();
        }
    }

    public void BuyItemByPumpkin()
    {
        if (GameData.Pumpkin >= numberPumpkin)
        {
            GameData.Pumpkin -= numberPumpkin;
            GameAnalytics.LogUseItem("Pumpkin", numberPumpkin, "pumpkin", "HomeScene", "popup_halloween");
            if (resourceItems[0].type == ResourceType.Character)
            {
                GameAnalytics.LogGetItem(resourceItems[0].detail.ToString(), 1, "pumpkin", 0, 0, "HomeScene", "popup_halloween");
                txtPumpkin.GetComponentInParent<Button>().interactable = false;
            }
            else
            {
                GameAnalytics.LogGetItem(resourceItems[0].type.ToString(), resourceItems[0].quantity, "pumpkin", 0, 0, "HomeScene", "popup_halloween");
            }
            BuyComplete();
        }
        else
        {
            PanelShopController panelShop = PanelShopController.Setup();
            panelShop.Show();
            panelShop.OutOfPumpkin();
        }
    }

    void UpdateTextCurAds() {
        txtAds.text = currentAds + "/" + numberAds;
    }
    public void ShowFail() {
        GameAnalytics.LogWatchRewardAdsDone("item_shop", true, "cant_show");
        PopupNoVideo.Setup().Show();
    }
}

