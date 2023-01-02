using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FireBall : Trap
{
    private bool isUp = true;

    [SerializeField] private float topPos;
    [SerializeField] private float botPos;
    [SerializeField] private float speed;
    private float duration;
    [SerializeField] private Ease easeUp;
    [SerializeField] private Ease easeDown;
    private Vector3 localScale;

    private void Start()
    {
        duration = 1 / speed;
        localScale = transform.localScale;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (distance <= rangeCheck)
        { 
            Move();
        }
    }

    private void Move()
    {       
        if (isUp)
        {
            isUp = false;
            transform.localScale = new Vector3(localScale.x, -localScale.y, localScale.z);
            transform.DOMoveY(topPos, duration).SetEase(easeUp).OnComplete(() =>
            {
                transform.localScale = localScale;
                transform.DOMoveY(botPos, duration).SetEase(easeDown).OnComplete(() =>
                {
                    isUp = true;
                });                
            });            
        }               
    }
    private void OnDisable() {
        transform.DOKill();
    }
}