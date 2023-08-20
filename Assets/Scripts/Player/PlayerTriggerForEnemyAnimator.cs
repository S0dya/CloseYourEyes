using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerForEnemyAnimator : SingletonMonobehaviour<PlayerTriggerForEnemyAnimator>
{
    protected override void Awake()
    {
        base.Awake();

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BlindEnemy"))
        {
            BlindEnemy blindEnemy = collision.gameObject.GetComponent<BlindEnemy>();
            blindEnemy.animator.speed = 1;
        }
        else if (collision.CompareTag("DefEnemy"))
        {
            DefEnemy defEnemy = collision.gameObject.GetComponent<DefEnemy>();
            defEnemy.animator.speed = 1;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("BlindEnemy"))
        {
            BlindEnemy blindEnemy = collision.gameObject.GetComponent<BlindEnemy>();
            blindEnemy.animator.speed = 0;
        }
        else if (collision.CompareTag("DefEnemy"))
        {
            DefEnemy defEnemy = collision.gameObject.GetComponent<DefEnemy>();
            defEnemy.animator.speed = 0;
        }
    }
}
