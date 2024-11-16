using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearModule : BaseModule
{
    public override void ProcessCommand(string[] args)
    {
        if(args.Length > 3)
        {
            SendFeedback($"Invalid {args[0]} command! Try '{args[0]} help' to get list of available commands!!");
            return;
        }
        else
        {
            string argument = args[1];
            switch(argument)
            {
                case "console":
                    output.text = "";
                    break;

                case "logs":
                    SendFeedback("Logs cleared!");
                    break;

                case "help":
                    SendFeedback($"Available commands for {args[0]}:\n" +
                        "-----------------------------\n" +
                        "clear console   | Clears all console.\n" +
                        "clear logs      | Clears logs.\n" +
                        "-----------------------------");
                    break;

                default:
                    SendFeedback($"Invalid {args[0]} command! Try '{args[0]} help' to get list of available commands!!");
                    break;
            }
        }
    }



}
