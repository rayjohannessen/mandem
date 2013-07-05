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
			c.Teleport(partner.transform.position);	
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
