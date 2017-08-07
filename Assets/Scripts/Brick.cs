using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour 
{
	//To be called when a brick is destroyed
	public delegate void BrickDestroyedAction(GameObject brick);
	public static event BrickDestroyedAction BrickDestroyed;

	void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag == "Ball")
		{
			//Broadcast  that a brick was destroyed
			Brick.BrickDestroyed(this.gameObject);
			//Then destroy the object
			Destroy(this.gameObject);
		}
	}

	public static AudioClip GetCollisionAudioClip()
	{
		return Resources.Load("Sounds/crumbling") as AudioClip;
	}
}
