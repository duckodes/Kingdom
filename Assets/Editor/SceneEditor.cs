#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.ShortcutManagement;
using UnityEngine;

[InitializeOnLoad]
public static class SceneEditor
{
    static SceneEditor()
    {
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }
    [Shortcut("Play", KeyCode.F5)]
    public static void PlayGame()
    {
        if (EditorApplication.isPlaying)
        {
            EditorApplication.isPlaying = false;
        }
        else
        {
            SessionState.SetString("PreviousScenePath", EditorSceneManager.GetActiveScene().path);

            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene("Assets/Scenes/init.unity");
                EditorApplication.isPlaying = true;
            }
        }
    }
    private static void OnPlayModeChanged(PlayModeStateChange state)
    {
        switch (state)
        {
            case PlayModeStateChange.EnteredEditMode:
                string previousScenePath = SessionState.GetString("PreviousScenePath", "");
                if (!string.IsNullOrEmpty(previousScenePath))
                {
                    EditorSceneManager.OpenScene(previousScenePath);
                }
                break;
            case PlayModeStateChange.ExitingEditMode:
                SessionState.SetString("PreviousScenePath", EditorSceneManager.GetActiveScene().path);

                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    EditorSceneManager.OpenScene("Assets/Scenes/init.unity");
                }
                break;
        }
    }

}
#endif