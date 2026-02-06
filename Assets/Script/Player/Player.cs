using System;
using UnityEngine;

public class Player : Base, IStart, IUpdate
{
    private enum AnimatorMotion
    {
        Idle = 0,
        Run = 1,
    }
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D Collider2D;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float jumpForce = 20.0f;
    private const int maxJumpTimes = 2;
    private int jumpTimes;
    private float width;
    private float height;
    public void OnStart()
    {
        width = spriteRenderer.bounds.size.x;
        height = spriteRenderer.bounds.size.y;
    }
    public void OnUpdate()
    {
        Jump();
        Move();
    }
    private void Jump()
    {
        if (jumpTimes >= maxJumpTimes)
        {
            if (IsGround() && rb.velocity.y < 0)
            {
                jumpTimes = 0;
            }
            else
            {
                return;
            }
        }
        if (Input.GetKeyDown(KeyCodes.Jump) && !Input.GetKey(KeyCodes.MoveDown))
        {
            jumpTimes++;
            rb.AddForce(Vector2.up * jumpForce);
            animator.Play("PlayerJump");
        }
    }
    private async void Move()
    {
        float moveInput = 0f;
        if (Input.GetKey(KeyCodes.MoveLeft) && !Input.GetKey(KeyCodes.MoveRight))
        {
            moveInput = -1f;
            spriteRenderer.flipX = true;
            animator.SetInteger(nameof(AnimatorMotion), (int)AnimatorMotion.Run);
        }
        if (Input.GetKey(KeyCodes.MoveRight) && !Input.GetKey(KeyCodes.MoveLeft))
        {
            moveInput = 1f;
            spriteRenderer.flipX = false;
            animator.SetInteger(nameof(AnimatorMotion), (int)AnimatorMotion.Run);
        }
        if (Input.GetKeyUp(KeyCodes.MoveLeft) || Input.GetKeyUp(KeyCodes.MoveRight) || rb.velocity.x == 0)
        {
            animator.SetInteger(nameof(AnimatorMotion), (int)AnimatorMotion.Idle);
        }
        if (Input.GetKeyDown(KeyCodes.Jump) && Input.GetKey(KeyCodes.MoveDown) && !IsGroundLayer())
        {
            Collider2D.enabled = false;
            await Wait.Milliseconds(300, out Action cancel);
            Collider2D.enabled = true;
        }
        if (Input.GetKeyUp(KeyCodes.MoveDown) || Input.GetKeyUp(KeyCodes.Jump))
        {
            Collider2D.enabled = true;
        }
        if (moveInput != 0)
        {
            rb.velocity = new Vector2(moveInput * 3.0f, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, 0, 3.0f * Time.deltaTime), rb.velocity.y);
        }
    }
    private bool IsGround()
    {
        float halfWidth = width * 0.5f;
        float halfHeight = height * 0.6f;

        Vector3 leftOrigin = transform.position + Vector3.left * halfWidth + Vector3.down * halfHeight;
        Vector3 midOrigin = transform.position + halfHeight * Vector3.down;
        Vector3 rightOrigin = transform.position + Vector3.right * halfWidth + Vector3.down * halfHeight;

        bool leftHit = Physics2D.Raycast(leftOrigin, Vector2.down, 0.2f).collider != null;
        bool midHit = Physics2D.Raycast(midOrigin, Vector2.down, 0.2f).collider != null;
        bool rightHit = Physics2D.Raycast(rightOrigin, Vector2.down, 0.2f).collider != null;

        return leftHit || midHit || rightHit;
    }
    private bool IsGroundLayer()
    {
        float halfHeight = height * 0.6f;
        Vector3 midOrigin = transform.position + halfHeight * Vector3.down;

        return Physics2D.Raycast(midOrigin, Vector2.down, 0.2f).collider.gameObject.layer == LayerMask.NameToLayer("Ground");
    }
}
