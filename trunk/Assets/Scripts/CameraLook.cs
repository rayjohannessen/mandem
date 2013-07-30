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
	public float lerpSpeed = 1.75f;

	float rotation; // rotation around the player
	
	
	// Use this for initialization
	void Start () {
// 		playerHandle = (GameObject.Find("Player_Owner"));
// 		destination = new Vector3(playerHandle.transform.position.x, playerHandle.transform.position.y, playerHandle.transform.position.z);
		dif = new Vector3();
		rotation = 0.0f;
	}
	
	public void tele()
	{
		/*
		Vector3 dif;
		dif = _from.InverseTransformPoint(transform.position);
		dif = Vector3.Reflect(dif, _from.transform.forward);
		dif = _to.transform.TransformPoint(dif);
		destination = dif;
		*/
		
		destination = playerHandle.transform.TransformPoint(dif);
		
		
		//destination = playerHandle.transform.position;
		//destination += playerHandle.transform.forward * -1.0f * cameraFollowDistance;
		//destination.y += cameraFollowHeight;
		
		transform.position = destination;
		transform.LookAt(playerHandle.transform.position + Vector3.up * 4.0f); // look 2 feet above the players feet
		
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (playerHandle)
        {
            dif = playerHandle.transform.InverseTransformPoint(transform.position);

            // calculate where we want to locate ourselves
            destination = playerHandle.transform.position;
            destination += playerHandle.transform.forward * -1.0f * cameraFollowDistance;
            destination.y += cameraFollowHeight;

            if (!Input.GetMouseButton(1) && Input.GetMouseButton(0))
            {
                lerpSpeed = 10.0f;
                cameraFollowHeight += Input.GetAxis("Mouse Y");
            }
            else
            {
                lerpSpeed = 1.75f;
            }

            //Interpolate position and look at player
            transform.position += (destination - transform.position) * lerpSpeed * Time.deltaTime;
            transform.LookAt(playerHandle.transform.position + Vector3.up * 4.0f); // look 2 feet above the players feet
        }
	}

    public void SetPlayer(GameObject _player)
    {
        playerHandle = _player;
    }
}
