using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// The door connections could be sent during a period before the match starts I'd say. 
/// We'd have a "waiting for players to be ready" message while that stuff is being sent/received...
/// when the server gets a received message from all clients it sends out the "start match" signal and wala.
/// </summary>
public class MatchManager_Server : uLink.MonoBehaviour 
{
    /// <summary>
    /// 
    /// TEMP::this is how many players need to join before the server will start the match
    /// 
    /// </summary>
    public int numReqPlayers = 2;

    // players matched with bool, true if they're ready to go
    // Ready means:
    // - the player has notified this manager that it has received the door connections
    // - 
    Dictionary<uLink.NetworkPlayer, bool> players;

    door[] doors;

	void Start () 
    {
        players = new Dictionary<uLink.NetworkPlayer, bool>();

        // TEMP:: generate connections at the start of every new match
        GenerateDoorConnections();
	}
	
    /// <summary>
    /// This function can handle any verification that might need to be done
    /// before the player can be allowed to join
    /// </summary>
    [RPC]
    void RequestToJoinMatch(uLink.NetworkMessageInfo _info)
    {
        if (!players.ContainsKey(_info.sender))
        {
            Debug.Log("RequestToJoinMatch() - from player with id " + _info.sender.id);

            players.Add(_info.sender, false);
        
            bool isTypeSafe = ((uLink.Network.defaultRPCFlags & uLink.NetworkFlags.TypeUnsafe) == 0);
            uLink.BitStream stream = new uLink.BitStream(isTypeSafe);

            stream.WriteInt16((short)doors.Length);
            foreach (door d in doors)
            {
                stream.WriteString(d.name);
                stream.WriteString(d.partner.name);
            }

            networkView.RPC("JoinResponse", _info.sender, true, stream);
        }
    }

    [RPC]
    void NotifyReady(uLink.NetworkMessageInfo _info)
    {
        if (players.ContainsKey(_info.sender))
        {
            Debug.Log("NotifyReady() - from player with id " + _info.sender.id + "...waiting for " + (numReqPlayers-players.Count).ToString() + " more player(s) to join");

            players[_info.sender] = true;

            // TODO::in the end we'd need a timeout value if some players take too long
            //      to send this notification
            if (numReqPlayers == players.Count)
            {
	            foreach (KeyValuePair<uLink.NetworkPlayer, bool> p in players)
	            {
	                if (!p.Value)
	                {
	                    // a player isn't yet ready, no need to go any further
	                    return;
	                }
	            }
	            _StartMatch();
            }
        }
    }

    public void GenerateDoorConnections()
    {
        doors = FindObjectsOfType(typeof(door)) as door[];
        foreach (door d in doors)
        {
            d.SetPartner(doors);
        }
        Debug.Log("GenerateDoorConnections() - generated connections for " + doors.Length + " doors.");
    }

    public void PlayerLoggedOut(uLink.NetworkPlayer _player)
    {
        if (players.ContainsKey(_player))
        {
            players.Remove(_player);
        }
    }

    void _StartMatch()
    {
        Debug.Log("Starting match...sending StartMatch to " + players.Count + " players in match.");

        // Tell all the players to start the match
        foreach (KeyValuePair<uLink.NetworkPlayer, bool> player in players)
        {
            networkView.RPC("StartMatch", player.Key);
        }
    }
}
