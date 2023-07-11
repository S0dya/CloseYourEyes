using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class Joystick : SingletonMonobehaviour<Joystick>
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

        joystick.SetActive(false);//sd
        joystickTransform = joystick.GetComponent<RectTransform>();
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


    void HandleFingerDown(Finger touchedFinger)
    {
        if (movementFinger == null && touchedFinger.screenPosition.x <= Screen.width / 2f)
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