using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundTrigger : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("BlindEnemy"))
        {
            Vector2 direction = collision.transform.position - transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, direction.magnitude, LayerMask.GetMask("Obstacle"));
            BlindEnemy blindEnemy = collision.gameObject.GetComponent<BlindEnemy>();

            if (hit.collider == null)
            {
                if (!blindEnemy.isWatched)
                {
                    blindEnemy.HearPlayer();
                }
            }
            else
            {
                blindEnemy.isFollowing = false;
            }
        }
    }
    
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("BlindEnemy"))
        {
            BlindEnemy blindEnemy = collision.gameObject.GetComponent<BlindEnemy>();
            blindEnemy.isFollowing = false;
        }
    }
}
