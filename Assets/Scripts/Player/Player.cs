using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Player : SingletonMonobehaviour<Player>
{
    Rigidbody2D rigidbody;
    Light2D eyeVision;

    [HideInInspector] public Vector2 movementAmount;
    [HideInInspector] public bool isFingerDown;

    Vector2 currentVelocity = new Vector2(0, 0);

    Coroutine openEyeCor;
    Coroutine closeEyeCor;

    protected override void Awake()
    {
        base.Awake();

        rigidbody = GetComponent<Rigidbody2D>();
        eyeVision = GetComponentInChildren<Light2D>();
    }

    public IEnumerator Move()
    {
        while (isFingerDown)
        {
            Vector2 moveDirection = movementAmount.normalized;
            currentVelocity = Vector2.MoveTowards(currentVelocity, moveDirection * moveDirection.magnitude, 15 * Time.deltaTime);

            rigidbody.velocity = currentVelocity * 2.5f;

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
        while (eyeVision.pointLightOuterRadius < 4)
        {
            eyeVision.pointLightOuterRadius = Mathf.Lerp(eyeVision.pointLightOuterRadius, 5, 0.3f);
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
        while (eyeVision.pointLightOuterRadius > 0)
        {
            eyeVision.pointLightOuterRadius = Mathf.Lerp(eyeVision.pointLightOuterRadius, -1, 0.3f);
            yield return null;
        }
    }
}
