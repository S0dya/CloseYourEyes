using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisionTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DefEnemy"))
        {
            DefEnemy defEnemy = collision.gameObject.GetComponent<DefEnemy>();
            defEnemy.SeePlayer(true);
        }
        else if (collision.CompareTag("BlindEnemy"))
        {
            BlindEnemy blindEnemy = collision.gameObject.GetComponent<BlindEnemy>();
            blindEnemy.StopFollowing(false);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("DefEnemy"))
        {
            DefEnemy defEnemy = collision.gameObject.GetComponent<DefEnemy>();
            defEnemy.SeePlayer(false);
        }
        else if (collision.CompareTag("BlindEnemy"))
        {
            BlindEnemy blindEnemy = collision.gameObject.GetComponent<BlindEnemy>();
            blindEnemy.StopFollowing(true);
        }
    }
}
