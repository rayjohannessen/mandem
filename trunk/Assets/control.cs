using UnityEngine;
using System.Collections;



public class control : MonoBehaviour {
	public Vector3 target; // position vector of our destination
	public float speed; // the speed we are moving
	
	// Use this for initialization
	void Start () {
	
	}
	
	void onCollisionEnter(Collision c)
	{
		//if(c.gameObject.tag == "door")
		{
			Debug.Break();	
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		//data we need
		GameObject prim = GameObject.Find("ground"); // reference to a ground plane primitive
		Plane plane = new Plane(prim.transform.up, 0); 
	    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	    float distance;
		
		//check for input
		if(Input.GetMouseButtonDown(1))
		{
		
		    if(plane.Raycast(ray, out distance))
			{
		        target = ray.GetPoint(distance);
				target.y = transform.position.y;
				speed = 20.0f;
				transform.LookAt(target);
		    }
		
		}
		
		//move toward destination
		if (speed > 0.01)
		{
			
			transform.position += transform.forward * speed * Time.deltaTime;
			
			// acceleration falloff
			if (Vector3.Distance(transform.position, target) < 0.5f)
			{
				speed *= 0.5f;
			}
		}
		else
		{
			// clamp the speed at <= 0.1f
			speed = 0.0f;	
		}
		
	}
}
