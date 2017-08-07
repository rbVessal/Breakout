using UnityEngine;
using System.Collections;

public class GoldenBar : MonoBehaviour 
{

	//To be called when a brick is destroyed
	public delegate void GoldenBarDestroyedAction(GameObject goldenBar);
	public static event GoldenBarDestroyedAction GoldenBarDestroyed;

	void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag == "Ball")
		{
			GoldenBar.GoldenBarDestroyed(this.gameObject);
			Destroy(this.gameObject);
		}
	}
}
