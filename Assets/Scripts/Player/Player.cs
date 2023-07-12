using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Player : SingletonMonobehaviour<Player>
{
    //delater
    [SerializeField] Light2D globalLight;

    Rigidbody2D rigidbody;
    Light2D eyeVision;
    [SerializeField] CircleCollider2D playerTrigger;

    [HideInInspector] public Vector2 movementAmount;
    [HideInInspector] public float speed;
    [HideInInspector] public bool isFingerDown;

    Vector2 currentVelocity = new Vector2(0, 0);

    Coroutine openEyeCor;
    Coroutine closeEyeCor;

    protected override void Awake()
    {
        base.Awake();

        rigidbody = GetComponent<Rigidbody2D>();
        eyeVision = GetComponentInChildren<Light2D>();

        globalLight.intensity = 0f;
    }

    public IEnumerator Move()
    {
        while (isFingerDown)
        {
            Vector2 moveDirection = movementAmount.normalized;
            currentVelocity = Vector2.MoveTowards(currentVelocity, moveDirection * moveDirection.magnitude, 15 * Time.deltaTime);

            rigidbody.velocity = currentVelocity * speed;

            yield return null;
        }

        currentVelocity = Vector2.zero;
        rigidbody.velocity = currentVelocity;
    }


    public void OpenEye()
    {
        if (closeEyeCor != null)
        {
            StopCoroutine(closeEyeCor);
        }
        openEyeCor = StartCoroutine(OpenEyeCor());
    }
    IEnumerator OpenEyeCor()
    {
        float radius = eyeVision.pointLightOuterRadius;
        while (radius < 4)
        {
            eyeVision.pointLightOuterRadius = radius;
            playerTrigger.radius = radius;
            radius = Mathf.Lerp(eyeVision.pointLightOuterRadius, 5, Time.deltaTime);
            yield return null;
        }

        while (radius < 6.5f)
        {
            eyeVision.pointLightOuterRadius = radius;
            playerTrigger.radius = radius;
            radius = Mathf.Lerp(eyeVision.pointLightOuterRadius, 5, Time.deltaTime * 0.6f);
            yield return null;
        }
    }

    public void CloseEye()
    {
        if (openEyeCor != null)
        {
            StopCoroutine(openEyeCor);
        }
        closeEyeCor = StartCoroutine(CloseEyeCor());
    }
    IEnumerator CloseEyeCor()
    {
        float radius = eyeVision.pointLightOuterRadius;
        while (radius > 0)
        {
            eyeVision.pointLightOuterRadius = radius;
            playerTrigger.radius = Mathf.Max(radius, 0.9f);
            radius = Mathf.Lerp(eyeVision.pointLightOuterRadius, -1, 1.5f * Time.deltaTime);
            yield return null;
        }
    }
}
