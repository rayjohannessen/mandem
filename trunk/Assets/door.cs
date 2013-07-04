using UnityEngine;
using System.Collections;

public class door : MonoBehaviour {
	public door partner;
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
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
