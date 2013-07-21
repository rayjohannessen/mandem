using UnityEngine;
using System.Collections;

public class door : MonoBehaviour {
	public door partner;
	
	public GameObject player;
	control c;
	CameraLook p_cam;
	// Use this for initialization
	

	
	void Start () {
		partner = null;
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
			//if (c.state == "seeking")
			if (Vector3.Dot(c.transform.forward, transform.forward) < 0) 
			{
			Vector3 destposition = new Vector3();
			Quaternion destrotation = new Quaternion();
			destposition = partner.transform.position;// + (partner.transform.forward * 0.5f);
			destrotation = partner.transform.rotation;
				
			p_cam.dif = transform.position - c.p_cam.transform.position; // base new camera orientation off of the relativity to the IN door
			p_cam.actualDist = Vector3.Dot(p_cam.dif, transform.forward); // calculate relative z distance
			p_cam.actualXDist = Vector3.Dot(p_cam.dif, transform.right); // calculate relative x distance
				
			c.Teleport(destposition, destrotation);	
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
