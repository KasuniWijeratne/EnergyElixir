using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerControls controls;
    float direction = 0; // 0 = idle, 1 = right, -1 = left

    public Rigidbody2D playerRB;
    public Animator animator;
    public float speed = 400;
    bool isFacingRight = true;
    public float jumpForce = 7;
    bool isGrounded;
    bool isPlayerActive;
    int numberOfJumps = 0;

    public Transform groundCheck;
    public LayerMask groundLayer;

    public void SetPlayerActive(bool isActive)
    {
        isPlayerActive = isActive;
    }

    public bool GetPlayerActive()
    {
        return isPlayerActive;
    }
    private void Awake()
    {
        SetPlayerActive(true);
        controls = new PlayerControls();
        controls.Enable();
        controls.Ground.Move.performed += ctx =>
        {
            direction = ctx.ReadValue<float>();
        };

        controls.Ground.Jump.performed += ctx =>
        {
            Jump();
        };
    }

    private void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();

        if (playerRB == null)
        {
            Debug.LogError("Rigidbody2D component is not assigned.");
        }

        if (animator == null)
        {
            Debug.LogError("Animator component is not assigned in the Inspector.");
        }

        if (groundCheck == null)
        {
            Debug.LogError("GroundCheck Transform is not assigned in the Inspector.");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.isGameOver)
            return;
        if (isPlayerActive)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
            animator.SetBool("isGrounded", isGrounded);
            playerRB.velocity = new Vector2(
                direction * speed * Time.fixedDeltaTime,
                playerRB.velocity.y
            );
            animator.SetFloat("speed", Mathf.Abs(direction));

            if (direction > 0 && !isFacingRight || direction < 0 && isFacingRight)
                Flip();
        }
    }

    void Flip()
    {
        if (GameManager.isGameOver || playerRB == null)
            return;
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
    }

    void Jump()
    {
        if (GameManager.isGameOver || playerRB == null)
            return; //null check to stop accessing player when the object is destroyed
        if (isGrounded)
        {
            numberOfJumps = 0;
            playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
            numberOfJumps++;
        }
        else
        {
            if (numberOfJumps == 1)
            {
                playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
                numberOfJumps++;
            }
        }
    }
}
