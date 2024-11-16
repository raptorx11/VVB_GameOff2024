using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoreHack : MonoBehaviour
{
    private bool isGameStarted = false;
    private string currentPlayerName;

    [Header("Console Fields")]
    [SerializeField] private TMP_Text outputText;
    [SerializeField] private TMP_InputField commandInput;
    [SerializeField] private TMP_Text outputPlaceholder;

    private NetworkManager networkManager;
    private ScanModule scanModule;
    private ClearModule clearModule;

    void Start()
    {
        networkManager = FindObjectOfType<NetworkManager>();
        scanModule = GetComponent<ScanModule>();
        clearModule = GetComponent<ClearModule>();
        commandInput.enabled = true;

        commandInput.onSubmit.AddListener(ProcessCommand);
        commandInput.ActivateInputField();
    }

    private void ProcessCommand(string command)
    {
        if (!isGameStarted)
        {
            outputText.text += "\n";
            switch (command)
            {
                case "exit":
                    outputText.text += "System closing!\n";
                    break;

                case "help":
                    outputText.text += "type player <player_name> to select a player...\n";
                    outputText.text += "type start <player_name> to start game...\n";
                    outputText.text += "type list players to see all players with their last saved time...\n";
                    outputText.text += "type delete player <player_name> to delete a player...\n";
                    outputText.text += "type set difficulty <easy|medium|hard> to choose game difficulty...\n";
                    outputText.text += "type exit to quit game...";
                    break;

                case "list players":
                    ListPlayers();
                    break;

                default:
                    if (command.StartsWith("player "))
                    {
                        currentPlayerName = command.Substring(7).Trim();
                        outputText.text += $"Loading player {currentPlayerName}...\n";
                        outputPlaceholder.text = $"Type start {currentPlayerName} to start game...";
                        networkManager.LoadPlayer(currentPlayerName);
                        break;
                    }
                    else if (command.StartsWith("start "))
                    {
                        if (!string.IsNullOrEmpty(currentPlayerName))
                        {
                            outputText.text += $"Game started for player {currentPlayerName}...\n";
                            outputPlaceholder.text = "Try out different commands like scan, clear...";
                            isGameStarted = true;
                            break;
                        }
                        outputText.text += "No player selected. Use 'player <name>' to select a player.\n";
                    }
                    else if (command.StartsWith("delete player "))
                    {
                        string playerToDelete = command.Substring(14).Trim();
                        DeletePlayer(playerToDelete);
                    }
                    else if (command.StartsWith("set difficulty "))
                    {
                        string difficulty = command.Substring(14).Trim().ToLower();
                        SetDifficulty(difficulty);
                    }
                    else
                    {
                        outputText.text += "Invalid command! Type help to learn more!\n";
                    }
                    break;
            }
        }
        else
        {
            if (command.StartsWith("scan"))
            {
                scanModule.SetOutput(outputText);
                scanModule.ProcessCommand(scanModule.ParseArguments(command));
            }
            if (command.StartsWith("clear"))
            {
                clearModule.SetOutput(outputText);
                clearModule.ProcessCommand(scanModule.ParseArguments(command));
            }
        }

        commandInput.text = "";
        commandInput.ActivateInputField();
    }

    private void DeletePlayer(string playerName)
    {
        string playersFilePath = Application.persistentDataPath + "/players_data.json";
        List<NetworkManager.PlayerData> players = networkManager.LoadPlayerData(playersFilePath);

        var player = players.Find(p => p.PlayerName == playerName);
        if (player != null)
        {
            players.Remove(player);
            networkManager.SavePlayerData(playersFilePath, players);
            outputText.text += $"Player {playerName} has been deleted.\n";
        }
        else
        {
            outputText.text += $"Player {playerName} not found.\n";
        }
    }

    private void SetDifficulty(string difficulty)
    {
        if (difficulty != "easy" && difficulty != "medium" && difficulty != "hard")
        {
            outputText.text += "Invalid difficulty. Choose from easy, medium, or hard.\n";
            return;
        }

        string playersFilePath = Application.persistentDataPath + "/players_data.json";
        List<NetworkManager.PlayerData> players = networkManager.LoadPlayerData(playersFilePath);

        var player = players.Find(p => p.PlayerName == currentPlayerName);
        if (player != null)
        {
            player.Difficulty = difficulty;
            networkManager.SavePlayerData(playersFilePath, players);
            outputText.text += $"Difficulty set to {difficulty} for player {currentPlayerName}.\n";
        }
        else
        {
            outputText.text += "No player selected. Use 'player <name>' to select a player.\n";
        }
    }


    private void ListPlayers()
    {
        string playersFilePath = Application.persistentDataPath + "/players_data.json";
        List<NetworkManager.PlayerData> players = networkManager.LoadPlayerData(playersFilePath);

        if (players.Count == 0)
        {
            outputText.text += "No players found.\n";
        }
        else
        {
            outputText.text += "Players List:\n";
            foreach (var player in players)
            {
                outputText.text += $"{player.PlayerName} - Last Saved: {player.LastSavedTime}\n";
            }
        }
    }
}
