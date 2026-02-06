using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

[Serializable]
public class Keys
{
    public KeyCode Jump = KeyCode.Space;
    public KeyCode MoveLeft = KeyCode.A;
    public KeyCode MoveRight = KeyCode.D;
    public KeyCode MoveDown = KeyCode.S;
}
public class KeyCodes : Base, IAwake
{
    private static Keys keys;
    public static KeyCode Jump => keys.Jump;
    public static KeyCode MoveLeft => keys.MoveLeft;
    public static KeyCode MoveRight => keys.MoveRight;
    public static KeyCode MoveDown => keys.MoveDown;
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
            string jsonData = PlayerPrefs.GetString(nameof(Keys));
            keys = JsonUtility.FromJson<Keys>(jsonData);

            if (!HasAllFields(jsonData))
            {
                SetKeys();
            }
        }
        else
        {
            keys = new Keys();
            SetKeys();
        }
    }
    public static bool HasAllFields(string json)
    {
        var matches = Regex.Matches(json, "\"(\\w+)\"\\s*:");

        var jsonFields = matches
            .Select(m => m.Groups[1].Value)
            .ToHashSet();

        var keyFields = typeof(Keys)
            .GetFields(BindingFlags.Public | BindingFlags.Instance)
            .Select(f => f.Name)
            .ToHashSet();

        return jsonFields.SetEquals(keyFields);
    }

}
