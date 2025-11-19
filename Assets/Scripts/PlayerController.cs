using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    InputAction move;
    InputAction jump;
    InputAction dash;
    Rigidbody2D rb;
    public float moveSpeed;
    public float jumpForce;
    public float dashForce;
    float direction;
    bool isGrounded = true;
    bool canDash = true;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        move = InputSystem.actions.FindAction("Move");
        jump = InputSystem.actions.FindAction("Jump");
        dash = InputSystem.actions.FindAction("Dash");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveValue = move.ReadValue<Vector2>();
        rb.linearVelocity = moveValue * moveSpeed;
        direction = moveValue.x;

        if (jump.IsPressed() && isGrounded)
        {
            // Debug.Log("Jump pressed!");
        }

        if (dash.IsPressed())
        {
            if (direction != 0)
            {
                rb.AddForceX(dashForce * direction, ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForceX(dashForce, ForceMode2D.Impulse);
            }
        }
    }
}
