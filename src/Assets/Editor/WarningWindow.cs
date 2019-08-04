using UnityEngine;
using UnityEditor;

public class WarningWindow : EditorWindow
{
    private static string message;

    public static void Init(string message)
    {
        WarningWindow.message = message;
        WarningWindow window = (WarningWindow)EditorWindow.GetWindow(typeof(WarningWindow));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label(WarningWindow.message, EditorStyles.boldLabel);
    }
}