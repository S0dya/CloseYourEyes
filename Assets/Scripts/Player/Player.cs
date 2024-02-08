using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using UnityEngine.Rendering.Universal;

public class Player : SingletonMonobehaviour<Player>
{
    Rigidbody2D rigidbody;
    Light2D eyeVision;
    Animator animator;
    [SerializeField] CircleCollider2D playerVisionTrigger;
    [SerializeField] CircleCollider2D playerSoundTrigger;

    [HideInInspector] public Vector2 movementAmount;
    [HideInInspector] public float speed;
    [HideInInspector] public bool isFingerDown;
    [HideInInspector] public bool canOpenEye = true;
    [HideInInspector] public bool onWater;
    [HideInInspector] public bool isThunderLevel;

    Vector2 currentVelocity = new Vector2(0, 0);

    Coroutine moveCor;

    Coroutine openEyeCor;
    Coroutine closeEyeCor;

    float xInput;
    float yInput;

    Inputs inputs;

    float walkingSpeed = 1.5f;
    float runningSpeed = 3f;

    protected override void Awake()
    {
        base.Awake();

        rigidbody = GetComponent<Rigidbody2D>();
        eyeVision = GetComponentInChildren<Light2D>();
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        if (isThunderLevel)
        {
            ThunderManager.Instance.StartThunder();
        }


#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        inputs = new Inputs();

        inputs.Input.Movement.performed += ctx => OnMoveInput(ctx.ReadValue<Vector2>());
        inputs.Input.Movement.canceled += ctx => OnMoveInput(ctx.ReadValue<Vector2>());

        inputs.Input.Shift.performed += ctx => speed = runningSpeed;
        inputs.Input.Shift.canceled += ctx => speed = walkingSpeed;

        inputs.Input.Space.performed += ctx => OpenEye();
        inputs.Input.Space.canceled += ctx => CloseEye();

        inputs.Enable();

        speed = walkingSpeed;
#endif
    }

    void OnDisable()
    {
        transform.position = Vector2.zero;
        StopAllCoroutines();
        playerVisionTrigger.radius = 0;
        playerSoundTrigger.radius = 0;
        eyeVision.pointLightOuterRadius = 0;
        canOpenEye = true;

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        inputs = new Inputs();

        inputs.Input.Movement.performed -= ctx => OnMoveInput(ctx.ReadValue<Vector2>());
        inputs.Input.Movement.canceled -= ctx => OnMoveInput(ctx.ReadValue<Vector2>());

        inputs.Input.Shift.performed -= ctx => speed = runningSpeed;
        inputs.Input.Shift.canceled -= ctx => speed = walkingSpeed;

        inputs.Input.Space.performed -= ctx => OpenEye();
        inputs.Input.Space.canceled -= ctx => CloseEye();

        inputs.Disable();
#endif
    }

    //inputs
    void OnMoveInput(Vector2 direction)
    {
        xInput = direction.x;
        yInput = direction.y; 

        if (yInput != 0 && xInput != 0)
        {
            xInput = xInput * 0.71f;
            yInput = yInput * 0.71f;
        }

        movementAmount = new Vector2(xInput, yInput);
    }

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
    void Update()
    {
        rigidbody.velocity = movementAmount.normalized * speed * (onWater ? 0.6f : 1);

        playerSoundTrigger.radius = (rigidbody.velocity == Vector2.zero ? 0 : Mathf.Max(speed * (onWater ? 2.75f : 1.75f), 1.2f));
    }
#endif

    public IEnumerator Move()
    {
        PlayWalking();

        while (isFingerDown)
        {
            Vector2 moveDirection = movementAmount.normalized;
            currentVelocity = Vector2.MoveTowards(currentVelocity, moveDirection * moveDirection.magnitude, 15 * Time.deltaTime);

            animator.speed = speed /3;
            rigidbody.velocity = currentVelocity * speed * (onWater ? 0.6f : 1);
            playerSoundTrigger.radius = Mathf.Max(speed * (onWater ? 2.75f : 1.75f), 1.2f);

            yield return null;
        }

        animator.speed = 1f;
        animator.Play("PlayerIDLE");
        speed = 0;
        playerSoundTrigger.radius = 1f;
        currentVelocity = Vector2.zero;
        rigidbody.velocity = currentVelocity;
    }


    public void OpenEye()
    {
        canOpenEye = false;
        if (closeEyeCor != null)
        {
            StopCoroutine(closeEyeCor);
        }
#if UNITY_ANDROID || UNITY_IOS
        EyeButton.Instance.eye.Play("OpenEye");
#endif
        openEyeCor = StartCoroutine(OpenEyeCor());
    }
    IEnumerator OpenEyeCor()
    {
        float radius = eyeVision.pointLightOuterRadius;
        float maxRadius = Settings.curVisionRadious;

        while (radius < maxRadius)
        {
            eyeVision.pointLightOuterRadius = radius;
            playerVisionTrigger.radius = radius;
            radius = Mathf.Lerp(eyeVision.pointLightOuterRadius, 8, Time.deltaTime);

            
            yield return null;
        }
        
        yield return new WaitForSeconds(Settings.visionTime[Settings.curSceneNum-2]);
        CloseEye();
    }
    public void CloseEye()
    {
        if (openEyeCor != null)
        {
            StopCoroutine(openEyeCor);
        }
        closeEyeCor = StartCoroutine(CloseEyeCor());
#if UNITY_ANDROID || UNITY_IOS
        EyeButton.Instance.eye.Play("CloseEye");
#endif
    }
    IEnumerator CloseEyeCor()
    {
        float radius = eyeVision.pointLightOuterRadius;
        while (radius > 0)
        {
            eyeVision.pointLightOuterRadius = radius;
            playerVisionTrigger.radius = radius;
            radius = Mathf.Lerp(eyeVision.pointLightOuterRadius, -9, Time.deltaTime);
            
            yield return null;
        }

        canOpenEye = true;
    }


    public void PlayWalking()
    {
        if (onWater)
        {
            animator.Play("PlayerWalkingOnWater");
        }
        else
        {
            animator.Play("PlayerWalking");
        }
    }

    public void PlayStepSound()
    {
        AudioManager.Instance.PlayOneShot(FMODManager.Instance.PlayerStepSound, Vector2.zero, speed/3);
    }

    public void PlayStepSoundOnWater()
    {
        AudioManager.Instance.PlayOneShot(FMODManager.Instance.PlayerStepSoundOnWater, Vector2.zero, speed /3);
    }

    public void Die()
    {
        AudioManager.Instance.PlayOneShot("DieSound");
        GameMenu.Instance.ToggleUnputUI(false);
        GameMenu.Instance.GameOver();
        StopAllCoroutines();
        speed = 0;
        rigidbody.velocity = Vector2.zero;
        animator.Play("PlayerDeath");
    }

    public void OpenGameOver()
    {
        GameMenu.Instance.OpenGameMenu();
        GameMenu.Instance.ToggleUnputUI(true);
    }
}
