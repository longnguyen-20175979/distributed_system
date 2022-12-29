using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinShopController : ResourceItemShop
{
    [SerializeField] private GameObject btnGroup;
    [SerializeField] private GameObject selectBtn;
    [SerializeField] private GameObject iAPBtn;
    [SerializeField] private GameObject chain;
    [SerializeField] private bool isIAP;

    [SerializeField] private Sprite greenBtnSprite;
    [SerializeField] private Sprite yellowBtnSprite;

    public override void OnEnable()
    {
        base.OnEnable();      
        if (isIAP)
        {
            btnGroup.SetActive(false);
        }       
        UpdateBtnStatus();
    }

    public override void BuyComplete()
    {
        base.BuyComplete();       
        UpdateBtnStatus();
    }

    public void Select()
    {
        selectBtn.SetActive(true);
        selectBtn.GetComponentInChildren<Text>().text = "Selected";
        selectBtn.GetComponentInChildren<Outline>().enabled = false;
        selectBtn.GetComponent<Image>().sprite = yellowBtnSprite;
        GameData.SelectedCharacter = resourceItems[0].detail;
        ShopCharacterController.instance.UpdateBtnDisplay();
        GameAnalytics.LogUseItem(resourceItems[0].detail.ToString(), 1, null, "HomeScene", "tab_character");
    }

    public void UpdateBtnStatus()
    {
        bool hasCharacter = GameData.HasCharacter(resourceItems[0].detail);
        bool isSelecting = GameData.SelectedCharacter == resourceItems[0].detail;
        if (hasCharacter)
        {
            chain.SetActive(false);
            btnGroup.SetActive(false);
            iAPBtn.SetActive(false);
            if (isSelecting)
            {
                selectBtn.SetActive(true);
                selectBtn.GetComponentInChildren<Text>().text = "Selected";
                selectBtn.GetComponentInChildren<Outline>().enabled = false;
                selectBtn.GetComponent<Image>().sprite = yellowBtnSprite;
            }
            else
            {
                selectBtn.SetActive(true);
                selectBtn.GetComponentInChildren<Text>().text = "Select";
                selectBtn.GetComponentInChildren<Outline>().enabled = true;
                selectBtn.GetComponent<Image>().sprite = greenBtnSprite;
            }
        }
        else if (!hasCharacter && !isIAP)
        {
            btnGroup.SetActive(true);
            selectBtn.SetActive(false);
            chain.SetActive(true);
        }
        else if (!hasCharacter && isIAP)
        {
            iAPBtn.SetActive(true);
            chain.SetActive(true);
        }
    }   
}
