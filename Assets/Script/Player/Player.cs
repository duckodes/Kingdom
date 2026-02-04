using UnityEngine;

public class Player : Base, IStart, IUpdate
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float jumpForce = 7.0f;
    private float width;
    private float height;
    public void OnStart()
    {
        width = gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
        height = gameObject.GetComponent<SpriteRenderer>().bounds.size.y;
    }
    public void OnUpdate()
    {
        if (IsGround() && Input.GetKeyDown(KeyCodes.Jump))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
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
        return Physics2D.Raycast(transform.position + height * 0.4f * Vector3.down, Vector2.down, 0.05f).collider != null;
    }
}
