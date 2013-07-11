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
	
	public void Teleport (Vector3 tov, Quaternion toq)
	{
		rigidbody.velocity = Vector3.zero; // reverse force
		transform.position = tov;
		transform.rotation = toq;
		rigidbody.velocity = transform.forward * 2.0f; // apply force
		state = "idle";
		p_camScript.tele();
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
	    float distance;
		
		//check for input
		if(Input.GetMouseButton(1))
		{
		
		    if(plane.Raycast(ray, out distance))
			{
		        target = ray.GetPoint(distance);
				target.y = transform.position.y;
				speed = 750.0f;
				transform.LookAt(target);
				state = "seeking";
		    }
		
		}
		
		//accelerate toward destination while five feet away

			float dist = Vector3.Distance(transform.position, target);
			if (state == "seeking")
			{
			rigidbody.AddForce(transform.forward * speed * Time.deltaTime);
			transform.LookAt(target);
			}
		
			if (dist <4.5f)
			{
			state = "idle";
			}
		
	}
}
