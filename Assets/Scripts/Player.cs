using UnityEngine;

public class Player : MonoBehaviour
{
    #region Components References ...
    Rigidbody2D playerRigidbody;
    PlayerInputAction playerInput;
    #endregion

    #region Attributes ...
    [Header("Run")]
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float acceleration = 2f;
    [SerializeField] float deceleration = 2f;
    [SerializeField] float velocityPower = 3f;

    float playerScaleMultiplier = 10f;
    #endregion

    private void Awake()
    {
        Initialize();
    }

    private void FixedUpdate()
    {
        Run();
    }

    private void FlipSprite(float inputX)
    {
        bool isMovingHorizontally = Mathf.Abs(inputX) > Mathf.Epsilon;

        if(isMovingHorizontally)
        {
            transform.localScale =  new Vector2(Mathf.Sign(inputX) * playerScaleMultiplier, playerScaleMultiplier);
        }
    }

    private void Run()
    {
        float inputX = playerInput.Player.Movement.ReadValue<Vector2>().x;

        FlipSprite(inputX);

        float targetSpeed = inputX * runSpeed;
        float runSpeedDifference = targetSpeed - playerRigidbody.velocity.x;
        float accelerationRate = (Mathf.Abs(targetSpeed) > Mathf.Epsilon) ? acceleration : deceleration;
        float movement = Mathf.Pow(Mathf.Abs(runSpeedDifference) * accelerationRate, velocityPower) * Mathf.Sign(runSpeedDifference);

        playerRigidbody.AddForce(movement * Vector2.right);
    }

    private void Initialize()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();

        playerInput = new PlayerInputAction();
        playerInput.Enable();
    }
}
