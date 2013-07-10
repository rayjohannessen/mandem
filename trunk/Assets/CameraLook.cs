using UnityEngine;
using System.Collections;

public class CameraLook : MonoBehaviour {
	
	public GameObject playerHandle;
	public float cameraFollowDistance;
	public float cameraFollowHeight;
	public Vector3 destination;
	public Vector3 dif;
	public float actualDist;
	public float actualXDist;
	
	// Use this for initialization
	void Start () {
		playerHandle = (GameObject.Find ("Player"));
		destination = new Vector3(playerHandle.transform.position.x, playerHandle.transform.position.y, playerHandle.transform.position.z);
		dif = new Vector3();
	}
	
	public void tele()
	{
		// recalculate relative position to OUT door
		destination = playerHandle.transform.position;
		destination += playerHandle.transform.forward * (actualDist);
		destination += playerHandle.transform.right * (actualXDist);
		destination.y = cameraFollowHeight;
		
		transform.position = destination;	
	}
	
	// Update is called once per frame
	void Update () {
		
		// calculate where we want to locate ourselves
		destination = playerHandle.transform.position;
		destination += playerHandle.transform.forward * -1.0f * cameraFollowDistance;
		destination.y += cameraFollowHeight;
		
		//Interpolate position and look at player
		transform.position += (destination - transform.position) * 1.75f * Time.deltaTime;
		transform.LookAt(playerHandle.transform.position + Vector3.up * 2.0f); // look 2 feet above the players feet
		
		// calculate relative things.
		dif = playerHandle.transform.position - transform.position;
		actualDist = Vector3.Dot(dif, playerHandle.transform.forward);
		actualXDist = Vector3.Dot (dif, playerHandle.transform.right);
	}
}
