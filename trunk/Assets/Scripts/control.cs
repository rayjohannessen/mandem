using UnityEngine;
using System.Collections;

public class control : MonoBehaviour {
	public Vector3 target; // position vector of our destination
	public float speed; // the speed we are moving
	public string state; // SEEKING, IDLE
	public GameObject p_cam;
	
	CharacterController controller;
	
	CameraLook p_camScript;

    MatchManager matchMngr;
	
	// Use this for initialization
	void Start () {
		state = "idle";
		p_cam = GameObject.Find ("Main Camera");
		p_camScript = p_cam.GetComponent<CameraLook>();
        p_camScript.SetPlayer(gameObject);
		

        door[] doors = FindObjectsOfType(typeof(door)) as door[];
        foreach (door d in doors)
        {
            d.SetPlayer(gameObject);
        }
		
		controller = GetComponent<CharacterController>();
        matchMngr = GameObject.Find("MatchManager").GetComponent<MatchManager>();
	}
	
	public void Teleport (Vector3 tov, Vector3 toq)
	{
		controller.transform.position = tov;
		controller.transform.rotation = Quaternion.LookRotation(toq);
		
		//state = "idle";
		
		target = tov + (controller.transform.forward * 3.0f);// walk three feet
	}
	
	void OnControllerColliderHit(ControllerColliderHit other)
	{
		if(other.gameObject.name.Substring(0, 4) == "door")
		{
			//state = "idle";
			Transform DoorA = other.collider.gameObject.transform;
			Transform DoorB = other.collider.gameObject.GetComponent<door>().partner.transform;
			
			if (Vector3.Dot(transform.forward, DoorA.forward) < 0.0f) 
			{
			    Vector3 vel = transform.forward;
			    vel = DoorA.InverseTransformDirection (vel);
			    vel = DoorB.TransformDirection (vel);
			    vel = -vel;
    					
			    Teleport(DoorB.position + (Vector3.up * -1), vel); // offset by center vertically
				
			    p_camScript.tele(); // update camera position to new player position
			}
		}
		
		if(other.gameObject.name == "wall")
		{
			state = "idle";	
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        if (!matchMngr.matchStarted)
            return;

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
					bool validtarget = false;
					
					if (oinfo.collider.name == "ground")
					{
			        	target = ray.GetPoint(oinfo.distance); // set our destination to wherever on the ground the mouse has been clicked
						target.y = controller.transform.position.y;	
						validtarget = true;
					}
					else if (oinfo.collider.gameObject.tag == "Player")
					{
							target = oinfo.collider.gameObject.transform.position;
							validtarget = true;
					}
					else if (oinfo.collider.name == "wall")
					{
						return;	
					}
					else if (oinfo.collider.name.Substring(0, 4) == "door") // only click on the door if its facing toward us.  Otherwise ignore it.
					{
						if (Vector3.Dot(oinfo.collider.gameObject.transform.forward, Camera.main.transform.forward) < 0.0f)
						{
							validtarget = true;
							target = oinfo.collider.gameObject.transform.position;
							target.y = controller.transform.position.y;
						}
					}
					
					if (validtarget)
					{
						speed = 15.0f;
						controller.transform.LookAt(target);
						state = "seeking";
					}
			    }
			}
		}
		else if (Input.GetMouseButton (0))
		{
			float mouseturnspeed = 10.0f;
			controller.transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * mouseturnspeed);
			state = "idle";
		}

		float distanceFromTarget = Vector3.Distance(controller.transform.position, target);
	
		if (state == "seeking")
		{
			controller.Move(controller.transform.forward * speed * Time.deltaTime);
		}
	
		if (Vector3.Dot(transform.forward, (target - transform.position)) < -0.1f)
		{
			state = "idle";	// stop seeking a new position once we break epsilon of .01
		}
	}
}
