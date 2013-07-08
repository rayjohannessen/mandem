using UnityEngine;
using System.Collections;

public class door : MonoBehaviour {
	public door partner;
	
	public GameObject player;
	control c;
	// Use this for initialization
	

	
	void Start () {
		/*
		if (!partner)
		{
			door[] doors = GameObject.FindGameObjectsWithTag("door");
			
			foreach (door i in doors)
			{
				if (!i.partner)
				{
					i.partner = this;
					partner = i;
					break;
				}
			}
		}
		*/
		player = GameObject.Find("Player");
		c = player.GetComponent<control>();
	}
	
	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.name == "Player")
		{
			if (c.state == "seeking")
			{
			Vector3 destposition = new Vector3();
			Quaternion destrotation = new Quaternion();
			destposition = partner.transform.position + (partner.transform.forward * 0.5f);
			destrotation = partner.transform.rotation;
			c.Teleport(destposition, destrotation);	
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
