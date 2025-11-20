using System.Collections;
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
    public float dashSpeed;
    public float dashCooldown;
    public float dashDuration;
    float direction;
    int dashCooldownRemaining;
    bool isGrounded = true;
    bool isDashing = false;
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
        if (isDashing)
        {
            return;
        }

        Vector2 moveValue = move.ReadValue<Vector2>();
        rb.linearVelocity = moveValue * moveSpeed;
        direction = moveValue.x;

        if (jump.IsPressed() && isGrounded)
        {
            // Debug.Log("Jump pressed!");
        }

        if (dash.WasPressedThisFrame() && canDash)
        {
            StartCoroutine(Dash(moveValue, moveValue.x));
        }
    }

    IEnumerator Dash(Vector2 movement, float direction)
    {
        canDash = false;
        isDashing = true;
        Debug.Log("Dashing!");

        if (direction != 0)
        {
            rb.linearVelocity = dashSpeed * movement;
        }
        else
        {
            rb.linearVelocity = new Vector2(dashSpeed, 0f);
        }
        
        yield return new WaitForSeconds(dashDuration);
        
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }
}
