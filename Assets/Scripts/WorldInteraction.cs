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
        Debug.Log("Collided RPC");
    }
}
