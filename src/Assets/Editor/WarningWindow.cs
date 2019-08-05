using UnityEngine;
using UnityEditor;

public class WarningWindow : EditorWindow
{
    private static string message;
    private static WarningWindow window;

    public static void Show(string message)
    {
        WarningWindow.message = message;
        window = (WarningWindow)EditorWindow.GetWindow(typeof(WarningWindow), true, "Warning", true);
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Warning, Locked Files!", EditorStyles.boldLabel);
        GUILayout.Label(WarningWindow.message);
        GUILayout.Space(10f);
        GitLocksWarning.dontShowAgain = GUILayout.Toggle(GitLocksWarning.dontShowAgain, "Don't Show Again");
        GUILayout.Space(10f);
        if(GUILayout.Button("Okay")) {
            window.Close();
        }
    }
}