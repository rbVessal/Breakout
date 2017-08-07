using UnityEngine;
using System.Collections.Generic;

public class DoubleBallSizePowerUp : BallPowerUp 
{
	public static void Activate(ref List<GameObject>balls)
	{
		foreach(GameObject ball in balls)
		{
			Ball ballScript = ball.GetComponent<Ball>();
			int doubleBallRadiusPowerUp = (int)PowerUpTypes.DoubleBallRadius;
			if(ballScript.powerUp != doubleBallRadiusPowerUp)
			{
				ball.transform.localScale = new Vector3(ball.transform.localScale.x * 2,
				                                        ball.transform.localScale.y * 2,
				                                        ball.transform.localScale.z * 2);
				ballScript.powerUp = (int)PowerUpTypes.DoubleBallRadius;
			}
		}
	}
	
	public static AudioClip GetAudioClip()
	{
		return Resources.Load("Sounds/powerUp") as AudioClip;
	}	
}
