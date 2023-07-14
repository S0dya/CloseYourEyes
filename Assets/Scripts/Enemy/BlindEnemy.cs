using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pathfinding;

public class BlindEnemy : MonoBehaviour
{
    SpriteRenderer sprite;
    Animator animator;
    AIDestinationSetter destinationSetter;
    AI ai;

    GameObject playerObject;
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
        playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.GetComponent<Player>();

        ai.enabled = false;
    }

    public void HearPlayer(bool val)
    {
        isFollowingPlayer = val;
        if (val)
        {
            if (fadeOutCor != null)
            {
                StopCoroutine(fadeOutCor);
            }

            if (fadeInCor != null)
            {
                StopCoroutine(fadeInCor);
            }
            fadeInCor = StartCoroutine(FadeIn());
        }
        else
        {
            destinationSetter.target = point;
            point.position = playerObject.transform.position;
        }
    }

    public void StopFollowing(bool val)
    {
        ai.canMove = val;
    }

    public void OnDestinationReached()
    {
        fadeOutCor = StartCoroutine(FadeOut());
        ai.enabled = false;
    }

    IEnumerator FadeIn()
    {
        float b = sprite.color.b;
        while (b <= 1)
        {
            b = Mathf.Lerp(b, 1.3f, 0.009f);
            sprite.color = new Color(sprite.color.r, sprite.color.g, b);

            yield return null;
        }

        if (isFollowingPlayer)
        {
            destinationSetter.target = playerObject.transform;
        }
        ai.enabled = true;
    }
    IEnumerator FadeOut()
    {
        float b = sprite.color.b;
        while (b >= 0)
        {
            b = Mathf.Lerp(b, -0.3f, 0.02f);
            sprite.color = new Color(sprite.color.r, sprite.color.g, b);

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