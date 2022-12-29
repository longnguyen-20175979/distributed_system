using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LogBuyInShop : MonoBehaviour
{
    [SerializeField] string tabName;
    [SerializeField] string itemName;
    [SerializeField] string currency;
    private int watchedAds;
    private int totalAds;
    private int price;
    private string screenName;
    [SerializeField] private bool buyByGem;
    [SerializeField] private bool iAP;
    private ResourceItemShop resourceItemShop, myResourceItemShop;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(LogBuyBtnInShop);
        if (!iAP && transform.parent.GetComponent<ResourceItemShop>() != null)
        {
            resourceItemShop = transform.parent.GetComponent<ResourceItemShop>();
            totalAds = resourceItemShop.numberAds;
            price = resourceItemShop.numberPrices;
        }
        myResourceItemShop = GetComponent<ResourceItemShop>();       
    }

    private void LogBuyBtnInShop()
    {
        screenName = SceneManager.GetActiveScene().name;
        if (!iAP)
        {
            if (!buyByGem)
            {
                watchedAds++;
                if (watchedAds > totalAds)
                {
                    watchedAds = watchedAds - totalAds;
                }
                if (watchedAds == totalAds && resourceItemShop.resourceItems[0].type == ResourceType.BulletNormal)
                {
                    GameAnalytics.LogUseItem("Bullet", resourceItemShop.resourceItems[0].quantity, "ads", screenName, tabName);
                }
                GameAnalytics.LogBuyInShop(tabName, itemName, currency, watchedAds, totalAds, screenName, 0);               
                if (myResourceItemShop != null)
                {
                    GameAnalytics.LogGetItem(itemName, myResourceItemShop.resourceItems[0].quantity, currency, watchedAds, totalAds, screenName, tabName);
                }
                else GameAnalytics.LogGetItem(itemName, resourceItemShop.resourceItems[0].quantity, currency, watchedAds, totalAds, screenName, tabName);
                if (resourceItemShop != null && resourceItemShop.resourceItems.Length > 1)
                {
                    GameAnalytics.LogGetItem(itemName, resourceItemShop.resourceItems[1].quantity, currency, watchedAds, totalAds, screenName, tabName);
                }
            }
            else
            {
                GameAnalytics.LogBuyInShop(tabName, itemName, currency, 0, 0, screenName, price);
                GameAnalytics.LogGetItem(itemName, resourceItemShop.resourceItems[0].quantity, currency, 0, 0, screenName, tabName);
                GameAnalytics.LogUseItem("Gem", resourceItemShop.numberPrices, "gem", screenName, tabName);
                if (resourceItemShop.resourceItems.Length > 1)
                {
                    GameAnalytics.LogGetItem(itemName, resourceItemShop.resourceItems[1].quantity, currency, 0, 0, screenName, tabName);
                }
                if (resourceItemShop.resourceItems[0].type == ResourceType.BulletNormal)
                {
                    GameAnalytics.LogUseItem("Bullet", resourceItemShop.resourceItems[0].quantity, "gem", screenName, tabName);
                }
            }
        }
        else
        {
            GameAnalytics.LogGetItem(itemName, myResourceItemShop.resourceItems[0].quantity, currency, 0, 0, screenName, tabName);
        }
    }
}
