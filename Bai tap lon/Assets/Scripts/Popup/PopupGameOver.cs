using DarkTonic.MasterAudio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PopupGameOver : BaseBox {
    public static PopupGameOver instance;
    [SerializeField] Text txtGem;
    [SerializeField] Text txtReward;
    [SerializeField] Button btnGet;
    [SerializeField] Text txtLevel;
    [SerializeField] int reward;
    [SerializeField] ClaimCoinFx claim;
     public static PopupGameOver Setup() {
        if (instance == null) {
            instance = Instantiate(Resources.Load<PopupGameOver>(Constants.PathPrefabs.POPUP_GAMEOVER));
        }
        instance.InitPopup();
        return instance;
    }
    void InitPopup() {
        GameData.isRevive = false;
        GameData.freeRevive = false;
        GameData.isLevelPassed = false;
        txtGem.text = GameData.Gem.ToString();
        txtLevel.text = "Level " + GameData.levelSelected;
        txtReward.text = "+" + reward.ToString();
        MasterAudio.PlaySound(Constants.Audio.SOUND_GAME_OVER);
        GameAnalytics.LogUIAppear("popup_game_over", "GamePlay");
    }
    public void GetGem(bool success)
    {
        if (success)
        {
            btnGet.interactable = false;
            GameData.Gem += reward;
            GameData.GemReceived += reward;
            ClaimCoinFx coinFx = Instantiate(claim, btnGet.transform);
            coinFx.transform.position = btnGet.transform.position;
            coinFx.PlayCoin(txtGem.transform);
            StartCoroutine(SetTextGem(reward));
            GameAnalytics.LogWatchRewardAdsDone("gem_game_over", true, "true");
            GameAnalytics.LogGetItem("Gem", reward, "ads", 1, 1, "GamePlay", "popup_game_over");
        }
        else
        {
            GameAnalytics.LogWatchRewardAdsDone("gem_game_over", true, "false");
        }
    }

    public void Restart() {
        GameController.instance.RestartGame();
        GameAnalytics.LogLevelEnd(GameData.levelSelected, false, GameData.levelPlayedTime, GameData.levelPlayedTime, GameData.SelectedCharacter.ToString(), "die", GameData.bulletUse, GameData.heartLose);
        GameAnalytics.LogLevelStart(GameData.levelSelected, "gameplay", "flint", true);
        GameAnalytics.LogButtonClick("restart", "popup_game_over");
    }
    public void Home() {
        GameAnalytics.LogLevelEnd(GameData.levelSelected, false, GameData.levelPlayedTime, GameData.levelPlayedTime, GameData.SelectedCharacter.ToString(), "die", GameData.bulletUse, GameData.heartLose);
        GameAnalytics.LogButtonClick("back_to_home", "popup_game_over");
        GameData.curScore = 0;
        Initiate.Fade(Constants.SCENE_NAME.SCENE_HOME, Color.black, 1.5f);
    }
    IEnumerator SetTextGem(int reward) {
        float number = 0;
        while (number < reward) {
            number += 1;
            yield return new WaitForSeconds(0.15f);
            txtGem.text = ((GameData.Gem - reward) + number).ToString();
        }
        txtGem.text = GameData.Gem.ToString();
    }
    public void ShowFail() {
        GameAnalytics.LogWatchRewardAdsDone("gem_game_over", true, "cant_show");
        PopupNoVideo.Setup().Show();
    }
}
