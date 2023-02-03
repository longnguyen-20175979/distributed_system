using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheckBar : MonoBehaviour
{
    [SerializeField] private MoveDownBar moveDownBar;
    [SerializeField] private BoxCollider2D moveDownBarCol;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            moveDownBar.enabled = false;
        }
        if (collision.CompareTag(Constants.TAG.PLAYER))
        {
            moveDownBarCol.isTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.TAG.PLAYER))
        {
            moveDownBarCol.isTrigger = false;
        }
    }
}
