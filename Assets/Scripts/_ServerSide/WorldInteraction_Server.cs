using UnityEngine;
using System.Collections;

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
            //Debug.Log(" - player " + name + " collided with " + _item.name);

            // RPC something to client...
            networkView.RPC("Collided", networkView.owner);
        }
    }
}
