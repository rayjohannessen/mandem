using uLink;
using UnityEngine;
using System.Collections.Generic;

public class Server : uLink.MonoBehaviour
{
    public int serverPort = 7100;

    // the private key should be kept secret and never be send to any client.
    private uLink.PrivateKey privateKey = new uLink.PrivateKey(@"564X/m0pdFnEfp23hfbImyAz9th7hf/DCfpOLAMkN48qVLd2w/+dshbhSrVQOELzUWOWaNFdFNkNsac9yTAumEJL74LrjtYQ4h9fehkHBNKcwLelfKJApy/TBA/1UNluRZLs49tk1NKxEjz5esHMgBfBdsNHR55iRS9q90lYVwE=", @"AQAB", @"+jbjMLPtZIf06D4C4N4OtNr3Tl2R4O81N5B+a/eOdHOHaeySmjoY+xfjKL3k1x2YsDzXr9U9GJtNwlzdVrLK3w==", @"7Ql+mMoIXp2SR42bhc8TkcmVRbYerHx+JLA4Ige17jNFhHM02NBkYc7tFlgQUH7RgsbeYX1PTmhdTZljFuz6Hw==", @"vVSG+LVNLkLKCGnT179vNV5yv3OCDMg0ZoUJhDzgKDG7B2WhUN4hRO5ATvXRkQyuGr0PH9ek0VfCsQ1/1jiX1Q==", @"v+F7tbt2YwEzNPERAJTMxqtkRvZShlaQ1qpABmwvfg/LKpkIIqsvV23mxrurGT5P44mQ42JJHLOnM/YDHL/hCQ==", @"jwAp/IizAlE1UeChAwRgn315QIzW7zpagQ7bSNdtoYAQ6vIGtGhEBgv+CpQsxZdwmqx8HZYtBWICb1qJN8XWww==", @"Cv9ppis6Z4qHWFdWSeawGSULMnGOU4sTkBqwsUgo5PZH1SOsYJt2ueh6I1i+CR2sfTWUAz/FAmNXUhKVTUKbQ4no335o5HZQc82hoDE0MRvh2V/C4QQwI67sZ/9+VZMh3uk4xZtyhmpSApXxwfrSw4dLNqV775MuS3SozrFZbp0=");

    private bool includeCurrentPlayers = false;

    /// <summary>
    /// responsible for Instantiating the player object:
    /// - creates one on the server (creator)
    /// - creates one for the player (owner)
    /// - creates a proxy for other players (proxy)
    /// </summary>
    GameObject AuthSpawnPrefab;

    void Awake()
    {
        Application.runInBackground = true;

        //By setting the public and private key for this server before starting it, connection 
        //attempts will only be accepted if they are encrypted with the correct public key.
        uLink.Network.privateKey = privateKey;

        // Turning on security before the server starts is a way to ensure that all connection 
        // requests from clients must be encypted or they will be ignored.
        uLink.Network.InitializeSecurity(includeCurrentPlayers);
        Debug.Log("Server security Initialized");

        uLink.Network.InitializeServer(32, serverPort);
    }

    void uLink_OnServerInitialized()
    {
        Debug.Log("Server successfully started");
    }

    // this happens first
    void uLink_OnPlayerApproval(uLink.NetworkPlayerApproval approval)
    {
//         string user = approval.loginData.ReadString();
//         string password = approval.loginData.ReadString();

        approval.status = NetworkPlayerApprovalStatus.Approved;
    }

    // the player has connected properly
    void uLink_OnPlayerConnected(uLink.NetworkPlayer player)
    {
        Debug.Log("Player " + player.id + " connected from " + player.ipAddress + ":" + player.port);

        //Right after the player has been connected, the security is turned off for this player.
        //We save server resources (mostly CPU) by turning off security now after having verified the username + password
        uLink.Network.UninitializeSecurity(player);

        // the client can now load the world
        //uLink.Network.RPC(networkView.viewID, "EnterWorld", player, "_WORLD_");
        GetComponent<SpawnPrefabs>().SpawnPlayer(player);
    }

    void uLink_OnPlayerDisconnected(uLink.NetworkPlayer player)
    {
        Debug.Log("Player " + player.id + " disconnected " + player.ipAddress + ":" + player.port);

        GameObject.Find("MatchManager").GetComponent<MatchManager_Server>().PlayerLoggedOut(player);

        networkView.RPC("RemovePlayer", uLink.RPCMode.AllExceptOwner, player.id);
		
        uLink.Network.DestroyPlayerObjects(player);
        uLink.Network.RemoveRPCs(player);
        uLink.Network.RemoveInstantiates(player);
    }

    void uLink_OnSecurityInitialized(uLink.NetworkPlayer player)
    {
        Debug.Log("Encryption turned on for client " + player.ipAddress + ":" + player.port);
    }

    void uLink_OnSecurityUninitialized(uLink.NetworkPlayer player)
    {
        Debug.Log("Encryption turned off for client " + player.ipAddress + ":" + player.port);
    }



    void OnGUI()
    {
    }
}