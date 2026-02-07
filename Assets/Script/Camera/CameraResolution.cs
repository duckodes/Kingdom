using UnityEngine;

public class CameraResolution : Base, IStart, IUpdate
{
    public bool landscape = true;
    public bool limitWidth = false;
    public bool limitHeight = false;

    public float targetAspect = 16f / 9f;

    private Camera cam;

    public void OnStart()
    {
        cam = GetComponent<Camera>();
    }
    public void OnUpdate()
    {
        if (cam == null) return;

        float screenAspect = (float)Screen.width / Screen.height;
        Rect rect = new Rect(0, 0, 1, 1);

        if (limitWidth && screenAspect > targetAspect)
        {
            float scale = targetAspect / screenAspect;
            rect.width = scale;
            rect.x = (1f - scale) / 2f;
        }

        if (limitHeight && screenAspect < targetAspect)
        {
            float scale = screenAspect / targetAspect;
            rect.height = scale;
            rect.y = (1f - scale) / 2f;
        }

        cam.rect = rect;

    }
}