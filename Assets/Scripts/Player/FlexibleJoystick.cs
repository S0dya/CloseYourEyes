using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class FlexibleJoystick : SingletonMonobehaviour<FlexibleJoystick>
{
    Player player;

    [SerializeField] GameObject joystick;
    Vector2 joystickSize;
    RectTransform joystickTransform;
    [SerializeField] RectTransform joystickKnob;
    Finger movementFinger;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponent<Player>();

        joystickTransform = joystick.GetComponent<RectTransform>();
        joystickSize = new Vector2(300, 300);
    }

    void OnEnable()
    {
        joystick.SetActive(false);
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


    void HandleFingerDown(Finger touchedFinger)
    {
        if (movementFinger == null && touchedFinger.screenPosition.x <= Settings.screenHalf.x && !GameManager.Instance.isMenuOpen)
        {
            movementFinger = touchedFinger;
            joystickTransform.sizeDelta = joystickSize;
            joystickTransform.anchoredPosition = ClampStartPosition(touchedFinger.screenPosition);
            player.isFingerDown = true;
            player.StartCoroutine(player.Move());
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
        else if (startPosition.y > Settings.screen.y - joystickSize.y / 2)
        {
            startPosition.y = Settings.screen.y - joystickSize.y / 2;
        }

        return startPosition;
    }

    void HandleFingerMove(Finger moveFinger)
    {
        if (movementFinger == null || GameManager.Instance.isMenuOpen)
        {
            return;
        }
        if (GameManager.Instance.isInGame && moveFinger.currentTouch.screenPosition.x > Settings.placeForJoystickMovement.x && moveFinger.currentTouch.screenPosition.y < Settings.placeForJoystickMovement.y)
        {
            return;
        }

        Vector2 knobPos;
        float max = joystickSize.x / 2;
        
        if (Vector2.Distance(moveFinger.currentTouch.screenPosition, joystickTransform.anchoredPosition) > max)
        {
            knobPos = (moveFinger.currentTouch.screenPosition - joystickTransform.anchoredPosition).normalized * max;
        }
        else
        {
            knobPos = moveFinger.currentTouch.screenPosition - joystickTransform.anchoredPosition;
        }
        joystickKnob.anchoredPosition = knobPos;

        float speed = Mathf.Lerp(0.5f, 2.5f, knobPos.magnitude / max);
        player.speed = speed;
        player.movementAmount = knobPos / max;
    }


    void HandleRemoveFinger(Finger removeFinger)
    {
        if (removeFinger == movementFinger)
        {
            movementFinger = null;
            joystickKnob.anchoredPosition = Vector2.zero;
            player.isFingerDown = false;
            joystick.SetActive(false);
        }
    }
}
