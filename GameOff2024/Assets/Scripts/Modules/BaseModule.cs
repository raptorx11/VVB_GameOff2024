using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class BaseModule : MonoBehaviour
{
    protected TMP_Text output;
    protected NetworkManager networkManager;

    public void Start()
    {
        networkManager = FindObjectOfType<NetworkManager>();
    }

    public abstract void ProcessCommand(string[] args);

    public virtual string[] ParseArguments(string command) => command.Split(' ');

    public virtual void SendFeedback(string message) => output.text += "\n" + message; // + "\n";

    public virtual void SetOutput(TMP_Text output) => this.output = output;

}

