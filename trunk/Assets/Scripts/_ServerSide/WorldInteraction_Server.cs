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

    // NOTE: use this? that collider's radius is really small, so maybe we can use collision functions below??
    //      have to add a collider for that though...
	void OnControllerColliderHit(ControllerColliderHit other)
    {
        Debug.Log("OnControllerColliderHit()");
		Debug.Log (name + " collided with " + other.gameObject.name);
		if (other.gameObject.tag == "Player")
		{
	        PlayerData_Server pd = other.gameObject.GetComponent<WorldInteraction_Server>().playerData;
	        if (pd.isKiller)
	        {
	            Debug.Log(" - player " + name + " collided with " + other.gameObject.name);
				
	           // RPC something to client...
	            networkView.RPC("Collided", networkView.owner);
	        }
		}
    }
	
  /*  void OnCollisionEnter(Collision _collision)
    {
        Debug.Log("OnCollisionEnter()");

        PlayerData_Server pd = _collision.gameObject.GetComponent<WorldInteraction_Server>().playerData;
        if (pd.isKiller)
        {
            Debug.Log(" - player " + name + " collided with " + _collision.gameObject.name);

           // RPC something to client...
            networkView.RPC("Collided", networkView.owner);
			
        }
    }
    

    void OnCollisionStay(Collision _collision)
    {

    }

    void OnCollisionExit(Collision _collision)
    {

    }
    
    */
}
