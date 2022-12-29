using DarkTonic.MasterAudio;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupComplete : BaseBox
{
    [SerializeField] Text txtScore;
    [SerializeField] Text txtGem;
    [SerializeField] Text txtReward;
    public static PopupComplete instance;
    [SerializeField] Image[] stars;
    [SerializeField] Material gray;
    [SerializeField] Text txtLevelComplete;
    [SerializeField] int reward;
    [SerializeField] Button btnGetReward;
    [SerializeField] GameObject animAddGem;
    public static PopupComplete Setup()
    {
        if (instance == null)
        {
            instance = Instantiate(Resources.Load<PopupComplete>(Constants.PathPrefabs.POPUP_COMPLETE));
        }
        instance.Init();
        return instance;
    }
    private void Init()
    {
        StartCoroutine(CoroutineNumber(GameController.instance.score, txtScore));
        txtGem.text = GameData.Gem.ToString();
        txtLevelComplete.text = "LEVEL " + GameData.levelSelected;
        int starLevel = LevelController.instance.StarLevel(GameController.instance.score);
        GameData.SetStarForLevel(GameData.levelSelected, starLevel);
        GameData.isLevelPassed = true;
        GameData.curScore = 0;
        GameData.isRevive = false;
        GameData.freeRevive = false;
        GameData.posRevive = Vector3.zero;
        txtReward.text = "+" + reward.ToString();
        if (GameData.levelSelected == GameData.LevelUnlock)
        {
            GameData.LevelUnlock++;
        }
        animAddGem.SetActive(false);
        MasterAudio.PlaySound(Constants.Audio.SOUND_POPUP_WIN);
    }
    IEnumerator CoroutineNumber(int number, Text text)
    {
        float timer;
        if (number >= 30)
            timer = 1.5f;
        else
            timer = 0.5f;
        float value = 0;
        float countTime = timer / 0.05f;
        while (value < number)
        {
            value += ((float)number / countTime);
            text.text = ((int)value).ToString();
            int starLevel1 = LevelController.instance.StarLevel((int)value);
            SetStar(starLevel1);
            yield return new WaitForSeconds(0.05f);
        }
        int starLevel = LevelController.instance.StarLevel(number);
        SetStar(starLevel);
        text.text = number.ToString();
    }
    public void NextLevel()
    {
        if (GameData.levelSelected >= Constants.MAX_UNLOCK_LEVEL)
        {
            PopupComingSoon popup = PopupComingSoon.Setup();
            popup.OnCloseBox = OnComingSoonClose;
            popup.Show();
            return;
        }
        Initiate.Fade(Constants.SCENE_NAME.SCENE_HOME, Color.black, 1.5f);
        if (GameData.levelSelected + 1 == GameData.LevelUnlock)
        {
            GameData.isNextLevel = true;
        }
        GameData.levelPlayedTime = 0;
    }

    private void OnComingSoonClose()
    {
        Initiate.Fade(Constants.SCENE_NAME.SCENE_HOME, Color.black, 1.5f);
    }

    public void SetStar(int star)
    {
        for (int i = 0; i < stars.Length; i++)
        {
            if (i < star)
            {
                stars[i].material = null;
            }
            else
            {
                stars[i].material = gray;
            }
        }
    }
    public void RestartGame()
    {
        GameController.instance.RestartGame();
        GameData.levelPlayedTime = 0;
        GameAnalytics.LogLevelStart(GameData.levelSelected, "gameplay", "flint", true);
        Debug.Log(GameData.levelSelected <= GameData.LevelUnlock ? true : false);
    }
    public void GetGem(bool success)
    {
        if (success)
        {
            btnGetReward.interactable = false;
            GameData.Gem += reward;
            GameData.GemReceived += reward;
            animAddGem.GetComponentInChildren<Text>().text = "+" + reward.ToString();
            animAddGem.SetActive(true);
            animAddGem.GetComponent<Animator>().Rebind();
            animAddGem.transform.position = btnGetReward.transform.position;
            animAddGem.transform.DOMoveY(txtGem.transform.position.y, 0.8f).OnComplete(() => StartCoroutine(SetTextGem(reward)));
            GameAnalytics.LogWatchRewardAdsDone("gem_complete", true, "true");
            GameAnalytics.LogGetItem("Gem", reward, "ads", 1, 1, "GamePlay", "popup_complete");
        }
        else
        {
            GameAnalytics.LogWatchRewardAdsDone("gem_complete", true, "false");
        }
    }

    IEnumerator SetTextGem(int reward)
    {
        float number = 0;
        animAddGem.SetActive(false);
        while (number < reward)
        {
            number += 1;
            yield return new WaitForSeconds(0.08f);
            txtGem.text = ((GameData.Gem - reward) + number).ToString();
        }
        txtGem.text = GameData.Gem.ToString();
    }
    public void ShowFail()
    {
        GameAnalytics.LogWatchRewardAdsDone("gem_complete", true, "cant_show");
        PopupNoVideo.Setup().Show();
    }
}
