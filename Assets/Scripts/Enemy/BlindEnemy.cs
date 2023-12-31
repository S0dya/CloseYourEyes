using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;
using System.Linq;

public class BlindEnemy : MonoBehaviour
{
    SpriteRenderer sprite;
    Rigidbody2D rigidbody;
    [HideInInspector] public Animator animator;
    GameObject playerObject;
    Player player;

    [SerializeField] Transform point;
    [HideInInspector] public bool isWatched;
    [HideInInspector] public bool isFollowing;
    Vector2 startPos;

    Vector2 currentVelocity = new Vector2(0, 0);

    Coroutine movingCor;
    Coroutine fadeInCor;
    Coroutine fadeOutCor;
    Coroutine waitBeforeReturning;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.speed = 0;
        playerObject = SceneManager.GetSceneByName("PersistantScene").GetRootGameObjects().FirstOrDefault(obj => obj.CompareTag("Player"));
        player = playerObject.GetComponent<Player>();
    }

    void Start()
    {
        startPos = transform.position;
        point.position = startPos;
    }

    public void HearPlayer()
    {
        point.position = playerObject.transform.position;
        animator.speed = 1.5f;
        if (!isFollowing)
        {
            isFollowing = true;

            if (!GameManager.Instance.isBlindFollowingPlayer)
            {
                AudioManager.Instance.PlayOneShot(FMODManager.Instance.BlindJump, transform.position); 
            }

            if (waitBeforeReturning != null)
            {
                StopCoroutine(waitBeforeReturning);
            }
            if (fadeOutCor != null)
            {
                StopCoroutine(fadeOutCor);
            }
            if (fadeInCor != null)
            {
                StopCoroutine(fadeInCor);
            }
            fadeInCor = StartCoroutine(FadeIn());
            GameManager.Instance.isBlindFollowingPlayer = true;
        }
    }

    public void StopFollowing(bool val)
    {
        isWatched = val;
        if (!val)
        {
            animator.speed = 1.5f;
            movingCor = StartCoroutine(MoveToTarget());
        }
        else
        {
            animator.speed = 1f;
        }
    }

    public IEnumerator MoveToTarget()
    {
        while (!isWatched)
        {
            Vector2 direction = ((Vector2)point.position - rigidbody.position) * 5;
            float distance = direction.magnitude;

            if (distance > 0.1f)
            {
                currentVelocity = Vector2.MoveTowards(currentVelocity, direction, 17 * Time.deltaTime);
                rigidbody.velocity = currentVelocity;
            }
            else
            {
                currentVelocity = Vector2.zero;
                OnDestinationReached();
                break;
            }

            yield return null;
        }
        currentVelocity = Vector2.zero;
        rigidbody.velocity = currentVelocity;
    }

    public void OnDestinationReached()
    {
        if (Vector2.Distance(startPos, transform.position) > 1f)
        {
            waitBeforeReturning = StartCoroutine(WaitBeforeReturning(4));
        }
        else
        {
            GameManager.Instance.isBlindFollowingPlayer = false;
        }
        fadeOutCor = StartCoroutine(FadeOut());
    }

    IEnumerator WaitBeforeReturning(float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if (!isWatched)
            {
                elapsedTime += Time.deltaTime;
            }

            yield return null;
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
            b = Mathf.Lerp(b, 1.3f, 1.5f * Time.deltaTime);
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
            if (!isWatched)
            {
                b = Mathf.Lerp(b, -0.3f, 1.3f * Time.deltaTime);
                sprite.color = new Color(sprite.color.r, sprite.color.g, b);
            }

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
