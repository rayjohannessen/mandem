using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldInteraction_Server : uLink.MonoBehaviour 
{
    [HideInInspector]
    public PlayerData_Server playerData;


	void Start () 
    {
	
	}
	
	void Update () 
    {
	
	}

    public void HandleItemCollide(GameObject _item)
    {
        if (_item.tag == "Item")
        {
            Debug.Log(" - player " + name + " collided with item " + _item.name);

            // store the info in the player data and make sure the object is no longer visible
            playerData.ObtainedItem(_item.GetComponent<Item>(), false, false);
            _item.GetComponent<Item>().ownerID = networkView.owner.id;

            // RPC something to client...
            MatchManager_Server mm = GameObject.Find("MatchManager").GetComponent<MatchManager_Server>();
            foreach (KeyValuePair<uLink.NetworkPlayer, PlayerData_Server> players in mm.Players)
            {
                networkView.RPC("Collided", players.Key, WorldInteraction.eWorldInterType.WIT_PICKUP_ITEM, _item.GetComponent<Item>().id);
            }
        }
    }
}
