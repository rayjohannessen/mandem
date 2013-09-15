using UnityEngine;
using System.Collections;

public class WorldInteraction : uLink.MonoBehaviour 
{



	void Start () 
    {
	
	}
	
	void Update () 
    {
	
	}

    [RPC]
    void Collided()
    {
		PlayerData d = gameObject.GetComponent<PlayerData>();
        Debug.Log("Collided RPC");
		
		if (d.job == "Human")
		{
			gameObject.GetComponent<MatchManager>().ShowMessage("You have died.");
			d.job = "Ghost";
		}
    }
}
