using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pathfinding;

public class Enemy : MonoBehaviour
{
    SpriteRenderer sprite;
    Animator animator;
    AIDestinationSetter destinationSetter;
    AI ai;
    Player player;

    [SerializeField] Transform point;
    bool isFollowingPlayer;

    Coroutine fadeInCor;
    Coroutine fadeOutCor;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        ai = GetComponent<AI>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        destinationSetter.target = point;
        ai.enabled = false;
    }

    public void SeePlayer(bool val)
    {
        isFollowingPlayer = val;
        if (val)
        {
            if (fadeInCor == null)
            {
                if (fadeOutCor != null)
                {
                    StopCoroutine(fadeOutCor);
                }
                fadeInCor = StartCoroutine(FadeIn());
            }
        }
        else
        {
            point.position = player.transform.position;
        }
    }

    public void OnDestinationReached()
    {
        Debug.Log("d");
        fadeOutCor = StartCoroutine(FadeOut());
        ai.enabled = false;
    }

    IEnumerator FadeIn()
    {
        float r = sprite.color.r; 
        while (r < 255)
        {
            sprite.color = new Color(r, sprite.color.g, sprite.color.b);
            r += Mathf.Lerp(r, 255, 0.2f);
            Debug.Log(r);

            yield return null;
        }

        point = player.transform;
        ai.enabled = true;
    }
    IEnumerator FadeOut()
    {
        float r = sprite.color.r;
        while (r > 0)
        {
            sprite.color = new Color(r, sprite.color.g, sprite.color.b);
            r += Mathf.Lerp(r, 0, 0.5f);
            Debug.Log(r);

            yield return null;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.Die();
        }
    }

}
