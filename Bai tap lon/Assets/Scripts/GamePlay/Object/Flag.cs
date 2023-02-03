using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DarkTonic.MasterAudio;

public class Flag : MonoBehaviour {
    [SerializeField] private Transform flag;
    [SerializeField] private float duration;
    [SerializeField] private BoxCollider2D rootCol;
    [SerializeField] private Transform checkPoint;
    [SerializeField] private Transform checkPoint1;
    [SerializeField] private Transform checkPoint2;
    [SerializeField] private bool hitFirstTime = true;

    private SpriteRenderer mySprite;
    private float distance;

    private void Start() {
        mySprite = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag(Constants.TAG.PLAYER) && hitFirstTime) {
            hitFirstTime = false;
            PlayerMovement.instance.rb.velocity = Vector2.zero;
            flag.DOMoveY(checkPoint1.position.y, duration);
            MasterAudio.PlaySound(Constants.Audio.SOUND_FLAG);
            UIGameController.instance.EnableBtnControll(false);
            rootCol.isTrigger = true;
            PlayerMovement.instance.OnFlagEnter();
            PlayerMovement.instance.transform.DOMoveY(checkPoint.position.y, 1f).OnComplete(() => {
                PlayerMovement.instance.groundCheck.enabled = true;
                PlayerMovement.instance.playerAction.FinishLevel();
            });
            GameController.instance.finishLevel = true;
            distance = 2 * Vector2.Distance(PlayerMovement.instance.transform.position, checkPoint2.position);
            if (distance <= mySprite.size.y / 4) {
                UIGameController.instance.ShowScorePopup(100, PlayerMovement.instance.transform.position);
                GameController.instance.UpdateScore(100);
            } else if (distance > mySprite.size.y / 4 && distance <= mySprite.size.y / 2) {
                UIGameController.instance.ShowScorePopup(200, PlayerMovement.instance.transform.position);
                GameController.instance.UpdateScore(200);
            } else if (distance > mySprite.size.y / 2 && distance <= mySprite.size.y * 0.75f) {
                UIGameController.instance.ShowScorePopup(500, PlayerMovement.instance.transform.position);
                GameController.instance.UpdateScore(500);
            } else if (distance > mySprite.size.y * 0.75f) {
                UIGameController.instance.ShowScorePopup(1000, PlayerMovement.instance.transform.position);
                GameController.instance.UpdateScore(1000);
            }
        }
    }
}
