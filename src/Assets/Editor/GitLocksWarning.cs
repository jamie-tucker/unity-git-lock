using System;
using System.Diagnostics;
using System.Text;
using UnityEngine;
using UnityEditor;

public class GitLocksWarning : UnityEditor.AssetModificationProcessor
{
  private const int TIMEOUT = 3000;
  private const string FILE_NAME = "/usr/local/bin/git-lfs"; // which git-lfs
  private const string ARGS = "locks"; // can also use --json and parse the output for better information display.

  private static void OnWillSaveAssets(string[] paths)
  {
    string processOutput = string.Empty;
    bool fileIsLocked = false;
    StringBuilder output = new StringBuilder("YOU ARE EDITING A LOCKED FILE!\n");

    processOutput = RunProcess(FILE_NAME, ARGS);

    if (!string.IsNullOrEmpty(processOutput))
    {
      UnityEngine.Debug.LogWarning(output);

      foreach (string path in paths)
      {
        if (processOutput.Contains(path))
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

  private static string RunProcess(string fileName, string args)
  {
    string standardOutput = string.Empty;

    try
    {
      using (Process process = new Process())
      {
        ProcessStartInfo processStartInfo = new ProcessStartInfo()
        {
          FileName = fileName,
          UseShellExecute = false,
          RedirectStandardOutput = true,
          CreateNoWindow = true,
          Arguments = args
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

    return standardOutput;
  }
}