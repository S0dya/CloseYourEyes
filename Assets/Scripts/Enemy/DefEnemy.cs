using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pathfinding;

public class DefEnemy : MonoBehaviour
{
    SpriteRenderer sprite;
    Animator animator;
    AIDestinationSetter destinationSetter;
    [HideInInspector] public AI ai;

    GameObject playerObject;
    Player player;

    [SerializeField] Transform point;
    [HideInInspector] public bool isFollowingPlayer;

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

    public void SeePlayer(bool val)
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

    public void OnDestinationReached()
    {
        fadeOutCor = StartCoroutine(FadeOut());
        ai.enabled = false;
    }

    IEnumerator FadeIn()
    {
        float r = sprite.color.r; 
        while (r <= 1)
        {
            if (GameManager.Instance.isMenuOpen)
            {
                yield return null;
                continue;
            }

            r = Mathf.Lerp(r, 1.3f, 0.009f);
            sprite.color = new Color(r, sprite.color.g, sprite.color.b);

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
        float r = sprite.color.r;
        while (r >= 0)
        {
            if (GameManager.Instance.isMenuOpen)
            {
                yield return null;
                continue;
            }

            r = Mathf.Lerp(r, -0.3f, 0.02f);
            sprite.color = new Color(r, sprite.color.g, sprite.color.b);

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
