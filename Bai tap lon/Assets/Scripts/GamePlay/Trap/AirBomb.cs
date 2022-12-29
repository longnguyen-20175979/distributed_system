using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DarkTonic.MasterAudio;

public class AirBomb : Trap
{
    [SerializeField] private float radius;
    [SerializeField] private LayerMask hitLayer;
    [SerializeField] private GameObject explosionEffect;

    [SerializeField] private Transform bomb;
    [SerializeField] private Transform balloon;
    [SerializeField] private Transform clew;
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool idle;

    private void OnEnable()
    {
        explosionEffect.SetActive(false);
    }

    private void Start()
    {
        Floating();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (distance <= rangeCheck && !idle)
        {
            bomb.Translate(Vector2.left * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void Floating()
    {
        bomb.DOMoveY(bomb.position.y + 0.2f, 1f).OnComplete(() =>
        {
            bomb.DOMoveY(bomb.position.y - 0.2f, 1f).OnComplete(() =>
            {
                Floating();
            });
        });
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.TAG.PLAYER))
        {
            Explode();
            FlyToTheMoon();
        }
        if (collision.CompareTag(Constants.TAG.BULLET))
        {
            Explode();
            FlyToTheMoon();
        }
        if (collision.gameObject.layer == Constants.LAYER.GROUND)
        {
            Destroy(GetComponent<Rigidbody2D>());
            Explode();
            if (balloon != null && clew != null)
            {
                FlyToTheMoon();
            }
        }
    }

    public void Explode()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        moveSpeed = 0;
        MasterAudio.PlaySound(Constants.Audio.SOUND_SHOOT_BOOM);
        Vibration.VibrateLightImpact();
        GetComponent<SpriteRenderer>().color = Vector4.zero;
        GetComponent<CircleCollider2D>().enabled = false;
        explosionEffect.transform.position = transform.position;
        explosionEffect.SetActive(true);
        StartCoroutine(Helper.StartAction(() =>
        {
            gameObject.SetActive(false);
        }, 0.4f));

        Vector2 explosionPos = transform.position;
        Collider2D[] cols = Physics2D.OverlapCircleAll(explosionPos, radius, hitLayer);
        foreach (Collider2D col in cols)
        {
            if (col.CompareTag(Constants.TAG.ENEMY))
            {
                EnemyBase enemy = col.GetComponent<EnemyBase>();
                if (enemy != null)
                {
                    enemy.EnemyDie();
                }
            }
            if (col.CompareTag(Constants.TAG.PLAYER))
            {
                PlayerMovement.instance.playerAction.HurtPlayer(damage);
            }
        }       
    }

    private void FlyToTheMoon()
    {
        balloon.DOMoveY(transform.position.y + 10, 5).SetEase(Ease.Linear).OnComplete(() => Destroy(balloon.gameObject));
        clew.DOMoveY(transform.position.y + 10, 5).SetEase(Ease.Linear).OnComplete(() => Destroy(clew.gameObject));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
