using UnityEngine;

public class SkeletonMovement : MonoBehaviour
{
    #region Component References ...
    Rigidbody2D skeletonRigidbody;
    BoxCollider2D skeletonOutOfGroundCollider;
    #endregion

    #region Attributes ...
    [Header("Walk")]
    [SerializeField] float walkSpeed = 5f;

    bool isWalking;
    #endregion

    private void Awake()
    {
        Initialize();
    }

    private void FixedUpdate()
    {
        if (isWalking)
        {
            Walk();
        }
    }

    private void Walk()
    {
        if (IsFacingRight())
        {
            skeletonRigidbody.velocity = new Vector2(walkSpeed, 0);
        }
        else
        {
            skeletonRigidbody.velocity = new Vector2(-walkSpeed, 0);
        }
    }

    private bool IsFacingRight() => transform.localScale.x > 0;

    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.localScale = new Vector2(-Mathf.Sign(skeletonRigidbody.velocity.x), 1f);
    }

    private void Initialize()
    {
        skeletonRigidbody = GetComponent<Rigidbody2D>();
        skeletonOutOfGroundCollider = GetComponent<BoxCollider2D>();
        isWalking = true;
    }
}
