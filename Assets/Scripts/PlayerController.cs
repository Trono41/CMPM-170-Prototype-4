using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    InputAction move;
    InputAction jump;
    Rigidbody2D rb;
    public float moveSpeed;
    public float maxMoveSpeed = 10f;
    public float jumpForce;
    bool isGrounded = true;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        move = InputSystem.actions.FindAction("Move");
        jump = InputSystem.actions.FindAction("Jump");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveValue = move.ReadValue<Vector2>();
        rb.linearVelocity = moveValue * moveSpeed;
        if (move.IsPressed())
        {
            Debug.Log("Move Pressed! moveValue: " + moveValue);
        }

        if (jump.IsPressed() && isGrounded)
        {
            // Debug.Log("Jump pressed!");
        }
    }
}
