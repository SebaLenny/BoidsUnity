using UnityEngine;
using UnityEditor;

public class DebugController : MonoBehaviour
{
    public bool useSceneView = false;

    private void Awake()
    {
        if (useSceneView)
        {
            UnityEditor.SceneView.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));
            EditorApplication.pauseStateChanged += SceneSwitcher;
        }
    }

    private static void SceneSwitcher(PauseState state)
    {
        Debug.Log(state);
        UnityEditor.SceneView.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));
    }
}