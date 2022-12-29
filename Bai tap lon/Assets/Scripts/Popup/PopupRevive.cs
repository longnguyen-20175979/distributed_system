using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupRevive : BaseBox
{
    [SerializeField] Text txtTimer;
    [SerializeField] Slider slider;
    public static PopupRevive instance;
    [SerializeField] int totalTime;
    [SerializeField] Text txtRevive;
    [SerializeField] GameObject txtFreeRevive;
    [SerializeField] GameObject icon;
    [SerializeField] private Text txtCancel;
    [SerializeField] private Image headPlayer;
    public static PopupRevive Setup()
    {
        if (instance == null)
        {
            instance = Instantiate(Resources.Load<PopupRevive>(Constants.PathPrefabs.POPUP_REVIVE));
        }
        instance.Init();
        return instance;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GameAnalytics.LogUIAppear("popup_revive", "GamePlay");
        txtCancel.GetComponent<Button>().interactable = false;
        txtCancel.color = Vector4.zero;
        headPlayer.sprite = DataHolder.Instance.characters[GameData.SelectedCharacter].headSprite;
        StartCoroutine(ShowBtnCancel());
        StartCoroutine(Helper.StartAction(() => txtCancel.GetComponent<Button>().interactable = true, 2.2f));
    }

    private IEnumerator ShowBtnCancel()
    {
        yield return new WaitForSeconds(1.85f);
        float alpha = 0;
        while (alpha <= 0.7f)
        {
            alpha += 0.1f;
            txtCancel.color = new Color(txtCancel.color.r, txtCancel.color.g, txtCancel.color.b, alpha);
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void Init()
    {
        StartCoroutine(CountTimeRevive());
        slider.value = GameController.instance.progressPercent;
        if (GameData.vip && !GameData.freeRevive)
        {
            icon.SetActive(false);
            txtRevive.gameObject.SetActive(false);
            txtFreeRevive.SetActive(true);
        }
        else
        {
            icon.SetActive(true);
            txtRevive.gameObject.SetActive(true);
            txtFreeRevive.SetActive(false);
        }
    }

    IEnumerator CountTimeRevive()
    {
        while (totalTime >= 0)
        {
            txtTimer.text = totalTime.ToString();
            yield return new WaitForSecondsRealtime(1);
            totalTime--;
        }
        CancelRevive();
    }
    public void Revive()
    {       
        if (GameData.vip && !GameData.freeRevive)
        {
            GameAnalytics.LogButtonClick("revive", "popup_revive");
            GameData.freeRevive = true;
            GameAnalytics.LogRevive(GameData.levelSelected, "free_revive", "flint");
            ReviveSucess();
        }
        else
        {
            GetComponent<ShowRewardedController>().Show();
        }
    }
    public void GetRevive(bool success)
    {
        if (success)
        {
            GameAnalytics.LogRevive(GameData.levelSelected, "watch_ads", "flint");
            GameAnalytics.LogWatchRewardAdsDone("revive", true, "true");
            ReviveSucess();
        }
        else
        {
            GameAnalytics.LogWatchRewardAdsDone("revive", true, "false");
        }
    }
    void ReviveSucess()
    {
        GameAnalytics.LogFirebaseUserProperty("total_heart", GameData.realHealth);
        GameAnalytics.LogFirebaseUserProperty("total_acorn", GameData.Bullet);
        GameData.curScore = GameController.instance.score;
        GameData.isRevive = true;
        if (GameData.levelSelected % 20 == 0)
        {
            GameData.realHealth = 2;
        }
        else GameData.realHealth = 1;   
        Initiate.Fade(Constants.SCENE_NAME.SCENE_GAMEPLAY, Color.black, 1.5f);
    }
    public void CancelRevive()
    {
        GameAnalytics.LogFirebaseUserProperty("total_heart", GameData.realHealth);
        GameData.curScore = 0;
        GameData.realHealth = Constants.START_HEALTH;
        PopupGameOver.Setup().Show();
        GameAnalytics.LogButtonClick("no_thanks", "popup_revive");
    }
    public void ShowFail()
    {
        GameAnalytics.LogWatchRewardAdsDone("revive", true, "cant_show");
        PopupNoVideo.Setup().Show();
    }
}
