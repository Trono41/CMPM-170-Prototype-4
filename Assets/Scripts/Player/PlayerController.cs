using System.Collections;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    InputAction move;
    InputAction jump;
    InputAction dash;
    InputAction reset;
    Rigidbody2D rb;
    public float moveSpeed;
    public float jumpForce;
    public float dashSpeed;
    public float dashCooldown;
    public float dashDuration;
    int jumpCount = 0;
    int maxJumps = 2;
    bool isDashing = false;
    bool canDash = true;

    void Start()
    {
        move = InputSystem.actions.FindAction("Move");
        jump = InputSystem.actions.FindAction("Jump");
        dash = InputSystem.actions.FindAction("Dash");
        reset = InputSystem.actions.FindAction("reset");
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isDashing)
            return;

        Vector2 moveValue = move.ReadValue<Vector2>();
        rb.linearVelocity = new Vector2(moveValue.x * moveSpeed, rb.linearVelocity.y);

        if (Mathf.Abs(rb.linearVelocity.y) < 0.01f)
        {
            jumpCount = 0;
        }

        if (jump.WasPressedThisFrame() && jumpCount < maxJumps)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpCount++;
        }

        if (dash.WasPressedThisFrame() && canDash)
        {
            StartCoroutine(Dash(moveValue, moveValue.x));
        }

        if (reset.WasPressedThisFrame())
        {
            ResetScene();
        }
    }

    IEnumerator Dash(Vector2 movement, float direction)
    {
        canDash = false;
        isDashing = true;

        if (direction != 0)
            rb.linearVelocity = movement * dashSpeed;
        else
            rb.linearVelocity = new Vector2(dashSpeed, 0f);

        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
