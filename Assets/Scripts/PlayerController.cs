using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    InputAction move;
    InputAction jump;
    Rigidbody rb;
    float moveSpeed = 2f;
    float jumpForce = 5f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        move = InputSystem.actions.FindAction("Move");
        jump = InputSystem.actions.FindAction("Jump");
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveValue = move.ReadValue<Vector2>();
        /*if (move.IsPressed())
        {
            Debug.Log("Move Pressed! moveValue: " + moveValue);
        }*/

        if (jump.IsPressed())
        {
            // Debug.Log("Jump pressed!");
            rb.AddForce(transform.up * jumpForce);
        }
    }
}
