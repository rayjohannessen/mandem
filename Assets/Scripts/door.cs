using UnityEngine;
using System.Collections;

/// <summary>
/// Note: door names have to be unique and match across client/server so they can be identified
///     when the connection data is sent to the client. Either that or they should just be
///     spawned dynamically...
/// </summary>
public class door : uLink.MonoBehaviour 
{
	public door partner;
	
	public GameObject player;
	control m_Control;
	CameraLook p_cam;
	Camera relativeCam;
	RenderTexture portalTexture;
	// Use this for initialization
	
	void Start () 
    {		
		//player = GameObject.Find("Player");
		p_cam = GameObject.Find("Main Camera").GetComponent<CameraLook>();
	}
	
	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.CompareTag("Player"))
		{
			if (m_Control != null && m_Control.state == "seeking")
			{
			    if (Vector3.Dot(player.transform.forward, transform.forward) < 0.0f) 
			    {
				    Vector3 vel = player.transform.forward;
				    vel = transform.InverseTransformDirection (vel);
				    vel = partner.transform.TransformDirection (vel);
				    vel = -vel;
    					
			        m_Control.Teleport(partner.transform.position, vel);
			        p_cam.tele(); // update camera position to new player position
			    }
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
    {
	}

    public void SetPlayer(GameObject _player)
    {
        player = _player;
        m_Control = player.GetComponent<control>();
    }
	
    /// <summary>
    /// Server-only
    /// </summary>
    /// <param name="doors"></param>
    public void SetPartner(door[] doors)
    {
        while (!partner)
        {
            foreach (door i in doors)
            {
                if (i != this && !i.partner)
                {
                    // maybe select a partner
                    if (Random.value > 0.5f)
                    {
                        i.partner = this;
                        partner = i;
                        break;
                    }
                }
            }
        }
    }

}
