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
        if (Input.GetKeyDown(KeyCodes.Jump) && !Input.GetKey(KeyCodes.MoveDown)
            || ControlButtons.Instance.JumpButtonPointerDown)
        {
            jumpTimes++;
            rb.AddForce(Vector2.up * jumpForce);
            animator.Play("PlayerJump");
            ControlButtons.Instance.JumpButtonPointerDown = false;
        }
    }
    private async void Move()
    {
        float moveInput = 0f;
        // Move Left
        if (Input.GetKey(KeyCodes.MoveLeft) && !Input.GetKey(KeyCodes.MoveRight) 
            || Joystick.Instance.Direction.y >= 0 && Joystick.Instance.Direction.x < -0.35f)
        {
            moveInput = -1f;
            spriteRenderer.flipX = true;
            if (!IsWallLayer())
            {
                animator.SetInteger(nameof(AnimatorMotion), (int)AnimatorMotion.Run);
            }
        }
        // Move Right
        if (Input.GetKey(KeyCodes.MoveRight) && !Input.GetKey(KeyCodes.MoveLeft) 
            || Joystick.Instance.Direction.x > 0.35f)
        {
            moveInput = 1f;
            spriteRenderer.flipX = false;
            if (!IsWallLayer())
            {
                animator.SetInteger(nameof(AnimatorMotion), (int)AnimatorMotion.Run);
            }
        }
        // Stop Anim
        if (Input.GetKeyUp(KeyCodes.MoveLeft)
            || Input.GetKeyUp(KeyCodes.MoveRight)
            || rb.velocity.x == 0)
        {
            animator.SetInteger(nameof(AnimatorMotion), (int)AnimatorMotion.Idle);
        }
        // Go Down
        if (Input.GetKeyDown(KeyCodes.Jump) && Input.GetKey(KeyCodes.MoveDown) && !IsGroundLayer()
            || Joystick.Instance.Direction.y < 0 && ControlButtons.Instance.JumpButtonPointerDown && !IsGroundLayer())
        {
            animator.Play("PlayerDown");
            Collider2D.enabled = false;
            await Wait.Milliseconds(300, out Action cancel);
            Collider2D.enabled = true;
        }
        // Collider Limit
        if (Input.GetKeyUp(KeyCodes.MoveDown)
            || Input.GetKeyUp(KeyCodes.Jump)
            || ControlButtons.Instance.JumpButtonPointerUp
            || IsGroundLayer()
            || IsWallLayer())
        {
            Collider2D.enabled = true;
        }
        // Stick On Wall Velocity with Anim
        if (IsWallLayer() && !IsGroundLayer())
        {
            rb.velocity = new Vector2(rb.velocity.x, -3.0f);
            animator.Play("PlayerDown");
        }
        // MoveLeft or MoveRight
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
        var hit = Physics2D.Raycast(midOrigin, Vector2.down, 0.2f).collider;
        if (hit != null)
        {
            return hit.gameObject.layer == LayerMask.NameToLayer("Ground");
        }
        return false;
    }
    private bool IsWallLayer()
    {
        float halfWidth = width * 0.5f;
        Vector3 leftOrigin = transform.position + halfWidth * Vector3.left;
        Vector3 rightOrigin = transform.position + halfWidth * Vector3.right;
        var hitLeft = Physics2D.Raycast(leftOrigin, Vector2.left, 0.1f).collider;
        var hitRight = Physics2D.Raycast(rightOrigin, Vector2.right, 0.1f).collider;

        bool leftIsWall = hitLeft != null && hitLeft.gameObject.layer == LayerMask.NameToLayer("Wall");
        bool rightIsWall = hitRight != null && hitRight.gameObject.layer == LayerMask.NameToLayer("Wall");

        return leftIsWall || rightIsWall;
    }
}
