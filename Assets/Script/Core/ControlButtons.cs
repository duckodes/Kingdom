using System;
using UnityEngine;
using UnityEngine.UI;

public class ControlButtons : Base, IStart
{
    public static ControlButtons Instance { get; private set; }

    [SerializeField] private Button jumpButton;
    public bool JumpButtonClicked;

    public void OnStart()
    {
        Instance = this;

        jumpButton.onClick.AddListener(async delegate
        {
            JumpButtonClicked = true;
            await Wait.Milliseconds(100, out Action cancel);
            if (JumpButtonClicked)
            {
                JumpButtonClicked = false;
            }
        });
    }

}