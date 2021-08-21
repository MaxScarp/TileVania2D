using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region Components References ...
    Rigidbody2D playerRigidbody;
    PlayerInputAction playerInput;
    Animator playerAnimator;
    Collider2D playerCollider;
    #endregion

    #region Attributes ...
    [Header("Run")]
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float acceleration = 2f;
    [SerializeField] float deceleration = 2f;
    [SerializeField] float velocityPower = 3f;
    [Space(5f)]
    [SerializeField] float frictionAmount = 0.2f;
    [Space(10f)]
    [Header("Jump")]
    [SerializeField] float jumpSpeed = 5f;

    float playerScaleMultiplier = 10f;
    bool isGrounded;
    bool isJumping;
    #endregion

    private void Awake()
    {
        Initialize();
    }

    private void FixedUpdate()
    {
        Run();
    }

    private void GroundedCheck()
    {
        if (playerCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            isGrounded = true;
            if (playerRigidbody.velocity.y < Mathf.Epsilon && isJumping)
            {
                isJumping = false;
                playerAnimator.SetBool("isJumping", isJumping);
            }
        }
        else
        {
            isGrounded = false;
        }
    }

    private void Jump(InputAction.CallbackContext context)
    {
        GroundedCheck();
        if (isGrounded)
        {
            playerRigidbody.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            isJumping = true;
            playerAnimator.SetBool("isJumping", isJumping);
        }
    }

    private void FlipSprite(float inputX)
    {
        bool isMovingHorizontally = Mathf.Abs(inputX) > Mathf.Epsilon;

        playerAnimator.SetBool("isRunning", isMovingHorizontally);

        if (isMovingHorizontally)
        {
            transform.localScale = new Vector2(Mathf.Sign(inputX) * playerScaleMultiplier, playerScaleMultiplier);
        }
    }

    private void Run()
    {
        #region Run ...
        float inputX = playerInput.Player.Movement.ReadValue<Vector2>().x;

        FlipSprite(inputX);

        float targetSpeed = inputX * runSpeed;
        float runSpeedDifference = targetSpeed - playerRigidbody.velocity.x;
        float accelerationRate = (Mathf.Abs(targetSpeed) > Mathf.Epsilon) ? acceleration : deceleration;
        float movement = Mathf.Pow(Mathf.Abs(runSpeedDifference) * accelerationRate, velocityPower) * Mathf.Sign(runSpeedDifference);

        playerRigidbody.AddForce(movement * Vector2.right);
        #endregion

        #region Friction ...
        GroundedCheck();
        if (isGrounded && Mathf.Abs(inputX) < Mathf.Epsilon)
        {
            float amount = Mathf.Min(Mathf.Abs(inputX), Mathf.Abs(frictionAmount));

            amount *= Mathf.Sign(inputX);

            playerRigidbody.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }
        #endregion
    }

    private void Initialize()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerCollider = GetComponent<Collider2D>();

        playerInput = new PlayerInputAction();
        playerInput.Enable();

        playerInput.Player.Jump.performed += Jump;
    }
}
