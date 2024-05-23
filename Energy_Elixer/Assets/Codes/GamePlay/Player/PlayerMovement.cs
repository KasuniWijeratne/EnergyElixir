using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerControls controls;
    float direction = 0; // 0 = idle, 1 = right, -1 = left

    public Rigidbody2D playerRB;
    public Animator animator;
    public float speed = 400;
    bool isFacingRight = true;
    public float jumpForce = 5;
    bool isGrounded;
    int numberOfJumps = 0;

    public Transform groundCheck;
    public LayerMask groundLayer;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Enable();
        controls.Ground.Move.performed += ctx => {
            direction = ctx.ReadValue<float>();
            };

        controls.Ground.Jump.performed += ctx => {Jump ();};
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
        animator.SetBool("isGrounded", isGrounded);
        playerRB.velocity = new Vector2(direction * speed * Time.fixedDeltaTime, playerRB.velocity.y);
        animator.SetFloat("speed", Mathf.Abs(direction));

        if (direction > 0 && !isFacingRight || direction < 0 && isFacingRight)
            Flip();

    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
    }

    void Jump(){
        if (isGrounded)
            {
                numberOfJumps = 0;
                playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
                numberOfJumps++;
            }
        else{
            if(numberOfJumps == 1){
                playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
                numberOfJumps++;
                
            }
        }
    }
}
