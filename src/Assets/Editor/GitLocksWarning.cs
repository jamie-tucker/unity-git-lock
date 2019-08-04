using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Diagnostics;

public class GitLocksWarning : UnityEditor.AssetModificationProcessor {
  private static string[] OnWillSaveAssets(string[] paths) {
    WarningWindow.Init("OnWillSaveAssets");

    ProcessStartInfo processStartInfo = new ProcessStartInfo(); 
    processStartInfo.FileName = Application.dataPath+"/git-locks.sh";
    // processStartInfo.FileName = System.IO.Path.GetFullPath("Packages/com.jamie-tucker.git-locks/git-locks.sh");

    Process process = Process.Start(processStartInfo); 
    string strOutput = process.StandardOutput.ReadToEnd(); 
    process.WaitForExit(); 
    UnityEngine.Debug.Log(strOutput);

    foreach (string path in paths)
      UnityEngine.Debug.Log(path);
    return paths;
  }
}