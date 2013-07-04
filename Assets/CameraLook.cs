using UnityEngine;
using System.Collections;

public class CameraLook : MonoBehaviour {
	
	public GameObject playerHandle;
	public float cameraFollowDistance;
	public float cameraFollowHeight;
	
	// Use this for initialization
	void Start () {
		playerHandle = (GameObject.Find ("Player"));
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 destination = new Vector3(playerHandle.transform.position.x, playerHandle.transform.position.y, playerHandle.transform.position.z);
		destination += playerHandle.transform.forward * -1.0f * cameraFollowDistance;
		destination.y += cameraFollowHeight;
		
	//transform.translate
	transform.position += (destination - transform.position) * 0.5f * Time.deltaTime;
	transform.LookAt(playerHandle.transform.position);

	}
}
