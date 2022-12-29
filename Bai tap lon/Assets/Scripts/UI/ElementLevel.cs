using DarkTonic.MasterAudio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementLevel : MonoBehaviour {
    [SerializeField] Text txtLevel;
    [SerializeField] Image[] stars;
    [SerializeField] Material gray;
    int level;
    [SerializeField] Sprite normal, hard;
    [SerializeField] Image choiceLevel;
    public void InitLevel(int level, int star) {
        txtLevel.text = level.ToString();
        SetStar(star);
        this.level = level;
        if (level < GameData.LevelUnlock)
            EnableStar(true);
        else
            EnableStar(false);
        if (level % 20 == 0) {
            choiceLevel.sprite = hard;
        } else {
            choiceLevel.sprite = normal;
        }
        if (level > GameData.LevelUnlock) {
#if ENV_PROD
            choiceLevel.GetComponent<Button>().interactable = false;
#endif
        }
    }
    public void SetStar(int star) {
        for (int i = 0; i < stars.Length; i++) {
            if (i < star) {
                stars[i].material = null;
            } else {
                stars[i].material = gray;
            }
        }
    }
    void EnableStar(bool enable) {
        for (int i = 0; i < stars.Length; i++) {
            stars[i].gameObject.SetActive(enable);
        }
    }
    public void SelectLevel() {
        if (level > Constants.MAX_UNLOCK_LEVEL)
        {
            PopupComingSoon popup = PopupComingSoon.Setup();
            popup.OnCloseBox = null;
            popup.Show();
            return;
        }
        GameData.levelSelected = level;
        PopupStartLevel.Setup().Show();
        MasterAudio.PlaySound(Constants.Audio.TAB_BUTTON);
        GameAnalytics.LogButtonClick("level_button", "HomeScene");
    }
}
