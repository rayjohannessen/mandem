using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MatchManager : uLink.MonoBehaviour 
{
    [HideInInspector]
    public bool matchStarting = false;
    [HideInInspector]
    public bool matchStarted = false;
    [HideInInspector]
    public bool waitingForPlayers = false;

    public float coundownSeconds;
    float matchStartCounter;
	
	float messageCounter;

    public GUIText centerMsgGUI;
	
	PlayerData playerData;

	void Start () 
    {
        centerMsgGUI.enabled = false;	
		messageCounter = 0.0f;
	}
	
	public void ShowMessage(string msg)
	{
		if (centerMsgGUI)
		{
		centerMsgGUI.text = msg;
		messageCounter = (float)(msg.Length);
		centerMsgGUI.enabled = true;
		}
	}
	
	void Update () 
    {
	    if (matchStarting)
	    {
            matchStartCounter -= Time.deltaTime;
            if (matchStartCounter < 0f)
            {
                matchStarted = true;
                matchStarting = false;
                if (centerMsgGUI)
                    centerMsgGUI.enabled = false;
                Debug.Log("Match Started!!");
                return;
            }
            if (centerMsgGUI)
                centerMsgGUI.text = ((int)matchStartCounter).ToString();
	    }
		else if (messageCounter > 0.0f)
		{
			messageCounter -= Time.deltaTime;
		}
		else
		{
			centerMsgGUI.enabled = false;	
		}
	}

    /// <summary>
    /// TODO::take in a match ID??
    /// Or will the non-demo version just have the server match everyone up and it
    ///     will keep track of match ID?
    /// </summary>
    public void RequestJoinMatch()
    {
        if (centerMsgGUI)
        {
            centerMsgGUI.enabled = true;
            centerMsgGUI.text = "Joining match...";
        }
        networkView.RPC("RequestToJoinMatch", uLink.RPCMode.Server);
        Debug.Log("Request to join match sent to server.");
    }

    /// <summary>
    /// The current implementation of this func requires the door names to be consistent
    ///     across the server and client.
    /// </summary>
    /// <param name="_canJoin"></param>
    /// <param name="_stream"></param>
    [RPC]
    void JoinResponse(bool _canJoin, uLink.BitStream _stream)
    {
        if (_canJoin)
        {
            waitingForPlayers = true;
            if (centerMsgGUI)
            {
	            centerMsgGUI.enabled = true;
                centerMsgGUI.text = "Waiting for other players...";
            }

            door[] doors = FindObjectsOfType(typeof(door)) as door[];

            // putting them in a dictionary to make it easier
            Dictionary<string, door> doorsDict = new Dictionary<string, door>();
            foreach (door d in doors)
                doorsDict.Add(d.name, d);

            short numDoors = _stream.ReadInt16();
            for (short i = 0; i < numDoors; ++i)
            {
                string currDoor = _stream.ReadString();
                string partnerDoor = _stream.ReadString();
                if (doorsDict.ContainsKey(currDoor) && doorsDict.ContainsKey(partnerDoor))
                    doorsDict[currDoor].partner = doorsDict[partnerDoor];
            }

            ItemFactory itemFactory = GameObject.Find("ItemFactory").GetComponent<ItemFactory>();

            short numItems = _stream.ReadInt16();
            Vector3 pos;
            int subType, id;
            Item.eItemType itemType;
            for (short i = 0; i < numItems; ++i)
            {
                itemType = (Item.eItemType)_stream.ReadInt16();
                subType = _stream.ReadInt16();
                id = _stream.ReadInt32();
                pos = _stream.ReadVector3();

                itemFactory.GenerateItem(itemType, pos, Quaternion.identity, subType, id);
            }

            Debug.Log("JoinResponse() - Received partner info for " + numDoors + " doors and " + numItems + " items. Server said go ahead and start the match. Sending notification we're ready.");

            // we're good to go, let the server know
            networkView.RPC("NotifyReady", uLink.RPCMode.Server);
        }
        else
        {
            Debug.Log("JoinResponse() - server won't allow us to join match.");
        }
    }

    [RPC]
    void StartMatch()
    {
		playerData = GameObject.Find("Player_Owner(Clone)").GetComponent<PlayerData>();
        waitingForPlayers = false;
        matchStarting = true;
        matchStartCounter = coundownSeconds;
        if (centerMsgGUI)
        {
            centerMsgGUI.enabled = true;
            centerMsgGUI.text = ((int)matchStartCounter).ToString();
        }
        Debug.Log("Match Starting!!");
    }
	
	[RPC]
	void AlertMurderer(bool isKiller)
	{
		if (isKiller)
		{
			playerData.job = "Killer";
			ShowMessage ("Mundus Vult Decipiatur Ergo Interficere");
		}
		else
		{
			playerData.job = "Human";
			ShowMessage ("Quaerendo Invenietis");
		}
			Debug.Log("Player " + playerData.playerID + " is a " + playerData.job);
	}

    public void OnLogout()
    {
        if (centerMsgGUI)
        {
            centerMsgGUI.text = "";
            centerMsgGUI.enabled = false;
        }
        waitingForPlayers = false;
        matchStarted = false;
        matchStarting = false;
        matchStartCounter = 0f;
    }
}
