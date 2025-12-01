using UnityEngine;

public class TornadoController : MonoBehaviour
{
    public float moveSpeed = 5f;

    public LayerMask groundLayer;
    public float groundCheckDistance = 0.6f;
    public float verticalSpawnOffset = 1.0f;

    public float horizontalReleaseForce = 1000f;

    // Changed from onlyOwner to capturedPlayer for clarity
    private Transform capturedPlayer;
    private Transform playerTarget;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTarget = playerObj.transform;
        }
    }

    void Update()
    {
        float moveDirection = 0f;

        if (capturedPlayer == null && playerTarget != null)
        {
            float direction = playerTarget.position.x - transform.position.x;
            moveDirection = direction > 0 ? 1f : -1f;
        }
        else if (capturedPlayer != null)
        {
            moveDirection = 1f;
        }

        if (moveDirection != 0f)
        {
            Vector3 newPosition = transform.position;
            newPosition.x += moveDirection * moveSpeed * Time.deltaTime;
            transform.position = newPosition;
        }
    }

    // NEW LOGIC: Use OnTriggerStay2D to enforce capture
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // If the player's parent is NOT this tornado, re-parent them.
            // This prevents the player from walking out of the trigger zone.
            if (other.transform.parent != this.transform)
            {
                capturedPlayer = other.transform;
                capturedPlayer.parent = this.transform;

                Rigidbody2D playerRb = capturedPlayer.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    // Clear velocity whenever they are re-captured/held
                    playerRb.linearVelocity = Vector2.zero;
                }
            }
        }
    }

    // Removed the redundant OnTriggerEnter2D method

    private void OnDestroy()
    {
        if (capturedPlayer != null)
        {
            capturedPlayer.parent = null;
        }
    }
}