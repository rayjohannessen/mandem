using UnityEngine;
using System.Collections;

public class serverCam : MonoBehaviour {
	Camera p_Cam;
	Vector3 targetPos;
	Quaternion targetRot;
	GameObject currentPlayer;
	float timer;
	
	public float switch_interval = 5.0f; // duration in seconds we look at a player before switching
	public float followDistance = 30.5f; // distance behind the player we position ourselves
	public float cameraHeight = 2.5f; // height above the player we position ourselves
	public float cameraLead = 5.0f; // how far in front of the player do we look?
	
	// Use this for initialization
	void Start () {
		p_Cam = GameObject.Find("Main Camera").GetComponent<Camera>();
		targetPos = transform.position;
		targetRot = transform.rotation;
		currentPlayer = null;
	}
	
	void Add(GameObject player)
	{
	}
	
	// Update is called once per frame
	void Update () {
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		
		timer -= Time.deltaTime;
		
		if (timer <= 0.0f)
		{
			float d = (float)players.Length;
			
			foreach (GameObject i in players)
			{
				// roll for a new player, each one gets a one / total chance
				if (Random.value <= 1.0f / d)
				{
					if (i != currentPlayer)
					{
						currentPlayer = i;
						timer += switch_interval;
						break;
					}
				}
			}
			
		}
		
		if (currentPlayer)
		{
			targetPos = currentPlayer.transform.position;
			targetPos -= currentPlayer.transform.forward * followDistance;
			targetPos += currentPlayer.transform.up * cameraHeight;
		
			transform.position += (targetPos - transform.position) * Time.deltaTime;
			
			targetRot = Quaternion.LookRotation((currentPlayer.transform.position + (currentPlayer.transform.forward * cameraLead)) - transform.position);
			
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime);			
		}
			
	}
}
