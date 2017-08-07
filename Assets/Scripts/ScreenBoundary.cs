using UnityEngine;
using System.Collections;

public class ScreenBoundary : MonoBehaviour 
{
	//Get the screen boundary
	public static Boundary GetBoundary()
	{
		Vector3 topRightScreenBoundary = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));
		Vector3 bottomLeftScreenBoundary = Camera.main.ScreenToWorldPoint(Vector3.zero);
		return new Boundary(bottomLeftScreenBoundary.x, 
		                    bottomLeftScreenBoundary.y, 
		                    topRightScreenBoundary.x, 
		                    topRightScreenBoundary.y);
	}

}
