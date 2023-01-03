using DarkTonic.MasterAudio;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimItemStart : MonoBehaviour
{
    [SerializeField] private Rigidbody2D[] rb;

    void OnEnable()
    {
        Boom(PlayerMovement.instance.transform.position);
    }

    void ResetPiece()
    {
        gameObject.SetActive(false);
    }
    public void Boom(Vector3 posBoom)
    {
        transform.position = new Vector2(posBoom.x, posBoom.y + 0.5f);

        MasterAudio.PlaySound(Constants.Audio.SOUND_COLLECT_ITEM);
        rb[0].AddForce(new Vector2(-45, 200));
        rb[1].AddForce(new Vector2(-15, 250));
        rb[2].AddForce(new Vector2(15, 250));
        rb[3].AddForce(new Vector2(45, 200));
        Invoke("ResetPiece", 3);
    }
}
