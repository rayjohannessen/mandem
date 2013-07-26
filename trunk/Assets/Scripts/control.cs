using UnityEngine;
using System.Collections;

public class control : MonoBehaviour {
	public Vector3 target; // position vector of our destination
	public float speed; // the speed we are moving
	public string state; // SEEKING, IDLE
	public GameObject p_cam;
	
	CameraLook p_camScript;
	
	// Use this for initialization
	void Start () {
		state = "idle";
		p_cam = GameObject.Find ("Main Camera");
		p_camScript = p_cam.GetComponent<CameraLook>();
	}
	
	public void Teleport (Vector3 tov, Vector3 toq)
	{
		rigidbody.velocity = Vector3.zero; // reverse force
		transform.position = tov;
		transform.rotation = Quaternion.LookRotation(toq);
		rigidbody.velocity = transform.forward * 2.0f; // apply force
		state = "idle";
	}
	
	void OnCollisionEnter(Collision other)	
	{
		if(other.gameObject.name == "door")
		{
			//state = "idle";
		}
		
		if(other.gameObject.name == "wall")
		{
			state = "idle";	
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		//data we need
		Vector3 up = new Vector3(0,1,0);
		Plane plane = new Plane(up, 0); 
	    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit oinfo;
		
		//check for input
		if(Input.GetMouseButton(1))
		{
			//if (state == "idle") // only set a new destination if we found the old one
				{
			    if(Physics.Raycast(ray, out oinfo))
				{
	
					
					if (oinfo.collider.name == "door") // if we click on a door, make sure to head to the position of the door.
					{
						target = oinfo.collider.gameObject.transform.position;
						target.y = transform.position.y;
					}
					else
					{
			        	target = ray.GetPoint(oinfo.distance); // set our destination to wherever on the ground the mouse has been clicked
						target.y = transform.position.y;					
					}
					
					speed = 750.0f;
					transform.LookAt(target);
					state = "seeking";
			    }
			}
		
		}
		
		//accelerate toward destination while five feet away

			float distanceFromTarget = Vector3.Distance(transform.position, target);
		
			if (state == "seeking")
			{
				rigidbody.AddForce(transform.forward * speed * Time.deltaTime);
				//transform.position += transform.forward * speed * Time.deltaTime * 0.01f;
			}
		
			if (distanceFromTarget < 0.5f)
			{
			//	speed *= 0.9f; // slow down once we're 2.5 units away
			}
		
			if (Vector3.Dot(transform.forward, (target - transform.position)) < -0.1f)
			{
				//transform.position = target;
				state = "idle";	// stop seeking a new position once we break epsilon of .01
			}
		
	}
}
