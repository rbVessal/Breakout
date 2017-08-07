using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour 
{
	//To be called when a ball is destroyed
	public delegate void DestroyedAction(GameObject ball);
	public static event DestroyedAction Destroyed;

	private float initialSpeed = 400.0f;
	private bool hasLaunched;
	private Rigidbody rigidBody;
	private TrailRenderer trailRenderer;
	private Boundary worldScreenBoundary;
	private float worldRadius;
	private AudioSource collisionSoundEffect;
	public int powerUp;
	void Awake()
	{
		collisionSoundEffect = GetComponent<AudioSource>();
		collisionSoundEffect.enabled = true;
		powerUp = -1;
		hasLaunched = false;
		rigidBody = GetComponent<Rigidbody>();
		SphereCollider sphereCollider = GetComponent<SphereCollider>();
		Vector3 worldRadiusVector = sphereCollider.transform.TransformPoint(new Vector3(sphereCollider.radius, 0.0f, 0.0f));
		worldRadius = worldRadiusVector.x;
		worldScreenBoundary = ScreenBoundary.GetBoundary();
		trailRenderer = GetComponentInChildren<TrailRenderer>();
		trailRenderer.enabled = false;

		//Move the ball up by its radius to ensure the ball is centered over
		//the paddle
		this.transform.position = new Vector3(this.transform.position.x,
		                                      this.transform.position.y + sphereCollider.radius,
		                                      this.transform.position.z);
	}

	public Vector3 CenterPositionOverRelativeTarget(GameObject target)
	{
		return new Vector3(target.transform.position.x, 
		            target.transform.position.y + target.GetComponent<MeshRenderer>().bounds.extents.y,
		            0.0f);
	}

	//This gets called before any physics is applied
	void FixedUpdate()
	{
		if(Input.GetKeyDown(KeyCode.Space) && !hasLaunched)
		{
			transform.parent = null;
			rigidBody.isKinematic = false;
			rigidBody.angularDrag = 0.0f;
			rigidBody.drag = 0.0f;
			rigidBody.AddForce(new Vector3(initialSpeed, initialSpeed, 0.0f));
			hasLaunched = true;
			trailRenderer.enabled = true;
		}

		//Check if the ball hit the top, left, or right screen boundaries
		//If it did, then reflect based on which screen boundary the ball collided
		//with.
		float rightSideOfBall = rigidBody.position.x + worldRadius;
		float leftSideOfBall = rigidBody.position.x - worldRadius;
		float topSideOfBall = rigidBody.position.y + worldRadius;
		float bottomSideOfBall = rigidBody.position.y - worldRadius;
		if(rightSideOfBall > worldScreenBoundary.right)
		{
			float differenceX = rightSideOfBall - worldScreenBoundary.right;
			Reflect (new Vector3(differenceX, 0.0f, 0.0f));
		}
		else if(leftSideOfBall < worldScreenBoundary.left)
		{
			float differenceX = leftSideOfBall - (worldScreenBoundary.left);
 			Reflect (new Vector3(differenceX, 0.0f, 0.0f));
		}
		else if(topSideOfBall > worldScreenBoundary.top)
		{
			float differenceY = topSideOfBall - worldScreenBoundary.top;
			Reflect (new Vector3(0.0f, differenceY, 0.0f));
		}
		//If the ball collided with the bottom part of the screen,
		//then destroy this ball
		else if(bottomSideOfBall < worldScreenBoundary.bottom)
		{
			Ball.Destroyed(this.gameObject);
			Destroy(this.gameObject);
		}
	}

	void Reflect(Vector3 difference)
	{
		//Get the ball out of the collision area
		rigidBody.MovePosition(new Vector3(rigidBody.position.x - difference.x, rigidBody.position.y - difference.y, rigidBody.position.z));
		//Then apply the reverse velocity component
		if(difference.x != 0)
		{
			rigidBody.velocity = new Vector3(-rigidBody.velocity.x, rigidBody.velocity.y, 0.0f);
		}
		if(difference.y != 0)
		{
			rigidBody.velocity = new Vector3(rigidBody.velocity.x, -rigidBody.velocity.y, 0.0f);
		}

		collisionSoundEffect.Play();
	}

	void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag == "Paddle")
		{
			collisionSoundEffect.Play ();
		}
		else if(collision.gameObject.tag == "Brick")
		{
			collisionSoundEffect.PlayOneShot(Brick.GetCollisionAudioClip());
		}
		else if(collision.gameObject.tag == "GoldenBar")
		{
			collisionSoundEffect.PlayOneShot(DoubleBallSizePowerUp.GetAudioClip());
		}
	}
}
