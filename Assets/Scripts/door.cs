using UnityEngine;
using System.Collections;

public class door : MonoBehaviour {
	public door partner;
	
	public GameObject player;
	control c;
	CameraLook p_cam;
	Camera relativeCam;
	RenderTexture portalTexture;
	// Use this for initialization
	

	
	void Start () {
		
		//partner = null;
		if (!partner)
		{
			door[] doors = FindObjectsOfType(typeof(door)) as door[];
			
			while (!partner)
			{
				foreach (door i in doors)
				{
					if ( i != this)
					{
						// maybe select a partner
						if(Random.value > 0.5f)
						{
							i.partner = this;
							partner = i;
							break;
						}
					}
				}
			}
		}
		
		player = GameObject.Find("Player");
		p_cam = GameObject.Find ("Main Camera").GetComponent<CameraLook>();
		c = player.GetComponent<control>();
		
	}
	
	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.name == "Player")
		{
			if (c.state == "seeking")
			{
			if (Vector3.Dot(player.transform.forward, transform.forward) < 0.0f) 
			{
				Vector3 vel; // what will we do with the players orientation?
					
					vel = player.transform.forward;
					vel = transform.InverseTransformDirection (vel);
					vel = partner.transform.TransformDirection (vel);
					vel = -vel;
				
					
			c.Teleport(partner.transform.position, vel);
			p_cam.tele(); // update camera position to new player position
			}
			}
		}
	}
	
	
	
	// Update is called once per frame
	void Update () {
		
	}
}
