using System;
using System.Diagnostics;
using System.Text;
using UnityEngine;
using UnityEditor;

public class GitLocksWarning : UnityEditor.AssetModificationProcessor
{
  private const int TIMEOUT = 1000;
  private const string FILE_NAME = "/usr/local/bin/git-lfs"; // which git-lfs
  private const string ARGS = "locks"; // can also use --json and parse the output for better information display.

  private static void OnWillSaveAssets(string[] paths)
  {
    string standardOutput = string.Empty;
    bool fileIsLocked = false;
    StringBuilder output = new StringBuilder("YOU ARE EDITING A LOCKED FILE!\n");

    try
    {
      using (Process process = new Process())
      {
        ProcessStartInfo processStartInfo = new ProcessStartInfo()
        {
          FileName = FILE_NAME,
          UseShellExecute = false,
          RedirectStandardOutput = true,
          CreateNoWindow = true,
          Arguments = ARGS
        };
        process.StartInfo = processStartInfo;
        process.Start();
        standardOutput = process.StandardOutput.ReadToEnd();

        bool waitSucceeded = process.WaitForExit(TIMEOUT);

        if (waitSucceeded)
        {
          process.WaitForExit();
        }
        else
        {
          process.Close();
          throw (new Exception("Process Timed out"));
        }
      }
    }
    catch (Exception e)
    {
      UnityEngine.Debug.LogError(e);
    }

    if (!string.IsNullOrEmpty(standardOutput))
    {
      UnityEngine.Debug.LogWarning(output);

      foreach (string path in paths)
      {
        if (standardOutput.Contains(path))
        {
          fileIsLocked = true;
          string file = "\n<color=cyan>" + path + "</color>";
          UnityEngine.Debug.Log(file);
          output.Append(file);
        }
      }

      if (fileIsLocked && !WarningWindow.DontShowAgain)
      {
        WarningWindow.Show(output.ToString());
      }
    }
  }
}