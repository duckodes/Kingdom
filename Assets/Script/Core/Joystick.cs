using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : Base, IStart, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public static Joystick Instance { get; private set; }

    [SerializeField] private RectTransform baseHandle;
    [SerializeField] private RectTransform handle;
    [SerializeField] private float handleLimit = 100f;

    private Vector2 inputVector;
    public void OnStart()
    {
        Instance = this;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            baseHandle, eventData.position, eventData.pressEventCamera, out pos);

        pos = Vector2.ClampMagnitude(pos, handleLimit);
        handle.anchoredPosition = pos;

        inputVector = pos / handleLimit;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        handle.anchoredPosition = Vector2.zero;
        inputVector = Vector2.zero;
    }

    public float Horizontal => inputVector.x;
    public float Vertical => inputVector.y;
    public Vector2 Direction => inputVector;

}