using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class DefEnemy : MonoBehaviour
{
    SpriteRenderer sprite;
    Rigidbody2D rigidbody;
    [HideInInspector] public Animator animator;
    GameObject playerObject;
    Player player;

    [SerializeField] Transform point;
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
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    public void SeePlayer()
    {
        point.position = playerObject.transform.position;
        animator.speed = 1.5f;
        if (!isFollowing)
        {
            isFollowing = true;

            if (!GameManager.Instance.isBlindFollowingPlayer)
            {
                AudioManager.Instance.PlayOneShot(FMODManager.Instance.DefJump, transform.position);
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
            GameManager.Instance.isDefFollowingPlayer = true;
        }
    }

    public void StopFollowing(bool val)
    {
        if (val)
        {
            animator.speed = 1f;
            if (movingCor != null)
            {
                StopCoroutine(movingCor);
            }
        }
        else
        {
            animator.speed = 1.5f;
            movingCor = StartCoroutine(MoveToTarget());
        }
    }

    public IEnumerator MoveToTarget()
    {
        while (true)
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
        rigidbody.velocity = currentVelocity;
    }

    public void OnDestinationReached()
    {
        if (Vector2.Distance(startPos, transform.position) > 1f)
        {
            waitBeforeReturning = StartCoroutine(WaitBeforeReturning());
        }
        else
        {
            GameManager.Instance.isDefFollowingPlayer = false;
        }
        fadeOutCor = StartCoroutine(FadeOut());
    }

    IEnumerator WaitBeforeReturning()
    {
        yield return new WaitForSeconds(4f);

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
            r = Mathf.Lerp(r, 1.3f, 1.5f * Time.deltaTime);
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
            r = Mathf.Lerp(r, -0.3f, 1.3f * Time.deltaTime);
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
