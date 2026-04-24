using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    private static PlayerMovementController instance;
   
    private Vector2 touchStartPos;

    [Header("Character Controller")]
    [SerializeField] public CharacterController characterController;

    [Header("Movement")]
    [SerializeField] private float dragSensitivity = 0.1f;
    [SerializeField] private float forwardSpeed = 5f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float jumpSpeedMultiplier = 1.5f;
    [SerializeField] private float gravity = -12f;

    private float velocityY;
    private float originalForwardSpeed;

    [Header("Ground Check")]
    [SerializeField] private Transform raycastPosition;
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private float groundCheckDistance = 0.5f;

    [Header("States")]
    public bool isGrounded;
    public bool isRotation = false;
    public bool IsControlDisable = true;
    public bool isDealingWithHurdle = false;
    public bool PlayerMovementCheck = true;
    public bool IsUpdateRun = true;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        originalForwardSpeed = forwardSpeed;
    }

    public static PlayerMovementController GetInstance()
    {
        return instance;
    }
    void Update()
    {
        if (!IsUpdateRun) return;

        dragSensitivity = Mathf.Lerp(1.2f, 3f, Mathf.InverseLerp(18, 50, 1f / Time.unscaledDeltaTime));

        // Ground check
        isGrounded = characterController.isGrounded;

        // Forward movement
        Vector3 move = Vector3.zero;

        if (PlayerMovementCheck && !isDealingWithHurdle)
        {
            float currentSpeed = isGrounded ? originalForwardSpeed : originalForwardSpeed * jumpSpeedMultiplier;
            move += Vector3.forward * currentSpeed;
        }

        // Horizontal drag movement
        HandleMovement();

        // Use custom gravity
        ApplyGravity();
        move.y = velocityY;

        // Final move once per frame
        characterController.Move(move * Time.deltaTime);
    }


    void HandleMovement()
    {
        if (Input.touchCount > 0 || Input.GetMouseButton(0))
        {
            if (IsControlDisable && !isDealingWithHurdle)
            {
                Vector2 inputPos = Input.touchCount > 0 ? Input.GetTouch(0).position : (Vector2)Input.mousePosition;

                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButtonDown(0))
                {
                    touchStartPos = inputPos;
                }

                Vector2 touchDelta = inputPos - touchStartPos;
                Vector3 worldDelta = new Vector3(touchDelta.x, 0, 0) * dragSensitivity * Time.deltaTime;
                if (PlayerMovementCheck)
                {
                    characterController.Move(worldDelta);
                }

                touchStartPos = inputPos;
            }


        }
    }
    public void Jump()
    {
        if (isGrounded)
        {
            velocityY = jumpForce;
            isGrounded = false;

            if (GameManager.Instance.PlayerAnimation.m_Animator != null && !GameManager.Instance.IsPlayerDead)
            {
                GameManager.Instance.PlayerAnimation.m_Animator.Play("Jump");
                isRotation = true;
            }
        }
    }

    public void EndBossKillJump()
    {
        if (!isGrounded) return;

        velocityY = 30f;
        isGrounded = false;

        if (GameManager.Instance.PlayerAnimation.m_Animator != null && !GameManager.Instance.IsPlayerDead)
        {
            GameManager.Instance.PlayerAnimation.EndSlashAnimation();
            isRotation = true;
        }
    }

    public void PlayLowerRunAnimation()
    {
        if (GameManager.Instance.PlayerAnimation.m_Animator != null && !GameManager.Instance.IsPlayerDead)
        {
            GameManager.Instance.PlayerAnimation.m_Animator.transform.rotation = Quaternion.identity;
            GameManager.Instance.PlayerAnimation.m_Animator.Play("Run");
            Debug.Log("Play Run Animation from PlayerMovementController");
        }
    }

    void ApplyGravity()
    {
        if (isGrounded && velocityY < 0)
        {
            velocityY = -2f;
        }
        else
        {
            if (velocityY > 0)
            {
                velocityY += gravity * 0.8f * Time.deltaTime; // softer while rising
            }
            else
            {
                velocityY += gravity * 2f * Time.deltaTime; // stronger while falling
            }
        }
    }
}
