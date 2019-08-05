using UnityEngine;
using UnityEditor;

public class WarningWindow : EditorWindow
{
  private const string MENU_NAME = "Developer/Show Git Lock Warning";

  public static bool DontShowAgain
  {
    get;
    private set;
  }

  private static string message;
  private static WarningWindow window;
  private static GUIStyle richStyle = EditorStyles.boldLabel;

  public static void Show(string message)
  {
    EditorStyles.boldLabel.richText = true;
    EditorStyles.label.richText = true;
    WarningWindow.message = message;
    window = (WarningWindow)EditorWindow.GetWindow(typeof(WarningWindow), true, "Warning", true);
    window.Show();
  }

  void OnGUI()
  {
    GUILayout.Label("<b><color=yellow>WARNING</color></b>", EditorStyles.boldLabel);
    GUILayout.Label(WarningWindow.message, EditorStyles.label);
    GUILayout.Space(24f);
    DontShowAgain = GUILayout.Toggle(DontShowAgain, "Don't Show Again");
    GUILayout.Space(12f);
    if (GUILayout.Button("Okay"))
    {
      window.Close();
    }
  }

  #region Menu
  [MenuItem(MENU_NAME)]
  private static void ToggleGitLock()
  {
    DontShowAgain = !DontShowAgain;
    Menu.SetChecked(MENU_NAME, !DontShowAgain);
  }

  [MenuItem(MENU_NAME, true)]
  private static bool ToggleGitLockValidate()
  {
    Menu.SetChecked(MENU_NAME, !DontShowAgain);
    return true;
  }
  #endregion
}