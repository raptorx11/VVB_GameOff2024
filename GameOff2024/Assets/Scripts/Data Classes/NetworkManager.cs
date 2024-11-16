using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class NetworkManager : MonoBehaviour
{
    public List<NetworkData> Networks = new List<NetworkData>();
    private string currentPlayerName;

    [System.Serializable]
    public class NetworkData
    {
        public string NetworkAddress;
        public List<SubnetData> Subnets = new List<SubnetData>();
    }

    [System.Serializable]
    public class SubnetData
    {
        public string SubnetAddress;
        public List<SystemData> Systems = new List<SystemData>();
    }

    [System.Serializable]
    public class SystemData
    {
        public string IPAddress;
        public string AccessLevel;
        public int Firewall;
        public string Antivirus;
        public bool Keylogger;
        public bool Hacked;
        public float DetectionRisk;
        public bool LogsEnabled;
        public bool LogsAudit;
        public bool BackdoorInstalled;
        public string MaintenanceSchedule;

        public SystemData(string ipAddress, string difficulty)
        {

            IPAddress = ipAddress;
            AccessLevel = GenerateAccessLevel();
            if(difficulty == "easy")
            {
                Firewall = Random.Range(1, 2);
                Antivirus = Random.value > 0.8f ? "paid" : "free";
                DetectionRisk = (float)System.Math.Round(Random.Range(0.1f, 0.4f), 2);
                LogsAudit = AccessLevel == "admin" || AccessLevel == "manager" ? Random.value > 0.5f : Random.value > 0.9f;
                LogsEnabled = LogsAudit || Random.value > 0.85f;
            }
            else if(difficulty == "medium")
            {
                Firewall = Random.Range(1, 4);
                Antivirus = Random.value > 0.6f ? "paid" : "free";
                DetectionRisk = (float)System.Math.Round(Random.Range(0.1f, 0.65f), 2);
                LogsAudit = AccessLevel == "admin" || AccessLevel == "manager" ? Random.value > 0.25f : Random.value > 0.75f;
                LogsEnabled = LogsAudit || Random.value > 0.6f;
            }
            else if(difficulty == "hard")
            {
                Firewall = Random.Range(1, 6);
                Antivirus = Random.value > 0.35f ? "paid" : "free";
                DetectionRisk = (float)System.Math.Round(Random.Range(0.1f, 0.9f), 2);
                LogsAudit = AccessLevel == "admin" || AccessLevel == "manager" ? Random.value > 0.1f : Random.value > 0.5f;
                LogsEnabled = LogsAudit || Random.value > 0.35f;
            }
            Keylogger = false;
            Hacked = false;
            BackdoorInstalled = false;
            MaintenanceSchedule = GenerateMaintenanceSchedule();
        }

        private string GenerateAccessLevel()
        {
            float chance = Random.value;
            if (chance < 0.9f) return "employee";
            else if (chance < 0.95f) return "manager";
            else return "admin";
        }

        private string GenerateMaintenanceSchedule()
        {
            int hour = Random.Range(0, 24);
            int minute = Random.Range(0, 60);
            return $"{hour:D2}:{minute:D2}";
        }
    }

    [System.Serializable]
    public class PlayerData
    {
        public string PlayerName;
        public string LastSavedTime;
        public string Difficulty = "easy"; 
    }


    public void LoadPlayer(string playerName)
    {
        currentPlayerName = playerName;

        string filePath = Application.persistentDataPath + $"/network_data_{playerName}.json";

        if (File.Exists(filePath))
        {
            LoadNetworksFromJson(filePath);
        }
        else
        {
            GenerateNetworks();
            SaveNetworksToJson(filePath);
        }

        UpdatePlayerLastSavedTime(playerName);
    }

    public void GenerateNetworks()
    {
        Networks.Clear();

        int networkCount = 0, subnetCount = 0, systemCountMin = 0, systemCountMax = 0;

        switch (GetPlayerDifficulty())
        {
            case "easy":
                networkCount = 1;
                subnetCount = 2;
                systemCountMin = systemCountMax = 3;
                break;

            case "medium":
                networkCount = 2;
                subnetCount = 3;
                systemCountMin = systemCountMax = 5;
                break;

            case "hard":
                networkCount = 3;
                subnetCount = 5;
                systemCountMin = 8;
                systemCountMax = 10;
                break;

            default:
                Debug.LogError("Unknown difficulty level.");
                return;
        }
        // A list to track all generated systems
        List<SystemData> allSystems = new List<SystemData>();

        for (int i = 0; i < networkCount; i++)
        {
            string baseAddress = GenerateBaseAddress();
            NetworkData network = new NetworkData
            {
                NetworkAddress = baseAddress
            };

            for (int j = 1; j <= subnetCount; j++)
            {
                string subnetAddress = $"{baseAddress.Split('.')[0]}.{baseAddress.Split('.')[1]}.{j}.0";
                SubnetData subnet = new SubnetData { SubnetAddress = subnetAddress };

                int systemCount = Random.Range(systemCountMin, systemCountMax + 1);
                for (int k = 0; k < systemCount; k++)
                {
                    string systemIP = $"{subnetAddress.Split('.')[0]}.{subnetAddress.Split('.')[1]}.{subnetAddress.Split('.')[2]}.{k + 1}";
                    var system = new SystemData(systemIP, GetPlayerDifficulty());

                    subnet.Systems.Add(system);
                    allSystems.Add(system); // Add each system to the master list
                }

                network.Subnets.Add(subnet);
            }

            Networks.Add(network);
        }
        // Ensure at least one admin and one manager exist
        EnsureRolesExist(allSystems);
    }

    private void EnsureRolesExist(List<SystemData> systems)
    {
        bool hasAdmin = systems.Any(system => system.AccessLevel == "admin");
        bool hasManager = systems.Any(system => system.AccessLevel == "manager");

        if (!hasAdmin)
        {
            // Randomly assign one system to be an admin
            SystemData randomSystem = systems[Random.Range(0, systems.Count)];
            randomSystem.AccessLevel = "admin";
        }

        if (!hasManager)
        {
            // Randomly assign one system to be a manager
            SystemData randomSystem = systems[Random.Range(0, systems.Count)];
            // Ensure it's not the same as the admin
            while (randomSystem.AccessLevel == "admin")
            {
                randomSystem = systems[Random.Range(0, systems.Count)];
            }
            randomSystem.AccessLevel = "manager";
        }
    }
    private string GetPlayerDifficulty()
    {
        string playersFilePath = Application.persistentDataPath + "/players_data.json";
        List<PlayerData> players = LoadPlayerData(playersFilePath);

        var player = players.Find(p => p.PlayerName == currentPlayerName);
        return player?.Difficulty ?? "easy"; 
    }


    private string GenerateBaseAddress()
    {
        int ip = Random.Range(0, 256);
        return $"255.{ip}.0.0";
    }

    public void SaveNetworksToJson(string filePath)
    {
        string jsonData = JsonUtility.ToJson(new Wrapper<List<NetworkData>> { Data = Networks }, true);
        File.WriteAllText(filePath, jsonData);
        Debug.Log($"Network data saved to: {filePath}");
    }

    public void LoadNetworksFromJson(string filePath)
    {
        string jsonData = File.ReadAllText(filePath);
        Networks = JsonUtility.FromJson<Wrapper<List<NetworkData>>>(jsonData).Data;
        Debug.Log("Network data loaded.");
    }

    public void UpdatePlayerLastSavedTime(string playerName)
    {
        string playersFilePath = Application.persistentDataPath + "/players_data.json";
        List<PlayerData> players = LoadPlayerData(playersFilePath);

        var player = players.Find(p => p.PlayerName == playerName);
        if (player != null)
        {
            player.LastSavedTime = System.DateTime.Now.ToString();
        }
        else
        {
            players.Add(new PlayerData { PlayerName = playerName, LastSavedTime = System.DateTime.Now.ToString() });
        }

        SavePlayerData(playersFilePath, players);
    }

    public List<PlayerData> LoadPlayerData(string filePath)
    {
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            return JsonUtility.FromJson<Wrapper<List<PlayerData>>>(jsonData).Data;
        }
        return new List<PlayerData>();
    }

    public void SavePlayerData(string filePath, List<PlayerData> players)
    {
        string jsonData = JsonUtility.ToJson(new Wrapper<List<PlayerData>> { Data = players }, true);
        File.WriteAllText(filePath, jsonData);
        Debug.Log("Player data saved.");
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T Data;
    }
}
