using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    [SerializeField] private GameObject clew;
    [SerializeField] private AirBomb bomb;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.TAG.BULLET))
        {
            Boom();
        }
        if (collision.name == Constants.NAME.FOOT)
        {
            PlayerMovement.instance.rb.velocity = new Vector2(PlayerMovement.instance.rb.velocity.x, 0);
            PlayerMovement.instance.rb.AddForce(Vector2.up * 500);
            Boom();
        }
        if (collision.CompareTag(Constants.TAG.BOMB))
        {
            Boom();
        }
    }

    private void Boom()
    {
        gameObject.SetActive(false);
        if (clew != null)
        {
            clew.SetActive(false);
        }
        if (bomb.GetComponent<Rigidbody2D>() != null)
        {
            bomb.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }
    }
}
