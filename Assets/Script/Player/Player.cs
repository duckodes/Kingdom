using UnityEngine;

public class Player : Base, IStart, IUpdate, IFixedUpdate
{
    private Vector3 velocity;
    private float width;
    private float height;
    private bool isJump;
    private bool isGround;
    public void OnStart()
    {
        width = gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
        height = gameObject.GetComponent<SpriteRenderer>().bounds.size.y;
        Debug.Log(height);
    }
    public void OnUpdate()
    {
        if (isGround && Input.GetKey(KeyCodes.Jump))
        {
            isJump = true;
            velocity.y = 5f;
        }
        if (Input.GetKey(KeyCodes.MoveLeft))
        {
            transform.position -= transform.right * Time.deltaTime;
        }
        if (Input.GetKey(KeyCodes.MoveRight))
        {
            transform.position += transform.right * Time.deltaTime;
        }
    }
    public void OnFixedUpdate()
    {
        if (IsGround() && !isJump)
        {
            velocity = Vector2.zero;
        }
        else
        {
            isJump = false;
            velocity += Physics.gravity * Time.deltaTime;
            transform.position += velocity * Time.deltaTime;
        }

    }
    private bool IsGround()
    {
        float halfWidth = width * 0.5f;
        float halfHeight = height * 0.5f;

        Vector3 leftOrigin = transform.position + Vector3.left * halfWidth + Vector3.down * halfHeight;
        Vector3 midOrigin = transform.position + height * 0.5f * Vector3.down;
        Vector3 rightOrigin = transform.position + Vector3.right * halfWidth + Vector3.down * halfHeight;

        bool leftHit = Physics2D.Raycast(leftOrigin, Vector2.down, 0.01f).collider != null;
        bool midHit = Physics2D.Raycast(midOrigin, Vector2.down, 0.01f).collider != null;
        bool rightHit = Physics2D.Raycast(rightOrigin, Vector2.down, 0.01f).collider != null;

        return isGround = (leftHit || midHit || rightHit);

    }

}
