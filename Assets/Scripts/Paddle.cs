using UnityEngine;
using System.Collections;

public class Paddle : MonoBehaviour 
{
	public float speed = 1.0f;
	float maxSpeed;
	float minSpeed;

	void Start()
	{
		MeshRenderer meshRender = this.GetComponent<MeshRenderer>(); 
		Vector3 maxVelocity = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0.0f, 0.0f));
		maxSpeed = maxVelocity.x - meshRender.bounds.extents.x;
		Vector3 minVelocity = Camera.main.ScreenToWorldPoint (new Vector3(0.0f, 0.0f, 0.0f));
		minSpeed = minVelocity.x + meshRender.bounds.extents.x;
	}

	public Vector3 StartingPosition
	{
		get 
		{
			Vector3 halfWayBottomWorldPoint = Camera.main.ScreenToWorldPoint(new Vector3((Screen.width/2), 0.0f, 0.0f));
			MeshRenderer paddleMeshRender = this.GetComponent<MeshRenderer>(); 
			return new Vector3(halfWayBottomWorldPoint.x, 
		                        halfWayBottomWorldPoint.y + paddleMeshRender.bounds.extents.y, 
		                        0.0f);
		}
	}

	public Vector3 Size
	{
		get { return this.GetComponent<MeshRenderer>().bounds.size;}
	}
	
	// Update is called once per frame
	void Update () 
	{
		float velocity = transform.position.x + (Input.GetAxis("Horizontal") * speed);
		velocity = Mathf.Clamp(velocity, minSpeed, maxSpeed);
		transform.position = new Vector3(velocity, transform.position.y, transform.position.z);
	}
}
