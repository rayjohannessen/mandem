using UnityEngine;
using System.Collections;

public class CameraLook : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	transform.LookAt((GameObject.Find ("Cube")).transform.position);
	}
}
