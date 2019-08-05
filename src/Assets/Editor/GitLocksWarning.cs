using System.Diagnostics;
using System.Text;

public class GitLocksWarning : UnityEditor.AssetModificationProcessor {
  public static bool dontShowAgain;

  private static void OnWillSaveAssets(string[] paths) {
    Process process;
    string standardOutput;
    bool fileIsLocked = false;
    StringBuilder output = new StringBuilder();

    ProcessStartInfo processStartInfo = new ProcessStartInfo() {
        FileName = "/usr/local/bin/git-lfs",
        UseShellExecute = false,
        RedirectStandardError = true,
        RedirectStandardInput = true,
        RedirectStandardOutput = true,
        CreateNoWindow = true,
        Arguments = "locks"
    }; 

    process = Process.Start(processStartInfo); 
    standardOutput = process.StandardOutput.ReadToEnd(); 
    process.WaitForExit();

    foreach (string path in paths) {
      if(standardOutput.Contains(path)) {
        fileIsLocked = true;
        output.Append(path+"\n");
      }
    }

    if(fileIsLocked && !dontShowAgain) {
        WarningWindow.Show(output.ToString());
    }
  }
}