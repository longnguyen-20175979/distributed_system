using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DarkTonic.MasterAudio;

public class ItemTabController : MonoBehaviour
{
    public static ItemTabController instance;

    [SerializeField] private RectTransform item;
    [SerializeField] private RectTransform arrow;
    [SerializeField] private Ease ease;
    private bool isShowing;
    private Vector3 originPosItem, originPosArrow;

    private float timeCount;

    public GameObject shieldIcon;
    [SerializeField] private Button shieldBtn;
    public GameObject magnetIcon;
    [SerializeField] private Button magnetBtn;

    [SerializeField] private Sprite bulletGem, bulletAds;
    [SerializeField] private Image bulletAdsTabItem;

    [SerializeField] private ShowRewardedController rewardedController;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }    
    }

    private void Start()
    {
        originPosItem = item.anchoredPosition;
        originPosArrow = arrow.anchoredPosition;
        Show();
        if (GameData.levelSelected % 20 == 0)
        {
            shieldBtn.interactable = false;
            magnetBtn.interactable = false;
        }
    }

    private void Update()
    {
        if (isShowing)
        {
            timeCount += Time.deltaTime;
            if (timeCount >= 5)
            {
                timeCount = 0;
                Hide();
            }
        }
        if (GameData.Gem >= 1)
        {
            ChangeImgBulletGem();
        }
        else ChangeImgBulletAds();
    }

    public void ShowOrHide()
    {
        if (!isShowing)
        {
            GameAnalytics.LogButtonClick("tab_item_open", "tab_item");
            Show();
        }
        else
        {
            GameAnalytics.LogButtonClick("tab_item_close", "tab_item");
            Hide();
        }
    }

    private void Show()
    {
        isShowing = true;
        timeCount = 0;
        item.DOAnchorPosX(originPosItem.x + 255, 0.5f).SetEase(ease);
        arrow.DOAnchorPosX(originPosArrow.x + 140, 0.5f).SetEase(ease);
        arrow.DOLocalRotate(new Vector3(0, 0, 180), 0.5f);       
    }

    private void Hide()
    {
        isShowing = false;
        timeCount = 0;
        item.DOAnchorPosX(originPosItem.x, 0.5f).SetEase(ease);
        arrow.DOAnchorPosX(originPosArrow.x, 0.5f).SetEase(ease);
        arrow.DOLocalRotate(Vector3.zero, 0.5f);
    }

    public void GetShield(bool success)
    {
        GameAnalytics.LogItemTab("shield", "ads", GameData.levelSelected);
        if (success)
        {
            if (PlayerMovement.instance.isProtected)
            {
                GameObject lastShield = GameObject.FindGameObjectWithTag(Constants.TAG.SHIELD);
                Destroy(lastShield);
                PlayerMovement.instance.isProtected = false;
            }
            
            GameObject shield = Instantiate(Resources.Load<GameObject>("Items/Shield"));
            shield.GetComponent<SpriteRenderer>().enabled = false;
            shield.transform.position = PlayerMovement.instance.transform.position;
            StartCoroutine(Helper.StartAction(() =>
            {
                shield.GetComponent<SpriteRenderer>().enabled = true;
            }, 0.2f));
            StartCoroutine(Helper.StartAction(() =>
            {
                shieldBtn.interactable = true;
            }, Constants.SHIELD_DURATION));
            shieldIcon.SetActive(true);
            shieldBtn.interactable = false;
            GameAnalytics.LogWatchRewardAdsDone("shield_gameplay", true, "true");
            GameAnalytics.LogGetItem("Shield", 1, "ads", 1, 1, "GamePlay", "tab_item");
            GameAnalytics.LogUseItem("Shield", 1, "ads", "GamePlay", "tab_item");
        }
        else GameAnalytics.LogWatchRewardAdsDone("shield_gameplay", true, "false");
    }

    public void ShowFailShield()
    {
        GameAnalytics.LogWatchRewardAdsDone("shield_gameplay", true, "cant_show");
        UIGameController.instance.noVideo.Rebind();
        UIGameController.instance.noVideo.gameObject.SetActive(true);
    }

    public void GetMagnet(bool success)
    {
        GameAnalytics.LogItemTab("magnet", "ads", GameData.levelSelected);
        if (success)
        {
            GameObject magnet = Instantiate(Resources.Load<GameObject>("Items/Magnet"));
            magnet.GetComponent<SpriteRenderer>().enabled = false;
            magnet.transform.position = PlayerMovement.instance.transform.position;
            StartCoroutine(Helper.StartAction(() => magnet.GetComponent<SpriteRenderer>().enabled = true, 0.2f));
            StartCoroutine(Helper.StartAction(() =>
            {
                magnetBtn.interactable = true;
            }, Constants.MAGNET_DURATION));
            magnetIcon.SetActive(true);
            magnetBtn.interactable = false;
            GameAnalytics.LogWatchRewardAdsDone("magnet_gameplay", true, "true");
            GameAnalytics.LogGetItem("Magnet", 1, "ads", 1, 1, "GamePlay", "tab_item");
            GameAnalytics.LogUseItem("Magnet", 1, "ads", "GamePlay", "tab_item");
        }
        else GameAnalytics.LogWatchRewardAdsDone("magnet_gameplay", true, "false");
    }

    public void ShowFailMagnet()
    {
        GameAnalytics.LogWatchRewardAdsDone("magnet_gameplay", true, "cant_show");
        UIGameController.instance.noVideo.Rebind();
        UIGameController.instance.noVideo.gameObject.SetActive(true);
    }

    public void BuyBulletByGem()
    {
        if (GameData.Gem >= 1)
        {
            GameAnalytics.LogButtonClick("tab_item_bullet", "tab_item");
            GameAnalytics.LogItemTab("bullet", "gem", GameData.levelSelected);
            GameAnalytics.LogUseItem("Gem", 1, "gem", "GamePlay", "tab_item");
            timeCount = 0;
            GameData.Gem--;
            GameData.Bullet += 3;
            GameData.BulletReceived += 3;
            GameAnalytics.LogGetItem("Bullet", 3, "gem", 0, 0, "GamePlay", "tab_item");
            GameAnalytics.LogUseItem("Bullet", 3, "gem", "GamePlay", "tab_item");
            UIGameController.instance.SetTextBullet();
            UIGameController.instance.addBulletAds.Rebind();
            UIGameController.instance.addBulletAds.gameObject.SetActive(true);
        }
        else
        {
            GameAnalytics.LogButtonClick("tab_item_bullet", "tab_item");
            GameAnalytics.LogItemTab("bullet", "ads", GameData.levelSelected);
            rewardedController.Show();
        }
    }

    public void GetBullet(bool success) 
    {
        if (success)
        {
            GameData.Bullet += 3;
            GameData.BulletReceived += 3;
            GameAnalytics.LogGetItem("Bullet", 3, "ads", 1, 1, "GamePlay", "tab_item");
            GameAnalytics.LogUseItem("Bullet", 3, "gem", "GamePlay", "tab_item");
            UIGameController.instance.SetTextBullet();
            MasterAudio.PlaySound(Constants.Audio.SOUND_COLLECT_ITEM);
            UIGameController.instance.addBulletAds.Rebind();
            UIGameController.instance.addBulletAds.gameObject.SetActive(true);
            GameAnalytics.LogWatchRewardAdsDone("bullet_ads_tab_item", true, "true");
        }
        else GameAnalytics.LogWatchRewardAdsDone("bullet_ads_tab_item", true, "false");
    }

    public void ShowFailBulletTabItem()
    {
        GameAnalytics.LogWatchRewardAdsDone("bullet_ads_tab_item", true, "cant_show");
        UIGameController.instance.noVideo.Rebind();
        UIGameController.instance.noVideo.gameObject.SetActive(true);
    }

    public void ChangeImgBulletAds()
    {
        bulletAdsTabItem.sprite = bulletAds;
    }

    public void ChangeImgBulletGem()
    {
        bulletAdsTabItem.sprite = bulletGem;
    }
}
