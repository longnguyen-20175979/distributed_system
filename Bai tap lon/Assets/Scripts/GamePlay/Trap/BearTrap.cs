using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BearTrap : Trap
{
    [SerializeField] private Animator anim;
    private bool hit;

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.TAG.PLAYER) && !hit)
        {
            hit = true;
            anim.SetTrigger("Attack");
            DarkTonic.MasterAudio.MasterAudio.PlaySound(Constants.Audio.SOUND_BEAR_TRAP);
            PlayerMovement.instance.playerAction.HurtPlayer(damage);
            StartCoroutine(Disappear());
        }
    }

    private IEnumerator Disappear()
    {
        yield return new WaitForSeconds(3);
        GetComponent<SpriteRenderer>().DOFade(0, 1).OnComplete(() => Destroy(gameObject));
    }
}
