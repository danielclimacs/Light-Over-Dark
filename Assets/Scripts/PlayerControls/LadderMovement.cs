using UnityEngine;
using UnityEngine.InputSystem;

public class LadderMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed = 8f;

    private float vertical;
    private bool isLadder;
    private bool isClimbing;
    private Collider2D playerCollider;
    private Collider2D currentPlatformCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
    }

    public void OnMove(InputValue value)
    {
        vertical = value.Get<Vector2>().y;

        if (isLadder && Mathf.Abs(vertical) > 0f)
        {
            isClimbing = true;
        }
    }

    private void FixedUpdate()
    {
        if (isClimbing)
        {
            if (currentPlatformCollider != null)
            {
                Physics2D.IgnoreCollision(playerCollider, currentPlatformCollider, true);

            }
            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, vertical * speed);
        }
        else
        {
            if (currentPlatformCollider != null)
            {
                Physics2D.IgnoreCollision(playerCollider, currentPlatformCollider, false);

            }
            rb.gravityScale = 4f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;
            isClimbing = false;
            if (currentPlatformCollider != null)
            {
                Physics2D.IgnoreCollision(playerCollider, currentPlatformCollider, false);
                currentPlatformCollider = null;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlatformEffector2D>() != null)
        {
            currentPlatformCollider = collision.collider;
        }
    }
}