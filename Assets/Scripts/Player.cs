using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region Components References ...
    Rigidbody2D playerRigidbody;
    PlayerInputAction playerInput;
    Animator playerAnimator;
    CapsuleCollider2D playerBodyCollider;
    BoxCollider2D playerFeetCollider;
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
    [Space(10f)]
    [SerializeField] float climbSpeed = 5f;

    float playerScaleMultiplier = 10f;
    bool isGrounded;
    bool isJumping;
    bool isClimbing;
    #endregion

    private void Awake()
    {
        Initialize();
    }

    private void FixedUpdate()
    {
        GroundedCheck();
        Run();
        ClimbLadder();
    }

    private void GroundedCheck()
    {
        if (playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
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

    private void ClimbLadder()
    {
        if (!playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")) || isJumping)
        {
            playerRigidbody.gravityScale = 1f;
            isClimbing = false;
            playerAnimator.SetBool("isClimbingLadder", isClimbing);
            return;
        }

        if ((isClimbing && isGrounded) || (playerRigidbody.velocity.y < Mathf.Epsilon && !isJumping && !isClimbing))
        {
            playerRigidbody.gravityScale = 1f;
            isClimbing = false;
            playerAnimator.SetBool("isClimbingLadder", isClimbing);
            return;
        }

        float inputY = playerInput.Player.Movement.ReadValue<Vector2>().y;
        Vector2 climbVelocity = new Vector2(playerRigidbody.velocity.x, inputY * climbSpeed);

        playerRigidbody.velocity = climbVelocity;

        bool playerHasVerticalSpeed = playerRigidbody.velocity.y > Mathf.Epsilon;
        if (playerHasVerticalSpeed && !isGrounded)
        {
            playerRigidbody.gravityScale = 0f;
            isClimbing = true;
            playerAnimator.SetBool("isClimbingLadder", isClimbing);
        }
    }

    private void Jump(InputAction.CallbackContext context)
    {
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
        playerBodyCollider = GetComponent<CapsuleCollider2D>();
        playerFeetCollider = GetComponent<BoxCollider2D>();

        playerInput = new PlayerInputAction();
        playerInput.Enable();

        playerInput.Player.Jump.performed += Jump;
    }
}
