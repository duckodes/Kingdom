using UnityEngine;

public class Player : Base, IStart, IUpdate
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float jumpForce = 20.0f;
    private const int maxJumpTimes = 2;
    private int jumpTimes;
    private float width;
    private float height;
    private bool isGround;
    public void OnStart()
    {
        width = gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
        height = gameObject.GetComponent<SpriteRenderer>().bounds.size.y;
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
            if (IsGround()) jumpTimes = 0;
            else return;
        }
        if (Input.GetKeyDown(KeyCodes.Jump))
        {
            jumpTimes++;
            rb.AddForce(Vector2.up * jumpForce);
        }
    }
    private void Move()
    {
        float moveInput = 0f;
        if (Input.GetKey(KeyCodes.MoveLeft))
        {
            moveInput = -1f;
        }
        if (Input.GetKey(KeyCodes.MoveRight))
        {
            moveInput = 1f;
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

        return isGround = (leftHit || midHit || rightHit);

    }
}
