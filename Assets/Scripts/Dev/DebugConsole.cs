using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class DebugConsole : MonoBehaviour
{
    private TMP_Text consoleText; 
    private List<string> logMessages = new List<string>();


    private void Awake()
    {
        consoleText = GetComponent<TMP_Text>();
    }

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