using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanModule : BaseModule
{
    public override void ProcessCommand(string[] args)
    {
        if (args.Length == 1)
        {
            SendFeedback($"Invalid {args[0]} command! Try '{args[0]} help' to get list of available commands!!");
            return;
        }

        string argument = args[1];
        switch (argument)
        {
            case "network":
                // Scan for networks and subnets
                ScanNetwork();
                break;

            case "subnet":
                // Scan for systems in a specific subnet
                if (args.Length > 2)
                    ScanSubnet(args[2]);
                else
                    SendFeedback("Please specify a subnet address after 'scan subnet'. Example: 'scan subnet 255.100.1.0'");
                break;

            case "ip":
                // Scan for detailed information of a specific system IP
                if (args.Length > 2)
                    ScanIP(args[2]);
                else
                    SendFeedback("Please specify an IP address after 'scan ip'. Example: 'scan ip 255.100.1.1'");
                break;

            case "help":
                SendFeedback($"Available commands for {args[0]}:\n" +
                    "-----------------------------\n" +
                    "scan network           | Gives list of networks and subnets.\n" +
                    "scan subnet <ipaddr>   | Gives list of systems connected to the specified subnet.\n" +
                    "scan ip <ipaddr>       | Provides detailed information on a specific IP address.\n" +
                    "-----------------------------"
                    );
                break;

            default:
                SendFeedback($"Invalid {args[0]} command! Try '{args[0]} help' to get list of available commands!!");
                break;
        }
    }

    // Scan for networks and subnets
    private void ScanNetwork()
    {
        if (networkManager.Networks.Count > 0)
        {
            SendFeedback("Available networks:");
            SendFeedback("--------------------------------------");
            foreach (var network in networkManager.Networks)
            {
                SendFeedback($"- {network.NetworkAddress}");

                foreach (var subnet in network.Subnets)
                {
                    SendFeedback($"  - Subnet: {subnet.SubnetAddress}");
                }
                SendFeedback("--------------------------------------");
            }
        }
        else
        {
            SendFeedback("No networks available.");
        }
    }

    // Scan a specific subnet for systems
    private void ScanSubnet(string subnetaddr)
    {
        bool subnetFound = false;

        SendFeedback("Scanning Subnet!!");
        foreach (var network in networkManager.Networks)
        {
            foreach (var subnet in network.Subnets)
            {
                if (subnet.SubnetAddress == subnetaddr)
                {
                    subnetFound = true;
                    SendFeedback("--------------------------------------");
                    SendFeedback($"Systems in subnet {subnetaddr}:");

                    foreach (var system in subnet.Systems)
                    {
                        SendFeedback($"  - System IP: {system.IPAddress}");
                    }
                    SendFeedback("--------------------------------------");
                }
            }
        }

        if (!subnetFound)
        {
            SendFeedback($"Subnet {subnetaddr} not found.");
        }
    }

    // Scan for detailed information on a specific system
    private void ScanIP(string ipaddr)
    {
        SendFeedback("Scanning IP Addresses connected!!");
        foreach (var network in networkManager.Networks)
        {
            foreach (var subnet in network.Subnets)
            {
                foreach (var system in subnet.Systems)
                {
                    if (system.IPAddress == ipaddr)
                    {
                        SendFeedback("--------------------------------------");
                        SendFeedback($"System Details for IP: {ipaddr}");
                        SendFeedback($"- Access Level: {system.AccessLevel}");
                        SendFeedback($"- Firewall Strength: {system.Firewall}");
                        SendFeedback($"- Antivirus: {system.Antivirus}");
                        SendFeedback($"- Keylogger: {(system.Keylogger ? "Yes" : "No")}");
                        SendFeedback($"- Hacked: {(system.Hacked ? "Yes" : "No")}");
                        SendFeedback($"- Detection Risk: {system.DetectionRisk}");
                        SendFeedback($"- Logs Enabled: {(system.LogsEnabled ? "Yes" : "No")}");
                        SendFeedback($"- Logs Audit: {(system.LogsAudit ? "Yes" : "No")}");
                        SendFeedback($"- Backdoor Installed: {(system.BackdoorInstalled ? "Yes" : "No")}");
                        SendFeedback($"- Maintenance Schedule: {system.MaintenanceSchedule}");
                        SendFeedback("--------------------------------------");
                        return;
                    }
                }
            }
        }

        SendFeedback($"System with IP {ipaddr} not found.");
    }
}
