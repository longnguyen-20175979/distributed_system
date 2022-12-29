using DarkTonic.MasterAudio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.NiceVibrations;

public class PopupPause : BaseBox {
    public static PopupPause instance;

    [SerializeField]
    private SwitchToggle toggleMusic;

    [SerializeField]
    private SwitchToggle toggleSound;

    [SerializeField]
    private SwitchToggle toggleVibration;
    [SerializeField] Text txtLevel;
    [SerializeField] private Button restoreBtn;

    private void Start() {
        toggleMusic.OnSwitch(!(PersistentAudioSettings.MusicMuted != null ? PersistentAudioSettings.MusicMuted.Value : false));
        toggleSound.OnSwitch(!(PersistentAudioSettings.MixerMuted != null ? PersistentAudioSettings.MixerMuted.Value : false));
        toggleVibration.OnSwitch(GameData.IsVibrateEnabled);
        //holderTextLevel.SetStringValues(GameData.selectedLevel);
    }
    public static PopupPause Setup() {
        if (instance == null) {
            instance = Instantiate(Resources.Load<PopupPause>(Constants.PathPrefabs.POPUP_PAUSE));
        }
        return instance;
    }
    protected override void OnEnable() {
        base.OnEnable();
        Time.timeScale = 0;
        txtLevel.text = "LEVEL " + GameData.levelSelected;
#if UNITY_ANDROID 
        restoreBtn.interactable = false;
#endif
    }
    public override void Close() {
        base.Close();
        Time.timeScale = 1;
        GameAnalytics.LogButtonClick("resume", "popup_pause");
    }
    public void RestartGame() {
        GameController.instance.RestartGame();
        GameAnalytics.LogFirebaseUserProperty("total_heart", GameData.realHealth);
        GameAnalytics.LogFirebaseUserProperty("total_acorn", GameData.Bullet);
        GameAnalytics.LogLevelEnd(GameData.levelSelected, false, GameData.levelPlayedTime, GameData.levelPlayedTime, GameData.SelectedCharacter.ToString(), "restart_from_pause", GameData.bulletUse, GameData.heartLose);
        GameAnalytics.LogLevelStart(GameData.levelSelected, "gameplay", "flint", true);
        GameAnalytics.LogButtonClick("restart", "popup_pause");
    }
    public void BackHome() {
        GameAnalytics.LogFirebaseUserProperty("total_heart", GameData.realHealth);
        GameAnalytics.LogFirebaseUserProperty("total_acorn", GameData.Bullet);
        GameData.curScore = 0;
        GameData.realHealth = Constants.START_HEALTH;
        GameData.curScore = 0;
        Time.timeScale = 1;
        GameData.isRevive = false;
        GameData.freeRevive = false;
        GameData.posRevive = Vector3.zero;
        Initiate.Fade(Constants.SCENE_NAME.SCENE_HOME, Color.black, 1.5f);
        GameAnalytics.LogLevelEnd(GameData.levelSelected, false, GameData.levelPlayedTime, GameData.levelPlayedTime, GameData.SelectedCharacter.ToString(), "back_to_home_from_pause", GameData.bulletUse, GameData.heartLose);
        GameData.levelPlayedTime = 0;
        GameAnalytics.LogButtonClick("back_to_home", "popup_pause");
    }
    public void OnToggleMusicChanged(bool value) {
        PersistentAudioSettings.MusicMuted = !value;
    }
    public void OnToggleSoundChanged(bool value) {
        PersistentAudioSettings.MixerMuted = !value;
    }
    public void OnToggleVibrateChanged(bool status)
    {
        GameData.IsVibrateEnabled = status;
    }
    public void OpenURLTermOfUse() {
        Application.OpenURL("https://commandoo247.blogspot.com/2021/08/term-of-use-commandoo.html");
        GameAnalytics.LogButtonClick("term", "popup_pause");
    }
    public void OpenURLPrivacy() {
        Application.OpenURL("https://commandoo247.blogspot.com/2021/08/privacy.html");
        GameAnalytics.LogButtonClick("privacy", "popup_pause");
    }
}
