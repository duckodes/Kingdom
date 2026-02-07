using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ControlButtons : Base, IStart, IPointerDownHandler, IPointerUpHandler
{
    public static ControlButtons Instance { get; private set; }

    [SerializeField] private Button jumpButton;
    public bool JumpButtonPointerDown;
    public bool JumpButtonPointerUp;

    public void OnStart()
    {
        Instance = this;

        jumpButton.AddComponent<PointerBase>();
    }
    public async void OnPointerDown(PointerEventData eventData)
    {
        JumpButtonPointerDown = true;
        JumpButtonPointerUp = false;
        await Wait.Milliseconds(10, out Action cancel);
        if (JumpButtonPointerDown)
        {
            JumpButtonPointerDown = false;
        }
    }

    public async void OnPointerUp(PointerEventData eventData)
    {
        JumpButtonPointerDown = false;
        JumpButtonPointerUp = true;
        await Wait.Milliseconds(10, out Action cancel);
        if (JumpButtonPointerUp)
        {
            JumpButtonPointerUp = false;
        }
    }

    private class PointerBase : Base, IPointerDownHandler, IPointerUpHandler
    {
        private GameObject Target => ControlButtons.Instance.gameObject;
        public void OnPointerDown(PointerEventData eventData)
        {
            ExecuteEvents.Execute<IPointerDownHandler>(
                Target,
                eventData,
                (handler, data) => handler.OnPointerDown((PointerEventData)data)
            );
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            ExecuteEvents.Execute<IPointerUpHandler>(
                Target,
                eventData,
                (handler, data) => handler.OnPointerUp((PointerEventData)data)
            );
        }
    }

}