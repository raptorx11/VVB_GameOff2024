using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoreHack : MonoBehaviour
{
    private bool isGameStarted = false;

    [Header("Console Fields")]
    [SerializeField] private TMP_Text outputText;
    [SerializeField] private TMP_InputField commandInput;

    [Header("Modules")]
    [SerializeField] private GameObject hackModules;
    private ScanModule scanModule;

    // Start is called before the first frame update
    void Start()
    {
        scanModule = hackModules.GetComponent<ScanModule>();
        commandInput.enabled = true;

        commandInput.onSubmit.AddListener(ProcessCommand);
        commandInput.ActivateInputField();
    }

    private void ProcessCommand(string command)
    {
        outputText.text += "\n";
        if (!isGameStarted)
        {
            switch(command)
            {
                case "start":
                    outputText.text += "System initiated!";
                    isGameStarted=true;
                    break;
                case "exit":
                    outputText.text += "System closing!";
                    break;
                case "help":
                    outputText.text += "type start to start game..." +
                        "type exit to quit game...";
                    break;
                default:
                    outputText.text += "Invalid command! Type help to learn more!";
                    break;
            }


        } else
        {

            if(command.StartsWith("scan"))
            {

            }


        }

        commandInput.text = "";
        commandInput.ActivateInputField();
    }






}
