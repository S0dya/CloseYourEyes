using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisionTrigger : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("DefEnemy"))
        {
            Vector2 direction = collision.transform.position - transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, direction.magnitude, LayerMask.GetMask("Obstacle"));
            DefEnemy defEnemy = collision.gameObject.GetComponent<DefEnemy>();

            if (hit.collider == null)
            {
                defEnemy.SeePlayer();
            }
            else
            {
                defEnemy.isFollowing = false;
            }
        }
        
        if (collision.CompareTag("BlindEnemy"))
        {
            Vector2 direction = collision.transform.position - transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, direction.magnitude, LayerMask.GetMask("Obstacle"));

            if (hit.collider == null)
            {
                BlindEnemy blindEnemy = collision.gameObject.GetComponent<BlindEnemy>();
                if (!blindEnemy.isWatched)
                {
                    blindEnemy.StopFollowing(true);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("DefEnemy"))
        {
            DefEnemy defEnemy = collision.gameObject.GetComponent<DefEnemy>();
            defEnemy.isFollowing = false;
        }

        if (collision.CompareTag("BlindEnemy"))
        {
            BlindEnemy blindEnemy = collision.gameObject.GetComponent<BlindEnemy>();
            blindEnemy.StopFollowing(false);
        }
    }
}
