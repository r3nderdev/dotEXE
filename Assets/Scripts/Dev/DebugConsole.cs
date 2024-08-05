using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class DebugConsole : MonoBehaviour
{
    public TMP_Text consoleText; // Assign your Text component here
    private List<string> logMessages = new List<string>();

    void OnEnable()
    {
        Application.logMessageReceived += LogMessage;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= LogMessage;
    }

    void LogMessage(string logString, string stackTrace, LogType type)
    {
        logMessages.Add(logString);
        UpdateConsoleText();
    }

    void UpdateConsoleText()
    {
        consoleText.text = string.Join("\n", logMessages);
    }
}