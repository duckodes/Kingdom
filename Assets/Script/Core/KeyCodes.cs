using System;
using UnityEngine;

[Serializable]
public class Keys
{
    public KeyCode Jump = KeyCode.Space;
    public KeyCode MoveLeft = KeyCode.A;
    public KeyCode MoveRight = KeyCode.D;
}
public class KeyCodes : Base, IAwake
{
    private static Keys keys;
    public static KeyCode Jump => keys.Jump;
    public static KeyCode MoveLeft => keys.MoveLeft;
    public static KeyCode MoveRight => keys.MoveRight;
    public void OnAwake()
    {
        UpdateKeys();
    }
    public static void SetKeys()
    {
        PlayerPrefs.SetString(nameof(Keys), JsonUtility.ToJson(keys));
        PlayerPrefs.Save();
    }
    public static void UpdateKeys()
    {
        if (PlayerPrefs.HasKey(nameof(Keys)))
        {
            keys = JsonUtility.FromJson<Keys>(PlayerPrefs.GetString(nameof(Keys)));
        }
        else
        {
            keys = new Keys();
            SetKeys();
        }
    }
}
