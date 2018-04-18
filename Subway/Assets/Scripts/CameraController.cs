using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public GameObject player;

	void Start(){
	}

	// Update is called once per frame
	void Update () {
		Vector3 moveZ = new Vector3(transform.position.x,transform.position.y,player.transform.position.z);
		transform.position = moveZ;
	}
}
