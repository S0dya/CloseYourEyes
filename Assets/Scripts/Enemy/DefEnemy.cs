using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class DefEnemy : MonoBehaviour
{
    SpriteRenderer sprite;
    Rigidbody2D rigidbody;
    Animator animator;
    GameObject playerObject;
    Player player;

    [SerializeField] Transform point;
    [HideInInspector] public bool isFollowingPlayer;
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
    public void SeePlayer()
    {
        point.position = playerObject.transform.position;
    }

    public void StopFollowing(bool val)
    {
        if (val)
        {
            if (movingCor != null)
            {
                StopCoroutine(movingCor);
            }
        }
        else
        {
            movingCor = StartCoroutine(MoveToTarget());
        }
    }

    public IEnumerator MoveToTarget()
    {
        while (true)
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
            waitBeforeReturning = StartCoroutine(WaitBeforeReturning());
        }
        fadeOutCor = StartCoroutine(FadeOut());
    }

    IEnumerator WaitBeforeReturning()
    {
        yield return GameManager.Instance.StartCoroutine(GameManager.Instance.Timer(4f));

        point.position = startPos;
        movingCor = StartCoroutine(MoveToTarget());
    }

    IEnumerator FadeIn()
    {
        if (waitBeforeReturning != null)
        {
            StopCoroutine(waitBeforeReturning);
        }
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

        movingCor = StartCoroutine(MoveToTarget());
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
