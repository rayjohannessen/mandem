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
    Dictionary<uLink.NetworkPlayer, Player_Server> players;

    bool killerAssigned = false;

    door[] doors;
	
	public float match_length = 93.0f;
	public float seconds_to_midnight = 23.0f;
	float matchTimer = 0.0f;
	bool midnight = false;
	bool matchStarted = false;

	void Start () 
    {
        players = new Dictionary<uLink.NetworkPlayer, Player_Server>();

        // TEMP:: generate connections at the start of every new match
        GenerateDoorConnections();
	}

    void Update()
    {
		if (matchStarted)
		{
			matchTimer += Time.deltaTime;
			if (!midnight)
			{
				if (matchTimer >= seconds_to_midnight)
				{
					midnight = true;
	            	Debug.Log("IT IS MIDNIGHT");
					
					// Tell all the players ALERTTTTTT
	        		foreach (KeyValuePair<uLink.NetworkPlayer, Player_Server> player in players)
	        		{
	        		    networkView.RPC("AlertMurderer", player.Key, player.Value.isKiller);
						
						if (player.Value.isKiller)
							Debug.Log ("Player" + player.Key + "is the muderer");
						else
							Debug.Log ("Player" + player.Key + "is human.");
	        		}
					
				}
			}
		}
		
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

            bool isKiller = false;

            // if this is the last player to join and we still don't have a killer, he's it
            if (!killerAssigned && players.Count == numReqPlayers-1)
            {
                isKiller = true;
                killerAssigned = isKiller;
            }
            else if (!killerAssigned)
            {
                // TODO:: how to pick killer?? completely random?
                isKiller = UnityEngine.Random.Range(0, 100) > 50;
	            killerAssigned = isKiller;
            }

            players.Add(_info.sender, new Player_Server(isKiller));
        
            // setup the stream to send all the door connections back through to the client
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

    /// <summary>
    /// The client calls this to notify the server that he's ready to start
    /// He has all the door connection info (and anything else necessary to start a match).
    /// 
    /// If the server sees that everyone is ready it starts the match, otherwise it does nothing.
    /// </summary>
    /// <param name="_info"></param>
    [RPC]
    void NotifyReady(uLink.NetworkMessageInfo _info)
    {
        if (players.ContainsKey(_info.sender))
        {
            Debug.Log("NotifyReady() - from player with id " + _info.sender.id + "...waiting for " + (numReqPlayers-players.Count).ToString() + " more player(s) to join");

            players[_info.sender].state = Player_Server.ePlayerstate.PS_READY_AND_WAITING;

            // TODO::in the end we'd need a timeout value if some players take too long
            //      to send this notification
            if (numReqPlayers == players.Count)
            {
	            foreach (KeyValuePair<uLink.NetworkPlayer, Player_Server> p in players)
	            {
	                if (p.Value.state != Player_Server.ePlayerstate.PS_READY_AND_WAITING)
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

            if (players.Count == 0)
            {
                killerAssigned = false;
            }
        }
    }

    void _StartMatch()
    {
        Debug.Log("Starting match...sending StartMatch to " + players.Count + " players in match.");

        // Tell all the players to start the match
        foreach (KeyValuePair<uLink.NetworkPlayer, Player_Server> player in players)
        {
            networkView.RPC("StartMatch", player.Key);
            player.Value.state = Player_Server.ePlayerstate.PS_IN_MATCH;
        }
		matchTimer = 0.0f;
		midnight = false;
		matchStarted = true;
    }
}
