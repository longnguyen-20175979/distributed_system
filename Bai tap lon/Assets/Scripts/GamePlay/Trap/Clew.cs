using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Clew : MonoBehaviour
{
    [SerializeField] private Transform balloon;
    [SerializeField] private AirBomb bomb;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.TAG.BULLET))
        {            
            balloon.DOMoveY(transform.position.y + 10, 5).SetEase(Ease.Linear).OnComplete(() =>
            {
                if (balloon.gameObject != null) Destroy(balloon.gameObject);
            });
            if (bomb.GetComponent<Rigidbody2D>() != null) bomb.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            gameObject.SetActive(false);
        }
    }
}
