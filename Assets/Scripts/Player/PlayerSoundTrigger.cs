using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundTrigger : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BlindEnemy"))
        {
            BlindEnemy blindEnemy = collision.gameObject.GetComponent<BlindEnemy>();
            blindEnemy.HearPlayer(true);
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
