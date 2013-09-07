using uLink;
using UnityEngine;

public class Login : uLink.MonoBehaviour
{
    public string serverIP = "127.0.0.1";
    public int serverPort = 7100;

    private uLink.PublicKey publicKey = new uLink.PublicKey(@"564X/m0pdFnEfp23hfbImyAz9th7hf/DCfpOLAMkN48qVLd2w/+dshbhSrVQOELzUWOWaNFdFNkNsac9yTAumEJL74LrjtYQ4h9fehkHBNKcwLelfKJApy/TBA/1UNluRZLs49tk1NKxEjz5esHMgBfBdsNHR55iRS9q90lYVwE=", @"AQAB");

    public string user = "username";
    public string pw = "pw";

    // GUI
    public float panelWidth = 300;
    public float panelHeight = 200;
    Rect panelRect;

    MatchManager matchMngr;

    void Start()
    {
        float panelPosX = Screen.width / 2 - panelWidth / 2;
        float panelPosY = Screen.height / 2 - panelHeight / 2;
        panelRect = new Rect(panelPosX, panelPosY, panelWidth, panelHeight);
        matchMngr = GameObject.Find("MatchManager").GetComponent<MatchManager>();
    }

    // Connection
    void uLink_OnConnectedToServer()
    {
        Debug.Log("Now connected to server with username: " + user);
        Debug.Log("Local Port = " + uLink.Network.listenPort);
    }
    void uLink_OnDisconnectedFromServer(uLink.NetworkDisconnection _mode)
    {
        Debug.Log("Disconnected from server: " + _mode);

        GameObject.Find("PlayerManager").GetComponent<PlayerManager>().Clear();
        GameObject.Find("MatchManager").GetComponent<MatchManager>().OnLogout();
    }
    void uLink_OnFailedToConnect(uLink.NetworkConnectionError error)
    {
        Debug.Log("uLink_OnFailedToConnect() - " + error);
    }

    [RPC]
    void RemovePlayer(int _id)
    {
        GameObject.Find("PlayerManager").GetComponent<PlayerManager>().RemovePlayer(_id);
    }

    void OnGUI()
    {
        if (uLink.Network.status != uLink.NetworkStatus.Connected)
        {
            GUILayout.BeginArea(panelRect);
            {
                GUILayout.Box("User Login", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                GUILayout.BeginVertical();
                {
                    GUILayout.BeginArea(new Rect(20, 25, panelWidth - 40, panelHeight - 30), GUI.skin.customStyles[0]);
                    {
                        // Lets show login box!
                        GUILayout.Label("Username: ");
                        user = GUILayout.TextField(user, 20, GUILayout.MinWidth(150));

                        GUILayout.Label("Password: ");
                        pw = GUILayout.PasswordField(pw, '*', 20, GUILayout.MinWidth(150));

                        GUILayout.FlexibleSpace();

                        // Center login button
                        GUILayout.BeginHorizontal();

                        if (GUILayout.Button("Login") || (Event.current.type == EventType.keyDown && Event.current.character == '\n'))
                        {
                            if (user.Length > 0 && pw.Length > 0)
                            {
                                uLink.Network.publicKey = publicKey;
                                uLink.Network.Connect(serverIP, serverPort, "", user, pw);
                                Debug.Log("Sent encrypted connection request to server.");
                            }
                        }

                        GUILayout.FlexibleSpace();
                        GUILayout.EndHorizontal();
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndArea();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndArea();
        }
        else if (uLink.Network.status == uLink.NetworkStatus.Connected)
        {
            GUILayout.BeginArea(new Rect(10, 10, 60, 40));
            {
                if (GUILayout.Button("Logout"))
                {
                    uLink.Network.Disconnect();
                    Debug.Log("Disconnecting...");
                }
            }
            GUILayout.EndArea();

            if (!matchMngr.matchStarted && !matchMngr.waitingForPlayers && !matchMngr.matchStarting)
            {
	            GUILayout.BeginArea(new Rect(10, 45, 75, 40));
	            {
	                //
	                // TODO::we'll go to some sort of lobby where matchmaking occurs in the real game..
	                //  for now we just try to join to a match by pressing this button
	                //
	                if (GUILayout.Button("Join Match"))
	                {
	                    matchMngr.RequestJoinMatch();
	                }
	            }
	            GUILayout.EndArea();
            }
        }
    }
}
