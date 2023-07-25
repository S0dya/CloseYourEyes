using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class BlindEnemy : MonoBehaviour
{
    SpriteRenderer sprite;
    Rigidbody2D rigidbody;
    Animator animator;
    GameObject playerObject;
    Player player;

    [SerializeField] Transform point;
    [HideInInspector] public bool isWatched;
    Vector2 startPos;

    Coroutine movingCor;
    Coroutine fadeInCor;
    Coroutine fadeOutCor;
    Coroutine waitBeforeReturning;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerObject = SceneManager.GetSceneByName("PersistantScene").GetRootGameObjects().FirstOrDefault(obj => obj.CompareTag("Player"));
        player = playerObject.GetComponent<Player>();
    }

    void Start()
    {
        startPos = transform.position;
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    public void StartFollowoingPlayer()
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
    public void HearPlayer()
    {
        point.position = playerObject.transform.position;
    }

    public void StopFollowing(bool val)
    {
        isWatched = val;
        if (!val)
        {
            movingCor = StartCoroutine(MoveToTarget());
        }
    }

    public IEnumerator MoveToTarget()
    {
        while (!isWatched)
        {
            Vector2 direction = (Vector2)point.position - rigidbody.position;
            float distance = direction.magnitude;

            if (distance > 0.1f)
            {
                direction.Normalize();
                rigidbody.velocity = direction * 5;
            }
            else
            {
                OnDestinationReached();
                break;
            }

            yield return null;
        }
        rigidbody.velocity = Vector2.zero;
    }

    public void OnDestinationReached()
    {
        if (Vector2.Distance(startPos, transform.position) > 1f)
        {
            waitBeforeReturning = StartCoroutine(WaitBeforeReturning(4));
        }
        fadeOutCor = StartCoroutine(FadeOut());
    }

    IEnumerator WaitBeforeReturning(float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if (GameManager.Instance.isMenuOpen || isWatched)
            {
                yield return null;
            }
            else
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        point.position = startPos;
        movingCor = StartCoroutine(MoveToTarget());
    }


    IEnumerator FadeIn()
    {
        if (waitBeforeReturning != null)
        {
            StopCoroutine(waitBeforeReturning);
        }

        float b = sprite.color.b;
        while (b <= 1)
        {
            if (GameManager.Instance.isMenuOpen)
            {
                yield return null;
            }

            b = Mathf.Lerp(b, 1.3f, 0.009f);
            sprite.color = new Color(sprite.color.r, sprite.color.g, b);

            yield return null;
        }

        movingCor = StartCoroutine(MoveToTarget());
    }
    IEnumerator FadeOut()
    {
        float b = sprite.color.b;
        while (b >= 0)
        {
            if (GameManager.Instance.isMenuOpen || isWatched)
            {
                yield return null;
            }

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
