using UnityEngine;

public class Player : MonoBehaviour
{
    #region Components References ...
    private Rigidbody2D playerRigidbody;
    private PlayerInputAction playerInput;
    #endregion

    #region Attributes ...
    [Header("Run")]
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float acceleration = 2f;
    [SerializeField] float deceleration = 2f;
    [SerializeField] float velocityPower = 3f;
    #endregion

    private void Awake()
    {
        Initialize();
    }

    private void FixedUpdate()
    {
        Run();
    }

    private void Run()
    {
        float inputVector = playerInput.Player.Movement.ReadValue<Vector2>().x;

        float targetSpeed = inputVector * runSpeed;
        float runSpeedDifference = targetSpeed - playerRigidbody.velocity.x;
        float accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
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
