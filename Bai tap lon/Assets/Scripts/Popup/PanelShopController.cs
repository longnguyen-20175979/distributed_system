using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PanelShopController : BaseBox
{
    public static PanelShopController instance;
    [SerializeField] Text txtGem;
    [SerializeField] Text txtHeart;
    [SerializeField] Text txtBullet;
    [SerializeField] Text txtPumpkin;
    [SerializeField] GameObject panelComboPack;
    [SerializeField] GameObject panelGemPack;
    [SerializeField] GameObject panelItemPack;
    [SerializeField] GameObject panelCharacterPack;
    [SerializeField] GameObject panelPumpkinPack;
    [SerializeField] Button btnItemPack;
    [SerializeField] Button btnComboPack;
    [SerializeField] Button btnGemPack;
    [SerializeField] Button btnCharacterPack;
    [SerializeField] Button btnPumpkinPack;
    [SerializeField] GameObject btnSelectItemPack;
    [SerializeField] GameObject btnSelectComboPack;
    [SerializeField] GameObject btnSelectGemPack;
    [SerializeField] GameObject btnSelectCharacterPack;
    [SerializeField] GameObject btnSelectPumpkinPack;
    [SerializeField] Button btnSubVip, btnRemoveAds;
    [SerializeField] GameObject outOfGem, outOfPumpkin;
    [SerializeField] GameObject shopBtn;

    private string screenName;

    public static PanelShopController Setup()
    {
        if (instance == null)
        {
            instance = Instantiate(Resources.Load<PanelShopController>(Constants.PathPrefabs.PANEL_SHOP));
        }
        return instance;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        GameData.RegisterResourceChangedListener(ResourceType.Gem, SetTextGem);
        GameData.RegisterResourceChangedListener(ResourceType.Heart, SetTextHeart);
        GameData.RegisterResourceChangedListener(ResourceType.BulletNormal, SetTextBullet);
        GameData.RegisterResourceChangedListener(ResourceType.Pumpkin, SetTextPumpkin);
        SetTextGem();
        SetTextHeart();
        SetTextBullet();
        SetTextPumpkin();
        if (GameData.vip)
        {
            DisableBtnSubVip(null);
        }
        if (GameData.removeAds)
        {
            DisableBtnRemoveAds(null);
        }
        if (GameData.inGame)
        {
            Time.timeScale = 0;
        }
        outOfGem.SetActive(false);
        outOfPumpkin.SetActive(false);
        this.RegisterListener(EventID.BuyVip, DisableBtnSubVip);
        this.RegisterListener(EventID.BuyNoads, DisableBtnRemoveAds);
        screenName = SceneManager.GetActiveScene().name;
        if (screenName == Constants.SCENE_NAME.SCENE_GAMEPLAY)
        {
            btnCharacterPack.gameObject.SetActive(false);
            shopBtn.GetComponent<HorizontalLayoutGroup>().spacing = -190;
        }
        else
        {
            btnCharacterPack.gameObject.SetActive(true);
            shopBtn.GetComponent<HorizontalLayoutGroup>().spacing = -55;
        }
    }
    public void DisableBtnSubVip(object a)
    {
        btnSubVip.interactable = false;
        DisableBtnRemoveAds(null);
    }
    public void DisableBtnRemoveAds(object a)
    {
        btnRemoveAds.interactable = false;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        if (GameData.inGame)
        {
            Time.timeScale = 1;
        }
        GameData.RemoveResourceChangedListener(ResourceType.Gem, SetTextGem);
        GameData.RemoveResourceChangedListener(ResourceType.Heart, SetTextHeart);
        GameData.RemoveResourceChangedListener(ResourceType.BulletNormal, SetTextBullet);
        GameData.RemoveResourceChangedListener(ResourceType.Pumpkin, SetTextPumpkin);
        this.RemoveListener(EventID.BuyNoads, DisableBtnRemoveAds);
        this.RemoveListener(EventID.BuyVip, DisableBtnSubVip);
    }
    void SetTextGem()
    {
        txtGem.text = GameData.Gem.ToString();
    }
    void SetTextHeart()
    {
        txtHeart.text = GameData.Heart.ToString();
    }
    void SetTextBullet()
    {
        txtBullet.text = GameData.Bullet.ToString();
    }
    void SetTextPumpkin()
    {
        txtPumpkin.text = GameData.Pumpkin.ToString();
    }
    public void SelectTapPack(GameObject panel)
    {
        panelComboPack.SetActive(false);
        panelGemPack.SetActive(false);
        panelItemPack.SetActive(false);
        panelCharacterPack.SetActive(false);
        panelPumpkinPack.SetActive(false);
        panel.SetActive(true);
    }
    public void TapButtonPack(Button btn)
    {
        btnComboPack.interactable = true;
        btnGemPack.interactable = true;
        btnItemPack.interactable = true;
        btnCharacterPack.interactable = true;
        btnPumpkinPack.interactable = true;
        btn.interactable = false;
    }
    public void TapButtonSelect(GameObject btn)
    {
        btnSelectComboPack.SetActive(false);
        btnSelectGemPack.SetActive(false);
        btnSelectItemPack.SetActive(false);
        btnSelectCharacterPack.SetActive(false);
        btnSelectPumpkinPack.SetActive(false);
        btn.SetActive(true);
    }
    public void ShowTapCombo()
    {
        GameAnalytics.LogUIAppear("tab_combo", screenName);
        SelectTapPack(panelComboPack);
        TapButtonPack(btnComboPack);
        TapButtonSelect(btnSelectComboPack);
    }
    public void ShowTapGem()
    {
        GameAnalytics.LogUIAppear("tab_gem", screenName);
        SelectTapPack(panelGemPack);
        TapButtonPack(btnGemPack);
        TapButtonSelect(btnSelectGemPack);
    }
    public void ShowTapItem()
    {
        GameAnalytics.LogUIAppear("tab_item", screenName);
        SelectTapPack(panelItemPack);
        TapButtonPack(btnItemPack);
        TapButtonSelect(btnSelectItemPack);
    }
    public void ShowTabCharacter()
    {
        GameAnalytics.LogUIAppear("tab_character", screenName);
        SelectTapPack(panelCharacterPack);
        TapButtonPack(btnCharacterPack);
        TapButtonSelect(btnSelectCharacterPack);
    }
    public void ShowTabPumpkin()
    {
        GameAnalytics.LogUIAppear("tab_pumpkin", screenName);
        SelectTapPack(panelPumpkinPack);
        TapButtonPack(btnPumpkinPack);
        TapButtonSelect(btnSelectPumpkinPack);
    }
    public void ShowVip()
    {       
        GameAnalytics.LogUIAppear("popup_vip_package_shop", screenName);
        UIVipController.Setup().Show();
    }
    public void OutOfGem()
    {
        outOfGem.GetComponent<Animator>().Rebind();
        outOfGem.SetActive(true);
        ShowTapGem();
    }
    public void OutOfPumpkin()
    {
        outOfPumpkin.GetComponent<Animator>().Rebind();
        outOfPumpkin.SetActive(true);
        ShowTabPumpkin();
    }
}
