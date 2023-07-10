using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class Player : SingletonMonobehaviour<Player>
{
    Rigidbody2D rigidbody;

    [SerializeField] GameObject joystick;
    Vector2 joystickSize;
    RectTransform joystickTransform;
    RectTransform joystickKnob;

    Finger movementFinger;
    Vector2 movementAmount;

    Vector2 currentVelocity = new Vector2(0, 0);

    protected override void Awake()
    {
        base.Awake();

        rigidbody = GetComponent<Rigidbody2D>();

        joystick.SetActive(false);//sd
        joystickTransform = joystick.GetComponent<RectTransform>();
        joystickKnob = joystick.GetComponentInChildren<RectTransform>();
        joystickSize = new Vector2(300, 300);
    }

    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += HandleFingerDown;
        ETouch.Touch.onFingerMove += HandleFingerMove;
        ETouch.Touch.onFingerUp += HandleRemoveFinger;
    }

    void OnDisable()
    {
        ETouch.Touch.onFingerDown -= HandleFingerDown;
        ETouch.Touch.onFingerMove -= HandleFingerMove;
        ETouch.Touch.onFingerUp -= HandleRemoveFinger;
        EnhancedTouchSupport.Disable();
    }
    //joystick
    void HandleFingerDown(Finger touchedFinger)
    {
        if (movementFinger == null && touchedFinger.screenPosition.x <= Screen.width / 2f)
        {
            movementFinger = touchedFinger;
            movementAmount = Vector2.zero;
            joystickTransform.sizeDelta = joystickSize;
            joystickTransform.anchoredPosition = ClampStartPosition(touchedFinger.screenPosition);
            joystick.SetActive(true);
        }
    }
    Vector2 ClampStartPosition(Vector2 startPosition)
    {
        if (startPosition.x < joystickSize.x / 2)
        {
            startPosition.x = joystickSize.x / 2;
        }

        if (startPosition.y < joystickSize.y / 2)
        {
            startPosition.y = joystickSize.y / 2;
        }
        else if (startPosition.y > Screen.height - joystickSize.y / 2)
        {
            startPosition.y = Screen.height - joystickSize.y / 2;
        }

        return startPosition;
    }

    void HandleFingerMove(Finger moveFinger)
    {
        Vector2 knobPos;
        float max = joystickSize.x / 2;
        ETouch.Touch cur = moveFinger.currentTouch;

        if (Vector2.Distance(cur.screenPosition, joystickTransform.anchoredPosition) > max)
        {
            knobPos = (cur.screenPosition - joystickTransform.anchoredPosition).normalized * max;
        }
        else
        {
            knobPos = cur.screenPosition - joystickTransform.anchoredPosition;
        }

        joystickKnob.anchoredPosition = knobPos;
        movementAmount = knobPos / max;
    }

    void HandleRemoveFinger(Finger removeFinger)
    {
        if (removeFinger == movementFinger)
        {
            movementFinger = null;
            joystick.SetActive(false);
            joystickKnob.anchoredPosition = Vector2.zero;
            movementAmount = Vector2.zero;
        }
    }

    void Update()
    {
        Vector2 moveDirection = movementAmount.normalized;
        float targetSpeed = 1f * moveDirection.magnitude;

        currentVelocity = Vector2.MoveTowards(currentVelocity, moveDirection * targetSpeed, 15 * Time.deltaTime);

        rigidbody.velocity = currentVelocity * 2.5f;
    }
}
