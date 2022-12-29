using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceItemHalloween : MonoBehaviour
{
    public ResourceItem[] resourceItems;

    [SerializeField] private GameObject lighting;
    [SerializeField] private string idPack;
    [SerializeField] private int minRange;
    [SerializeField] private Image border;
    [SerializeField] private Sprite inActive;

    public void Claim()
    {
        for (int i = 0; i < resourceItems.Length; i++)
        {
            if (resourceItems[i].countable)
            {
                GameAnalytics.LogGetItem(resourceItems[i].type.ToString(), resourceItems[i].quantity, "halloween_event", 0, 0, "HomeScene", "popup_halloween");
            }
            else
            {
                GameAnalytics.LogGetItem(resourceItems[i].detail.ToString(), 1, "halloween_event", 0, 0, "HomeScene", "popup_halloween");
            }
        }
        GameData.ClaimResourceItems(resourceItems);
        UIClaimResourcePanel.Setup(resourceItems).Show();
        GameData.SetHalloweenGiftStatus(idPack, true);
        UpdateBtnStatus();
    }

    internal void UpdateBtnStatus()
    {
        bool canCollect = GameData.PumpkinReceived >= minRange;
        if (GameData.GetHalloweenGiftStatus(idPack))
        {
            GetComponent<Button>().interactable = false;
            border.sprite = inActive;
            lighting.SetActive(false);
        }
        else
        {
            if (canCollect)
            {
                lighting.SetActive(true);
                GetComponent<Button>().interactable = true;
            }
            else
            {
                lighting.SetActive(false);
                GetComponent<Button>().interactable = false;
            }
        }
    }
}
