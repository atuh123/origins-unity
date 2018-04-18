using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController4 : MonoBehaviour {

	Rigidbody rb;
	public Quaternion lockRotation;
	Vector3 camRotation;
	public float maxFov = 45f;
	public float minFov = 25f;
	public float maxDist = 15f;
	public float minDist = 5f;
	public static float offsetPlayer;
	public float speed = 20f;
	public float fovConv = 6f;
	public float xConv = 1f;
	public float xDiff = 15f;
	public float zOffset = 0.5f;
	public float camOffset = 5f;
	public float camXOffset = 15f;
	public float smoothTime = 0.3f;
	private Vector3 velocity = Vector3.one;
	public Camera cam;
	Vector3 camPosition;
	GameObject[] enemys;
	GameObject currentEnemy;
	Quaternion initialRotation;
	Vector3 initialPosition;
	public float fov = 45f;
	Vector3 enemyPos;
	Vector3 playerZPos;
	float enemyDistance;
	float enemyCross;
	float playerCross;

	void Start()
	{
		rb = GetComponent<Rigidbody> ();
		if (cam == null) {
			cam = Camera.main;
		}

		camPosition = new Vector3 (cam.transform.position.x, cam.transform.position.y, (transform.position.z - camOffset));

		cam.transform.position = camPosition;

		initialPosition = cam.transform.position;

		initialRotation = cam.transform.rotation;

	}
	// Update is called once per frame
	void Update () {

		//player movement
		Vector3 right = Vector3.Cross (Vector3.up,cam.transform.forward);
		Vector3 forward = Vector3.Cross (right, Vector3.up);

		Vector3 movement = Vector3.zero;

		movement += right * (Input.GetAxis ("Horizontal") * speed * Time.deltaTime);
		movement += forward * (Input.GetAxis ("Vertical") * speed * Time.deltaTime);

		rb.AddForce(movement,ForceMode.VelocityChange);

		//lock-on

		GameObject currentEnemy = getEnemy ();

		if (canLock (currentEnemy)) 
		{
			fov = calcFov (currentEnemy);
			applyLockOn (currentEnemy, fov);
		} 
		else 
		{
			resetCam ();
		}

	}

	GameObject getEnemy()
	{
		enemys = GameObject.FindGameObjectsWithTag ("Enemy");
		currentEnemy = enemys [0];
		return currentEnemy;
	}

	void resetCam()
	{
		initialPosition = new Vector3 (initialPosition.x, initialPosition.y, (transform.position.z - camOffset));
		initialRotation = Quaternion.Euler (new Vector3(convX(transform.position.x),-54f,initialRotation.z));
		fov = 45f;
		cam.fieldOfView = fov;
		cam.transform.rotation = initialRotation;
		cam.transform.position = initialPosition;
	}

	float getEnemyDist(GameObject enemy)
	{
		float enemyDistance = Mathf.Abs(transform.position.z - enemy.transform.position.z);

		enemyDistance *= zOffset;

		return enemyDistance;
	}

	bool canLock(GameObject enemy)
	{
		bool canLock = false;

		float enemyDistance = getEnemyDist (enemy);

		if (enemyDistance > maxDist && Input.GetButton("Submit")) 
		{
			canLock = false;
		} 
		else if (enemyDistance < maxDist && Input.GetButton("Submit"))
		{
			canLock = true;
		} 
		else if (enemyDistance > maxDist && !Input.GetButton("Submit"))
		{
			canLock = false;
		}
		else if (enemyDistance < maxDist && !Input.GetButton("Submit"))
		{
			canLock = false;
		}

		return canLock;
	}

	float calcFov(GameObject enemy)
	{
		float newFov = 45f;

		float enemyDistance = getEnemyDist (enemy);

		newFov = enemyDistance * fovConv;

		if (newFov > maxFov) {
			newFov = maxFov;
		} 
		else if (newFov < minFov) {
			newFov = minFov;
		}

		return newFov;
	}

	float convX(float xPosition)
	{
		xPosition *= xConv;
		xPosition += xDiff;

		return xPosition;
	}

	void applyLockOn(GameObject enemy, float fov)
	{
		cam.fieldOfView = fov;
		camRotation = new Vector3(camXOffset, convX(transform.position.x) , 0f);
		enemyPos = new Vector3(enemy.transform.position.x,cam.transform.position.y,(transform.position.z - camOffset));
		lockRotation = Quaternion.Euler (camRotation);
		cam.transform.rotation = lockRotation;
		//Vector3.SmoothDamp(cam.transform.position,enemyPos,ref velocity,smoothTime);
		Vector3.Lerp (cam.transform.position,enemyPos,Time.deltaTime);
		//cam.transform.position = enemyPos; 
	}
}