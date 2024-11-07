using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField consoleInput;
    [SerializeField] private TextMeshProUGUI outputText;

    [SerializeField] private GameObject users;
    private GameObject[] usersList;

    [Header("Hack Modules")]
    [SerializeField] private Phishing phishingModule;

    private string breakline = "\n";

    // Start is called before the first frame update
    void Start()
    {
        consoleInput.text = "";
        outputText.text = "Enter a command, start, load, options, exit..." + breakline;
        FindUsers();

        consoleInput.onSubmit.AddListener(ProcessCommand);

        consoleInput.ActivateInputField();
    }

    private void FindUsers()
    {
        usersList = GameObject.FindGameObjectsWithTag("User");
    }
    

    private void ProcessCommand(string text)
    {
        text = text.ToLower();

        switch(text)
        {
            case "scan users":
                if(usersList.Count() > 0)
                {
                    foreach(GameObject user in usersList)
                    {
                        outputText.text += breakline + "Found -> " + user.name;
                    }
                }
                break;
            case "create phishing mail":
                outputText.text += breakline + "Phishing mail created!!";
                break;
            default:
                outputText.text += breakline + "Invalid command entered!!";
                break;
        }



        consoleInput.text = "";
        consoleInput.ActivateInputField();

    }













}
