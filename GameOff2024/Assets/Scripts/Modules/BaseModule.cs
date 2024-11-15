using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class BaseModule : MonoBehaviour
{
    private TMP_Text output;

    public abstract void ProcessCommand(string[] args);

    public virtual bool ValidateArgs(string[] args) => args != null && args.Length > 0;

    public virtual string[] ParseArguments(string command) => command.Split(' ');

    public virtual void SendFeedback(string message) => output.text += message;

    public virtual void SetOutput(TMP_Text output) => this.output = output;



}
