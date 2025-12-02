using System.Collections;
using UnityEngine;

public class TornadoController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float horizontalReleaseForce = 1000f;

    public LayerMask groundLayer;
    public float groundCheckDistance = 1f;
    public float destroyDelayAfterLeavingGround = 1f;

    public Transform leftBound;
    public Transform rightBound;

    [Header("Audio")]
    [SerializeField] private AudioClip windSound;
    [SerializeField] private AudioSource audioSource;

    private Transform capturedPlayer;
    private Transform playerTarget;
    private Rigidbody2D playerRb;
    private PlayerController playerController;
    private float originalGravityScale;
    private bool hasLeftGround = false;
    private float currentMoveDirection = 0f;

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
        if (leftBound != null && rightBound != null)
        {
            if (transform.position.x < leftBound.position.x || transform.position.x > rightBound.position.x)
            {
                ReleasePlayer();
                Destroy(gameObject);
                return;
            }
        }

        if (!IsOnGround())
        {
            if (!hasLeftGround)
            {
                hasLeftGround = true;
                StartCoroutine(DestroyAfterDelay());
            }
        }
        else
        {
            hasLeftGround = false;
        }

        float moveDirection = 0f;

        if (capturedPlayer == null && playerTarget != null)
        {
            float direction = playerTarget.position.x - transform.position.x;
            moveDirection = direction > 0 ? 1f : -1f;
            currentMoveDirection = moveDirection;
        }
        else if (capturedPlayer != null)
        {
            moveDirection = currentMoveDirection;
        }

        if (moveDirection != 0f)
        {
            Vector3 newPosition = transform.position;
            newPosition.x += moveDirection * moveSpeed * Time.deltaTime;
            transform.position = newPosition;
        }
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelayAfterLeavingGround);

        if (!IsOnGround())
        {
            ReleasePlayer();
            Destroy(gameObject);
        }
    }

    private bool IsOnGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
        return hit.collider != null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && capturedPlayer == null && other.transform.parent == null)
        {
            capturedPlayer = other.transform;
            playerRb = capturedPlayer.GetComponent<Rigidbody2D>();
            playerController = capturedPlayer.GetComponent<PlayerController>();

            if (playerRb != null)
            {
                originalGravityScale = playerRb.gravityScale;
                playerRb.linearVelocity = Vector2.zero;
                playerRb.gravityScale = 0f;
            }

            if (playerController != null)
            {
                playerController.enabled = false;
            }

            capturedPlayer.parent = this.transform;

            PlayWindSound();
        }
    }

    private void OnDestroy()
    {
        ReleasePlayer();
    }

    private void ReleasePlayer()
    {
        if (capturedPlayer != null)
        {
            capturedPlayer.parent = null;

            if (playerRb != null)
            {
                playerRb.gravityScale = originalGravityScale;
            }

            if (playerController != null)
            {
                playerController.enabled = true;
            }

            capturedPlayer = null;
            playerRb = null;
            playerController = null;

            StopWindSound();
        }
    }

    private void PlayWindSound()
    {
        if (audioSource != null && windSound != null)
        {
            audioSource.clip = windSound;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    private void StopWindSound()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
}