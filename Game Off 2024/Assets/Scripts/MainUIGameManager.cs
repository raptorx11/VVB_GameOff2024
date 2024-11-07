using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainUIGameManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField consoleInput;
    [SerializeField] private TextMeshProUGUI outputText;

    private string breakline = "\n";

    // Start is called before the first frame update
    void Start()
    {
        consoleInput.text = "";
        outputText.text = "Enter a command, start, load, options, exit..." + breakline;

        consoleInput.onSubmit.AddListener(ProcessCommand);

        consoleInput.ActivateInputField();
    }

    private void ProcessCommand(string consoleText)
    {
        consoleText = consoleText.ToLower();

        switch(consoleText)
        {
            case "start":
                outputText.text += breakline + "Starting game... please wait...";
                break;
            case "load":
                outputText.text += breakline + "Loading game... please wait...";
                break;
            case "options":
                outputText.text += breakline + "Select options";
                break;
            case "exit":
                outputText.text += breakline + "Exiting game...";
                StartCoroutine(ExitCoro());
                break;
            default:
                outputText.text += breakline + "Invalid command... Please try again...";
                break;
        }

        consoleInput.text = "";
        consoleInput.ActivateInputField();
    }

    private IEnumerator ExitCoro()
    {
        outputText.text += breakline + "Clearing Traces...";
        yield return new WaitForSeconds(1f);
        outputText.text += breakline + "Shutting hack module...";
        yield return new WaitForSeconds(1f);

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

}
