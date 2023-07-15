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
            if (hit.collider == null)
            {
                BlindEnemy blindEnemy = collision.gameObject.GetComponent<BlindEnemy>();
                if (!blindEnemy.isFollowingPlayer && !blindEnemy.isWatched)
                {
                    blindEnemy.HearPlayer(true);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("BlindEnemy"))
        {
            BlindEnemy blindEnemy = collision.gameObject.GetComponent<BlindEnemy>();
            blindEnemy.HearPlayer(false);
        }
    }
}
