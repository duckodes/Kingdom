using UnityEngine;

public class CameraFollow : Base, ILateUpdate
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform minBounds;
    [SerializeField] private Transform maxBounds;

    public void OnLateUpdate()
    {
        Vector3 targetPos = player.position;

        float clampedX = Mathf.Clamp(targetPos.x, minBounds.position.x, maxBounds.position.x);
        float clampedY = Mathf.Clamp(targetPos.y, minBounds.position.y, maxBounds.position.y);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}